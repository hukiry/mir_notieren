using System;
[AttributeUsage(AttributeTargets.Field| 
    AttributeTargets.Parameter | 
    AttributeTargets.Enum|
    AttributeTargets.Property ,
    AllowMultiple = true,
    Inherited =false)]
public  class FieldNameAttribute : Attribute
{
    public string name;
    public string tipText;
    public string icon;
    public int type;
    public FieldNameAttribute(string name)
    {
        this.name = name.Trim();
    }

    /// <summary>
    /// 编辑器特性
    /// </summary>
    /// <param name="name">字段化名</param>
    /// <param name="icon">编辑器加载时的图片精灵</param>
    public FieldNameAttribute(string name,string icon)
    {
        this.name = name;
        this.icon = icon;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">字段名称</param>
    /// <param name="isKey">设置当前字段为唯一键</param>
    public FieldNameAttribute(string name,string tipText, string icon)
    {
        this.name = name;
        this.icon = icon;
        this.tipText = tipText;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">字段名称</param>
    /// <param name="type">区别字段类型</param>
    public FieldNameAttribute(string name, int type)
    {
        this.name = name;
        this.type = type;
    }
}
