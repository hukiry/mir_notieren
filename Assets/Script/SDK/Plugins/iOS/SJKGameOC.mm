//
//  MyIos_sdk.m
//  Unity-iPhone
//
//  Created by clyde on 2020/4/30.
//


#import "SJKGameOC.h"
#import "SJKGameCenter.h"

@implementation SJKGameOC

static SJKGameOC* _instance = nil;
+ (SJKGameOC*)instance
{
    @synchronized(self.class)
    {
        if (_instance == nil) {
            _instance = [[self.class alloc] init];
        }
        return _instance;
    }
}

-(void) IntSdk:(NSString*)objectName param1:(NSString *)functionName param2:(NSString *)callFunckey param3:(NSString *)jsonkey
{
    UnityGameObject = objectName;
    MethodName = functionName;
    FunctionName = callFunckey;
    JsonDatakey = jsonkey;
}

-(void) SendUnityMessage:(SdkFunctionCallType)funType msg:(NSString *)msg codeType:(ECodeError)code
{
    NSNumber *number = [NSNumber numberWithInteger:funType];
    NSNumber *numberCode = [NSNumber numberWithInteger:code];
    SendToUnity(number.intValue, msg.UTF8String, numberCode.intValue);
//    NSNumber *numberCode = [NSNumber numberWithInteger:code];
//    NSMutableDictionary * dmessage=[[NSMutableDictionary alloc] init];
//    [dmessage setValue:numberCode forKey:@"code"];
//    [dmessage setValue:msg forKey:@"msg"];
//    NSString *jsonStr= [SJKGameOC arrayToJson: dmessage];
//    [self SendMessage:funType json:jsonStr];
}

-(void) SendMessage:(SdkFunctionCallType)funType json:(NSString *)jsonData
{
    NSNumber *number = [NSNumber numberWithInteger:funType];
    SendToUnity(number.intValue, jsonData.UTF8String);
//    NSNumber *number = [NSNumber numberWithInteger:funType];
//    NSLog(@"%@",number.description);
//    NSMutableDictionary * d=[[NSMutableDictionary alloc] init];
//    [d setValue:number forKey:FunctionName];
//    [d setValue:jsonData forKey:JsonDatakey];
//    NSString *str= [SJKGameOC arrayToJson: d];
//    const char * jsonParam = [str UTF8String];
//    NSLog(@"%@,%@,%@,%@",UnityGameObject,MethodName,FunctionName,JsonDatakey);
//    UnitySendMessage([UnityGameObject UTF8String],[MethodName UTF8String], jsonParam);
}

//json字符串转化成字典
+(NSDictionary*)getJsonDic:(NSString*)jsonString{

    NSData* jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    return [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableLeaves error:nil];
}

//字典转化成json字符串
+(NSString*)arrayToJson:(NSMutableDictionary *)dic
{
    NSError *parseError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:NSJSONWritingPrettyPrinted error:&parseError];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

}

+(NSString*)nsSetToJson:(NSMutableSet *)set
{
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:[set allObjects] options:NSJSONWritingPrettyPrinted error:&error];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

+(NSString*)nsArrayToJson:(NSMutableArray *)array{
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:array options:NSJSONWritingPrettyPrinted error:&error];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}
@end
