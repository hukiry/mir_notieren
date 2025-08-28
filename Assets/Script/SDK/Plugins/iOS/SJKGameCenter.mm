//
//  MyIos_sdk.m
//  Unity-iPhone
//
//  Created by clyde on 2020/4/30.
//


#import "SJKGameOC.h"
#import "SJKGameCenter.h"
#import "AppleHelper.h"
//#import "GoogleHelper.h"
//#import "PayAppleHelper.h"
//#import "FbHelper.h"
//#import "GoogleAdMob/AdMob.h"

const char * _maskcpy(const char *ch)
{
    if (ch==nil) {
        return nil;
    }
    char*str = (char*)malloc(strlen(ch)+1);
    strcpy(str, ch);
    return str;
}


#if defined(__cplusplus)
extern "C"{
#endif
extern void _InitSDK(const char * gameObjectName, const char * UnityFunctionName,const char * sdkFunctionName, const char * sdkJsonParamKey);
extern void _CallSDKParam(UnityParam param);
extern void _CallSDKFunction(int funType, const char *json);
extern const char * _GetSDK(int funType);
// 接受C#层传过来的函数指针
extern void _RegeditCallFunction(iOSCallBackFunction iOSCallBack);

static iOSCallBackFunction _unityCallBack = NULL;
#if defined(__cplusplus)
}
#endif


// 接受C#层传过来的函数指针
void _RegeditCallFunction(iOSCallBackFunction iOSCallBack)
{
	_unityCallBack = iOSCallBack;
}

void _CallSDKParam(UnityParam param)
{
    UnityParam* data=&param;
    if (data->funType == Login_Func) {
        [[AppleHelper instance] login];
    }
    else if (data->funType == Logout_Func)
    {
        [[AppleHelper instance] logOut];
    }
}

void _InitSDK(const char * gameObjectName, const char * UnityFunctionName,const char * sdkFunctionName, const char * sdkJsonParamKey)
{
    NSString* UnityGameObject=[NSString stringWithUTF8String:_maskcpy(gameObjectName)];
    NSString* MethodName=[NSString stringWithUTF8String:UnityFunctionName];
    NSString* FunctionName=[NSString stringWithUTF8String:sdkFunctionName];
    NSString* JsonDatakey=[NSString stringWithUTF8String:sdkJsonParamKey];
    NSLog(@"%@,%@,%@,%@",UnityGameObject,MethodName,FunctionName,JsonDatakey);
    [[SJKGameOC instance] IntSdk:UnityGameObject param1:MethodName param2:FunctionName param3:JsonDatakey ];
//    [[GoogleHelper instance] configure];
}

void _CallSDKFunction(int funType, const char *json)
{
   NSString *strJson=[NSString stringWithUTF8String:json];
   bool isvalid=[NSJSONSerialization isValidJSONObject:strJson];
   if(isvalid)
   {
       NSData *data=[NSJSONSerialization dataWithJSONObject:strJson options:NSJSONReadingAllowFragments  error:nil];
       NSLog(@"%@\t%@",strJson,data);
   }
   else{
       NSDictionary* dic = [SJKGameOC getJsonDic:strJson];
//       NSString *stringParam1 = [[dic objectForKey:@"stringKey1"] stringValue];
//       bool boolKey1 = [[dic objectForKey:@"boolKey1"] boolValue];
//       NSLog(@"function type=%d\t json param is = %@",funType, strJson);
//       if (funType == Login_Func) {
//          [[AppleHelper instance] login];
//       }
//       if (funType == Login_Func) {
//           NSInteger index = [[dic objectForKey:@"intKey1"] integerValue];
//           if (index == 1) {
//               //apple 登陆
//               [[AppleHelper instance] login];
//           }
//           else if(index==2)
//           {
//               //google登录
//               [[GoogleHelper instance] login:true];
//           }
          
//       }
//       else if(funType == BindAccount_Func) {
//           NSInteger index = [[dic objectForKey:@"intKey1"] integerValue];
//           if (index == 1) {
//               //apple 登陆
//               [[AppleHelper instance] login];
//           }
//           else if(index==2)
//           {
//               //google登录
//               [[GoogleHelper instance] login:true];
//           }
//
//       }
//       else if(funType == Logout_Func)
//       {
//           NSInteger index = [[dic objectForKey:@"intKey1"] integerValue];
//           if(index==1)
//           {
//               [[AppleHelper instance] logOut];
//           }
//           else if (index==2)
//           {
//               [[GoogleHelper instance] logOut];
//           }
//       }
//       else if(funType == CustomsEventUp_Func)
//       {
//           NSString *eventName = [[dic objectForKey:@"stringKey1"] stringValue];
////           [[GoogleHelper instance] logEvent:eventName];
//       }
//       else if(funType == 901)
//       {    UnityParam param;
//           param.funType=funType;
//           param.jsonParams = _maskcpy([@"测试结果" UTF8String]);
//           param.errorCode =200;
//           param.errorMsg = _maskcpy("null");
//           _unityCallBack(param);
//       }
//       elseif (funType == AdInit_Func) {
//           [[AdMob instance] initAd:strJson];//广告初始化
//       }
//       else if (funType == AdRewardFetch_Func)
//       {
//           NSInteger index = [[dic objectForKey:@"intKey1"] integerValue];
//           [[AdMob instance] showRewardedVideo:index];//广告展示播放
//       }
   }
    
}

const char * _GetSDK(int funType)
{
    NSString *shResult = @"";

        NSString *sh=[[NSNumber numberWithInt:funType] description];
        shResult = [NSString stringWithFormat:@"{\"funType\":%@}",sh];
    
    const char *ch = [shResult UTF8String];
    return _maskcpy(ch);
}

//-------------------------------------------------------------------------
//send error message to Unity
void SendToUnity(int funType ,const char *msg, int code)
{
    struct UnityParam param;
    param.funType=funType;
    param.errorCode=code;
    param.errorMsg = _maskcpy(msg);
    param.jsonParams = NULL;
    if(_unityCallBack!=NULL)
    {
        _unityCallBack(param);
    }
  
}

//send message to Unity
void SendToUnity(int funType ,const char *jsonData)
{
    struct UnityParam param;
    param.funType=funType;
    param.jsonParams = jsonData;
    param.errorCode=0;
    param.errorMsg =NULL;
    if(_unityCallBack!=NULL)
    {
        _unityCallBack(param);
    }
}
