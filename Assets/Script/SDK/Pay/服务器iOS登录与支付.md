# iOS 服务器SDK接入
## **一，[服务器登录验证](https://appleid.apple.com/auth/token)**
- 1，client_secret 服务器生成客户端的密匙，用于登录验证：自行查询百度即可
- 2，client_id :应用包名（com.xxx.xx）
- 3，code : 前端登录成功获得参数-拿给服务器
- 4，grant_type: 固定参数authorization_code 或 refresh_token 
    ~~~C#
    生成和验证令牌  POST https://appleid.apple.com/auth/token
    HTTP主体 form-data服务器验证授权代码或刷新令牌所需的输入参数列表。内容类型：application/x-www-form-urlencoded
        -> client_id:必填）应用程序的标识符（应用程序ID或服务ID）。标识符不得包含您的团队ID，以帮助防止将敏感数据暴露给最终用户的可能性。
        -> client_secret:（必填）由开发人员生成的秘密JSON Web令牌，使用与您的开发人员帐户关联的Apple私钥登录。授权代码和刷新令牌验证请求需要此参数。发送到您的应用程序的授权响应中收到的授权代码。该代码仅限一次性使用，有效期为五分钟。授权代码验证请求需要此参数。
        -> code:（必填）每次登录时获取[客户端获取的token]
        -> grant_type:（必填）授予类型决定了客户端应用程序如何与验证服务器交互。授权代码和刷新令牌验证请求需要此参数。对于授权代码验证，请使用authorization_code。对于刷新令牌验证请求，请使用refresh_token。    
    ~~~

> 登录验证用到URL
- Apple公钥接口:https://appleid.apple.com/auth/keys
    ~~~json
    后端的验证方法，是使用Apple提供的 公共密钥，去解析授权登录的Id_token
    ~~~
- 授权Apple登录: https://appleid.apple.com/auth/token

- 撤销Apple登出: https://appleid.apple.com/auth/revoke

## **二，[服务器登出验证](https://appleid.apple.com/auth/revoke)**  
### 当前端发起登出通知后，服务器需即刻执行。
-
    ~~~Json
    撤销令牌 POST https://appleid.apple.com/auth/revoke
    HTTP主体:form-data服务器使令牌无效所需的输入参数列表。内容类型：application/x-www-form-urlencoded
        -> client_id:必填）应用程序的标识符（应用程序ID或服务ID）。标识符必须与用户信息的授权请求期间提供的值相匹配。此外，标识符不得包含您的团队ID，以帮助减少向最终用户暴露敏感数据的可能性。
        -> client_secret:（必填）一个秘密的JSON Web令牌（JWT），它使用与您的开发人员帐户关联的Apple私钥登录。有关创建客户端密钥的更多信息，请参阅生成和验证令牌。
        -> token:（必填）打算撤销的用户刷新令牌或访问令牌。如果请求成功，与提供的令牌关联的用户会话将被撤销。
        -> token_type_hint:关于提交撤销的令牌类型的提示。使用refresh_token或access_token。
    ~~~

## **三，[服务器支付凭证验证](https://buy.itunes.apple.com/verifyReceipt)**
- 1，沙盒环境: https://sandbox.itunes.apple.com/verifyReceipt
    ~~~
    -> 审核时，必须要支持沙盒测试，否则不通过审核。
    -> 测试账号，必须添加到开发者证书上,审核通过后，才可以测试。
    -> 开发者自己支付测试，必须等审核通过，否则提示没有此商品ID。
    ~~~
- 2，正式环境: https://buy.itunes.apple.com/verifyReceipt
    ~~~
    -> 审核时，要默认用正式环境去验证支付。如果返回的错误码是：21007 ，则动态改用沙河路径去验证。其他错误码，直接判断为支付失败。
    ~~~

- 3，验证POST 提交格式： Json

    |参数      |必须 |类型| 描述|
    | --        | -- | -- | -- |
    |receipt-data |√|string|前端支付成功的凭证
    |password     |√|string|Apple开发者后台生成的密匙
    |||

    ~~~
     凭证验证成功后，就可以发货了。
    ~~~

- 4，App Store错误码如下：

    |错误码 | 错误原因|
    | --  |  -- |
    |21000|  App Store无法读取你提供的JSON数据
    |21002|  收据数据不符合格式
    |21003|  收据无法被验证
    |21004|  你提供的共享密钥和账户的共享密钥不一致
    |21005|  收据服务器当前不可用
    |21006|  收据是有效的，但订阅服务已经过期。当收到这个信息时，解码后的收据信息也包含在返回内容中
    |21007|  收据信息是测试用（sandbox），但却被发送到产品环境中验证
    |21008|  收据信息是产品环境中使用，但却被发送到测试环境中验证
    |21003|  解决方案：将共享密钥也传给苹果去验证
    |||

- 5，服务器的回调地址：apple后台可以配置服务器Url 沙盒回调环境、正式回调环境。无游戏服务器，则不需要配置

> **服务器登录授权码的验证-参考：[https://zhuanlan.zhihu.com/p/632483498](https://zhuanlan.zhihu.com/p/632483498)**

~~~
*注意：正式环境：错误码是 21007 需要去请求沙河路径。否则无法通过审核。
~~~