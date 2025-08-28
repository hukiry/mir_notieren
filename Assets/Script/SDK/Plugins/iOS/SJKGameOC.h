//
//  MyIos_sdk.m
//  Unity-iPhone
//
//  Created by clyde on 2020/4/30.
//

#import <Foundation/Foundation.h>
@interface SJKGameOC : NSObject
typedef NS_ENUM(NSInteger, ECodeError)
{
    successful_code=0,
    fail_code = -1,
    paramError_code = -2,
    expError_code = -3,
};

typedef NS_ENUM(NSInteger, SdkFunctionCallType) {
	///网络不可见
	NetworkUnavailable_Func = 100,
    //init SDK
    Init_Func = 101,//初始化
    InitSucces_Func = 102,
    InitFail_Func = 103,

    //login SDK
    Login_Func = 201,
    LoginSucces_Func = 202,
    LoginFail_Func = 203,
    LoginCancel_Func = 204,
    //撤销登陆
    LoginToken_Func = 205,
    LoginRevoke_Func = 206,
    //logout sdk
    Logout_Func = 301,
    LogoutSucces_Func = 302,
    LogoutFail_Func = 303,

    //pay SDK
    PayFetch_Func = 401,
    PaySucces_Func = 402,
    PayFail_Func = 403,
    PayCancel_Func = 404,
    //支付凭证验证成功：可以发货
    PayVerifyRecipeSucces_Func = 405,
    //支付凭证验证失败
    PayVerifyRecipeFail_Func = 406,
    // 初始化商品列表拉起
    InitProductFetch_Func = 501,
    // 初始化商品列表成功
    InitProductSucces_Func = 502,
    // 初始化商品列表失败
    InitProductFail_Func = 503,

    //绑定账号
    BindAccount_Func = 601,
    BindAccountSucces_Func = 602,
    BindAccountFail_Func = 603,
    BindAccountCancel_Func = 604,

    //ad init
    AdInit_Func = 701,
    AdInitSucces_Func = 702,
    AdInitFail_Func = 703,

    //ad fectch
    AdRewardFetch_Func = 711,
    AdRewardSucces_Func = 712,
    AdRewardFail_Func = 713,

	//离线通知
    LocalNotification_Func = 901,
    //other
    //获取系统语言代码
    GetSystemLanguage_Func = 1,
    //获取App版本号
    GetAppVersionName_Func = 2,
    //更新SDK 语言
    UpdateLanguage_Func = 3,
    UserAgreement_Func = 4,
    CustomsEventUp_Func = 5,
};

+(SJKGameOC*) instance;
+(NSDictionary*)getJsonDic:(NSString*)jsonString;
+(NSString*)arrayToJson:(NSMutableDictionary *)dic;
+(NSString*)nsSetToJson:(NSMutableSet *)set;
+(NSString*)nsArrayToJson:(NSMutableArray *)array;

-(void) SendUnityMessage:(SdkFunctionCallType)funType msg:(NSString *)msg codeType:(ECodeError)code;
-(void) SendMessage:(SdkFunctionCallType)funType json:(NSString *)jsonData;
-(void) IntSdk:(NSString*)objectName param1:(NSString *)functionName param2:(NSString *)callFunckey param3:(NSString *)jsonkey;
@end

static NSString *UnityGameObject=@"";
//find method name  is on gameobject
static NSString *MethodName=@"";

//is same the name with C# by side
static NSString *FunctionName=@"";
static NSString *JsonDatakey=@"";
