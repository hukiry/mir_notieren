using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class GenerateMenu 
{
	static private void ProcessCmd(string name, bool _wait = true)
	{
		Process proc = new Process();
		proc.StartInfo.FileName = Application.dataPath.Replace("Assets", "") + "Tools/" + name;
		proc.Start();
		if (_wait)
			proc.WaitForExit();
        else proc.Close();
    }

	[MenuItem("Hukiry/Export/1.生成Lua协议", false, 1)]
	static public void CreateLuaProtocol()
	{
		ProcessCmd("Protobuf/Protobuf.exe");
		AssetDatabase.Refresh();
	}
	
	[MenuItem("Hukiry/Export/2.导出策划配置表", false, 2)]
	static public void ExcelExportChart()
	{
		ProcessCmd("ExcelExportChart/ExcelExportChartTool.exe");
		AssetDatabase.Refresh();
	}
}
