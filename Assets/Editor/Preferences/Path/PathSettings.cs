using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class PathSettings : ProjectSettingsScriptable<PathSettings>
{
    [Tooltip("Lua代码目录生成路径")]
	[FieldName("Lua")]
	public string LuaCodeDirPath;
	[Tooltip("C#代码目录生成路径")][FieldName("C#")]
	public string CharpCodeDirPath;

    public override void Refresh()
    {
        
    }
}
