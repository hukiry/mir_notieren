
 ## 运营需要准备的参数如下：
 ---
|参数|必须|名称|类型|描述|
|--|:--:|--|--|--|
|Team ID      |√   |团队ID    |String|所有项目的ID都是相同的，每年会自动更新一次。
|Client ID    |√   |包名      |String|项目包名-App Bundle ID 例如：com.xx.xxx
|Key ID       |√   |密文Key   |String|Apple开发者在后台创建的一个服务id。
|key File     |√   |密文      |File|由开发者创建的服务ID，在Servers中配置key ，并下载此pm文件
|Pay password |√   |共享密匙  |string|需要开发者在后台生成一次，每个项目都需要创建。
||||

**以上所有参数，需要全部给游戏服务器使用。**

---

- > 一，根钥匙串创建
    - 1，在xcode上登录账号

    - 2，先打开钥匙串软件，接着下载证书，然后双击安装
        - [https://developer.apple.com/certificationauthority/AppleWWDRCA.cer](https://developer.apple.com/certificationauthority/AppleWWDRCA.cer)
        - [https://www.apple.com/certificateauthority/AppleWWDRCAG3.cer](https://www.apple.com/certificateauthority/AppleWWDRCAG3.cer)
    
    - 3, 钥匙串-证书颁发
         - 公司账号选择: 创建证书颁发机构
         - 个人账号选择: 创建证书颁发

    - 4，创建key：用于生成客户端密匙，登录时需要授权的参数
        - 地址：https://developer.apple.com/account/resources/identifiers/list
        - 点击keys->创建

    - 5，生成共享密匙
        - 点击app信息-> 下拉到最底部，有一个app专用共享密钥，然后点击生成

- > 二，创建项目
    - 创建证书套件（自定义描述、包名com.jhspdks.merge） 
    - 创建项目：游戏名称自定义
        - sku（包名）
        - 语言（美区）

- > 三，添加测试设备<font color=red size=4>优先配置，开发时的设备调试</font>
    - 1，设备链接电脑，通过xcode 或者 助手软件查看唯一设备ID
    - 2, 将设备码配置到开发者后台，才可以创建证书，否则出包无法测试。
    - 3，操作：https://developer.apple.com/account/resources/identifiers/list
点击Devices-->添加测试设备

- > 四，添加测试人员<font color=red size=4>优先配置，开发时登录、支付测试</font>
    - 1，测试账号不分地区，正式环境需要分。
    - 2，测试账号添加到开发者后台，设置为其他人员权限，添加到测试列表
    - 3，测试账号，需要打开被邀请邮箱同意测试，即可在TestFlight上下载。
    - 4，操作：https://appstoreconnect.apple.com/ 用户和访问，添加账号，然后testflight创建内部测试

- > 五，开发证书创建： [必须用Mac设备，且安装过最新Xcode电脑生成，否则证书无效]()
    - 先在 xcode 上>账号>证书管理，添加开发者证书和发布证书，然后回到apple后台创建描述文件
    - 测试证书-iOS App Development: 用于开发者App Bug调试，必须创建。
    - 发布证书-App Store：用于上线提包审核，必须创建。
    - 操作：https://developer.apple.com/account/resources/identifiers/list 点击profiles，创建并添加测试设备，下载证书。

- > 六，内购配置:<font color=red size=4>优先配置，开发时的商品ID</font>
    - 1，必须配置本地化语言（英语地区的需要全部配置，支付才可以通过审核，否则直接退回）
    - 2，必须上传审核的内购商品截图
    - 3，必须设置内购出售地区（推荐全球包括国内，海外上线只要发布地区不是国内即可）
    - 4，提交app时，内购需要一起提交，否则审核绝不通过。且还需添加引导审核文件，否则apple审核人员不知道内购商品在哪里，最终会被退回。

- > 七，提包测试
    - 1，提交到 testflight 必须用 mac上的 Connect 软件提交，每次提交必须准备好原装电源线，否则无法提交成功。
    - 2，在Connect登录开发者账号，提交之后。需要等15~30分钟，一般首次提交至少30分钟，后面都是15分钟左右。完成提交后，开发者需要添加到可测试列表。
    - 3，如果之前没有发布版本通过，那么是无法测试支付的（必须通过送审），只能测试登录。

- > 八，提包送审
    - 1，送审时：配置相关参数，五图，地区设置，加上内购。
    - 2，需要添加引导截图+说明文字，方便给审核人员来审五图、登录、支付功能，否则会立马退回（被问各个模块在哪里出现）
---

## * 注意：如果前面没有通过配置，后面则无法执行下一步（缺少参数，无法登录验证或支付验证，无法审核通过等等）


