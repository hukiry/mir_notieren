//
//  MyIos_sdk.h
//  Unity-iPhone
//
//  Created by clyde on 2020/4/30.
//

#import <Foundation/Foundation.h>


@interface SJKGameCenter:NSObject
@end

//结构体，每一个参数都必须赋值，可以为空
typedef struct UnityParam
{
    int funType;
    const char*jsonParams;
    int errorCode;
    const char*errorMsg;
}UnityParam;

//定义函数指针
typedef void (*iOSCallBackFunction)(struct UnityParam json);

void SendToUnity(int funType ,const char *msg, int code);
void SendToUnity(int funType ,const char *jsonData);
