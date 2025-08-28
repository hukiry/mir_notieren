
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class JsonUtil {
    private readonly static string KEY_64 = "";
    private readonly static string IV_64 = "";
    static JsonUtil()
    {
        KEY_64 = "!~@#$%^&*()_+12";
        IV_64 = "!~@#$%^&*()_+12";
    }
    #region JsonUtility
    [Serializable]
    private class Hukiry<T>
    {
        public T[] value;
    }

    public static string ToJsonUnity<T>(T[] o)
    {
        Hukiry<T> temp = new Hukiry<T>()
        {
            value = o
        };
        return JsonUtility.ToJson(temp);
    }

    public static string ToJsonUnity<T>(List<T> o)
    {
        Hukiry<T> temp = new Hukiry<T>()
        {
            value = o.ToArray()
        };
        return JsonUtility.ToJson(temp);
    }

    public static string ToJsonUnity<T>(T o)
    {
        return JsonUtility.ToJson(o);
    }

    public static T[] ToArrayUnity<T>(string str)
    {
        Hukiry<T> temp = JsonUtility.FromJson<Hukiry<T>>(str);
        return temp.value;
    }

    public static T ToObjectUnity<T>(string str)
    {
        return JsonUtility.FromJson<T>(str);
    }
    #endregion

	public static T ToObject<T>(string jsStr)
	{
		T t = JsonConvert.DeserializeObject<T>(jsStr);
		return t;
	}

	public static string ToJson(object o) {
		return JsonConvert.SerializeObject(o);
	}

  

    public static string Encode(string data) {
		byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
		byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		int i = cryptoProvider.KeySize;
		MemoryStream ms = new MemoryStream();
		CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
		StreamWriter sw = new StreamWriter(cst);
		sw.Write(data);
		sw.Flush();
		cst.FlushFinalBlock();
		sw.Flush();
		return Convert.ToBase64String(ms.GetBuffer(), 0, ( int )ms.Length);
	}

	public static string Decode(string data) {
		byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
		byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
		byte[] byEnc;
		try {
			byEnc = Convert.FromBase64String(data);
		} catch {
			return null;
		}

		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		MemoryStream ms = new MemoryStream(byEnc);
		CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
		StreamReader sr = new StreamReader(cst);
		return sr.ReadToEnd();
	}
    //public static Transform FindSkeletonInChildren(Transform ts, String name)
    //{
    //    Queue<Transform> q = new Queue<Transform>();
    //    q.Enqueue(ts);
    //    while (q.Count > 0)
    //    {
    //        Transform t = q.Dequeue();
    //        if (t.name == name)
    //            return t;
    //        int childCount = t.childCount;
    //        for (int i = 0; i < childCount; i++)
    //        {
    //            q.Enqueue(t.GetChild(i));
    //        }
    //    }
    //    return null;
    //}

    //public static byte[] Compress(string strData)
    //{
    //    MemoryStream ms = new MemoryStream();
    //    GZipOutputStream compressedzipStream = new GZipOutputStream(ms);
    //    byte[] data = Util.stringToBytes(strData);
    //    int size = data.Length;
    //    compressedzipStream.Write(data, 0, data.Length);
    //    compressedzipStream.Finish();
    //    compressedzipStream.Close();
    //    byte[] result = ms.ToArray();
    //    //Debug.LogWarning("compress rate " + (( float )size / result.Length));
    //    return result;
    //}
    //public static string Decompress(byte[] data)
    //{
    //    int size_before = data.Length;
    //    MemoryStream ms = new MemoryStream(data);
    //    GZipInputStream compressedzipStream = new GZipInputStream(ms);
    //    MemoryStream outBuffer = new MemoryStream();
    //    byte[] block = new byte[1024];
    //    while (true)
    //    {
    //        int bytesRead = compressedzipStream.Read(block, 0, block.Length);
    //        if (bytesRead <= 0)
    //            break;
    //        else
    //            outBuffer.Write(block, 0, bytesRead);
    //    }
    //    compressedzipStream.Close();
    //    byte[] result = outBuffer.ToArray();
    //    //Debug.LogWarning("Decompress rate " + (( float )size_before / result.Length));
    //    return JsonUtil.bytesToString(result);
    //}
}
