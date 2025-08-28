using System;
using UnityEngine;

namespace Hukiry.SDK
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class AppleLoginVerify
    {
        private const string urlKeys = "https://appleid.apple.com/auth/keys";
        private const string urlToken = "https://appleid.apple.com/auth/token";
        private const string urlRevoke = "https://appleid.apple.com/auth/revoke";
        private const string grant_type = "authorization_code";
        private const string token_type_hint = "access_token";

        //bundle ID
        private string client_id = @"com.yiliu.HappyMatch";
        //生成的密匙
        private string client_secret = @"eyJraWQiOiI4NTRBVFlSSjJIIiwiYWxnIjoiRVMyNTYifQ.eyJpc3MiOiJKODhLSEZaV1Y2IiwiaWF0IjoxNjkzNjMzMTcyLCJleHAiOjE3MDkxODUxNzIsImF1ZCI6Imh0dHBzOi8vYXBwbGVpZC5hcHBsZS5jb20iLCJzdWIiOiJjb20ueWlsaXUuSGFwcHlNYXRjaCJ9.YOMxyIhoHMq18gtewuRPOUaO1VIlaROpQoZHa4jgaJbgnbZpU7qXINiHzMjGcLmVf4fgCdxlnAdbLyvbx9f0Qg";
        /// <summary>
        /// 设置客户端登录密匙
        /// </summary>
        /// <param name="client_id">包名</param>
        /// <param name="client_secret">登录密匙</param>
        public void SetClient(string client_id, string client_secret)
        {
            if (!string.IsNullOrEmpty(client_id?.Trim()))
                this.client_id = client_id;

            if (!string.IsNullOrEmpty(client_secret?.Trim()))
                this.client_secret = client_secret;
        }

        /// <summary>
        ///每次登录，请求token，缓存access_token
        ///生成和验证令牌  POST https://appleid.apple.com/auth/token
        ///HTTP主体 form-data服务器验证授权代码或刷新令牌所需的输入参数列表。内容类型：application/x-www-form-urlencoded
        ///client_id:必填）应用程序的标识符（应用程序ID或服务ID）。标识符不得包含您的团队ID，以帮助防止将敏感数据暴露给最终用户的可能性。
        ///client_secret:（必填）由开发人员生成的秘密JSON Web令牌，使用与您的开发人员帐户关联的Apple私钥登录。授权代码和刷新令牌验证请求需要此参数。发送到您的应用程序的授权响应中收到的授权代码。该代码仅限一次性使用，有效期为五分钟。授权代码验证请求需要此参数。
        ///code:（必填）每次登录时获取
        ///grant_type:（必填）授予类型决定了客户端应用程序如何与验证服务器交互。授权代码和刷新令牌验证请求需要此参数。对于授权代码验证，请使用authorization_code。对于刷新令牌验证请求，请使用refresh_token。
        /// </summary>
        /// <param name="authorizationCode">每次登录都会生成</param>
        /// <param name="callBack"></param>
        public void RequstToken(string authorizationCode, Action<bool, string> callBack)
        {
            WWWForm form = new WWWForm();
            form.AddField("client_id", client_id);
            form.AddField("client_secret", client_secret);
            form.AddField("code", authorizationCode);
            form.AddField("grant_type", grant_type);
            new UwrPostAppleWWW(urlToken, form, callBack);
        }

        /// <summary>
        /// 撤销令牌 POST https://appleid.apple.com/auth/revoke
        /// HTTP主体:form-data服务器使令牌无效所需的输入参数列表。内容类型：application/x-www-form-urlencoded
        /// <see cref=""/>client_id:必填）应用程序的标识符（应用程序ID或服务ID）。标识符必须与用户信息的授权请求期间提供的值相匹配。此外，标识符不得包含您的团队ID，以帮助减少向最终用户暴露敏感数据的可能性。
        /// client_secret:（必填）一个秘密的JSON Web令牌（JWT），它使用与您的开发人员帐户关联的Apple私钥登录。有关创建客户端密钥的更多信息，请参阅生成和验证令牌。
        /// token:（必填）打算撤销的用户刷新令牌或访问令牌。如果请求成功，与提供的令牌关联的用户会话将被撤销。
        /// token_type_hint:关于提交撤销的令牌类型的提示。使用refresh_token或access_token。
        /// </summary>
        /// <param name="token">登录时请求的token</param>
        /// <param name="callBack"></param>
        public void RequstRevoke(string token, Action<bool, string> callBack)
        {
            WWWForm form = new WWWForm();
            form.AddField("client_id", client_id);
            form.AddField("client_secret", client_secret);
            form.AddField("token", token);
            form.AddField("token_type_hint", token_type_hint);
            new UwrPostAppleWWW(urlRevoke, form, callBack);
        }

        public void RequstKeys(Action<bool, string> callBack)
        {
            new UwrGetAppleJson(urlKeys, callBack);
        }

        //private string EncodeBase64(string str)
        //{
        //    byte[] buffer = Encoding.UTF8.GetBytes(str);
        //    return System.Convert.ToBase64String(buffer);
        //}

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        //private string DecodeBase64(string base64)
        //{
        //    byte[] buffer = System.Convert.FromBase64String(base64);
        //    return Encoding.UTF8.GetString(buffer);
        //}
    }
}
