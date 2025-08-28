using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public static class PutAliyunSdk
{
    public class AliyunSdkData
    {
        public Action<bool> callFinish;
        public string urlkey;
    }
    static string bucketName = "";
    static string endpoint;
    static string accessKeyId;
    static string accessKeySecret;

    static AutoResetEvent _event = new AutoResetEvent(false);
    static OssClient m_client = null;

    /// <summary>
    /// 初始化文件路径
    /// </summary>
    /// <param name="endpoint">区域</param>
    /// <param name="accessKeyId">key</param>
    /// <param name="accessKeySecret">KeySecret</param>
    /// <param name="bucketN">桶名</param>
    public static void Init(string endpoint1, string accessKeyId1, string accessKeySecret1, string bucketN)
    {
        endpoint = endpoint1;
        accessKeyId = accessKeyId1;
        accessKeySecret = accessKeySecret1;
        bucketName = bucketN;
    }

    /// <summary>
    /// 上传资源
    /// </summary>
    /// <param name="urlkey">上传文件地址</param>
    /// <param name="data">数据</param>
    /// <param name="callFinish">完成回调</param>
    public static void PutUploadUrl(string urlkey, string data)
    {
        byte[] binaryData = Encoding.ASCII.GetBytes(data);
        PutUploadUrl(urlkey, binaryData, binaryData.Length);
    }

    /// <summary>
    /// 异步上传资源
    /// </summary>
    /// <param name="urlkey">上传文件地址</param>
    /// <param name="binaryData">数据</param>
    /// <param name="callFinish">完成回调</param>
    public static void PutUploadUrl(string urlkey, byte[] binaryData, int len, Action<bool> callFinish = null)
    {
        AsyncPutObject(urlkey, binaryData, len, callFinish);
    }

    private static void AsyncPutObject(string urlkey, byte[] binaryData, int len, Action<bool> callFinish)
    {
        try
        {
            // 初始化OssClient
            if (m_client == null)
            {
                m_client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            }
            byte[] binary = new byte[len];
            Array.Copy(binaryData, 0, binary, 0, binary.Length);
            var stream = new MemoryStream(binary);

            var metadata = new ObjectMetadata();
            metadata.CacheControl = "No-Cache";
            metadata.ContentType = "text/html";

            AliyunSdkData temp = new AliyunSdkData()
            {
                callFinish = callFinish,
                urlkey = urlkey
            };
            m_client.BeginPutObject(bucketName, urlkey, stream, metadata, PutObjectCallback, temp);
            _event.WaitOne();
        }
        catch (Exception ex)
        {
            LogManager.Log("Put object failed, {0}", ex.Message);
        }
    }

    private static void PutObjectCallback(IAsyncResult ar)
    {
        AliyunSdkData data = ar?.AsyncState as AliyunSdkData;
        try
        {
            var result = m_client.EndPutObject(ar);
            data?.callFinish(true);
            LogManager.Log(string.Format("Put object:{0} succeeded", data.urlkey));
        }
        catch (OssException ex)
        {
            LogManager.LogError(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
            data?.callFinish(false);
        }
        catch (Exception ex)
        {
            data?.callFinish(false);
            LogManager.Log("Put object failed, {0}", ex.Message);
        }
        finally
        {
            _event.Set();
        }
    }

    /// <summary>
    /// 异步删除资源
    /// </summary>
    /// <param name="urlkey">上传文件地址</param>
    /// <param name="callFinish">完成回调</param>
    public static void DeleteUploadUrl(string urlkey, Action<bool> callFinish = null)
    {
        _DeleteUploadUrl(urlkey, callFinish);
    }

    private static void _DeleteUploadUrl(string urlkey, Action<bool> callFinish = null)
    {
        try
        {
            OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            client.DeleteObject(bucketName, urlkey);
            LogManager.Log(string.Format("delete object:{0} succeeded", urlkey));
            callFinish?.Invoke(true);
        }
        catch (OssException ex)
        {
            LogManager.LogError(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
            callFinish?.Invoke(false);
        }
        catch (Exception ex)
        {
            LogManager.LogError(string.Format("Failed with error info: {0}", ex.Message));
            callFinish?.Invoke(false);
        }
    }
}