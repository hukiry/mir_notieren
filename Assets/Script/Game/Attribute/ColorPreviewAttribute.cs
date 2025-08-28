using System;
using UnityEngine;

[AttributeUsageAttribute(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class ColorPreviewAttribute : PropertyAttribute
{
    public string materialName;
    public string text;
    public ColorPreviewAttribute(string text, string materialName)
    {
        this.text = text;
        this.materialName = materialName;
    }
}