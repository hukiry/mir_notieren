package com.five.mergegame;

public class UnityParam {
    // 函数类型
    public int funType;
    /// 列表=json格式，否则直接传参
    public String jsonParams;
    public int errorCode;
    public String errorMsg;

    public UnityParam()
    {
        this.funType = 0;
        this.jsonParams = "";
        this.errorMsg = "";
        this.errorCode = 0;
    }

}