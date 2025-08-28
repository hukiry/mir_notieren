using System;


[AttributeUsage(AttributeTargets.Field , Inherited = false, AllowMultiple = false)]
public class DescriptionAttribute : UnityEngine.PropertyAttribute
{
	public string name;
	public Type enumType;
	public DescriptionAttribute(string name)
	{
		this.name = name;
	}

	/// <summary>
	/// 自定义枚举字段描述
	/// </summary>
	/// <param name="name">显示名称</param>
	/// <param name="enumType">枚举类型</param>
	public DescriptionAttribute(string name, Type enumType)
	{
		this.name = name;
		this.enumType = enumType;
	}
}