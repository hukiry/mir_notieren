---
--- HttpContentType       
--- Author : hukiry     
--- DateTime 2024/7/12 17:15   
---

---@class EHttpContentType
EHttpContentType = {
    ---普通文本
    TEXT_PLAIN = "text/plain",
    ---JSON字符串
    APPLICATION_JSON = "application/json; charset=UTF-8",
    ---未知类型(数据流)
    APPLICATION_OCTET_STREAM = "application/octet-stream",
    ---表单数据(键值对)
    WWW_FORM_URLENCODED = "application/x-www-form-urlencoded";
    ---表单数据(键值对)。编码方式为 gb2312
    WWW_FORM_URLENCODED_GB2312 = "application/x-www-form-urlencoded;charset=gb2312",
    ---表单数据(键值对)。编码方式为 utf-8
    WWW_FORM_URLENCODED_UTF8 = "application/x-www-form-urlencoded;charset=utf-8",
    ---多分部数据
    MULTIPART_FORM_DATA = "multipart/form-data",
}

---@class EHttpCode
EHttpCode = {
    Success = 0,
    Failure = 1,
    Cancel = 2,
}

