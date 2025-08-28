using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class PostprocessBuildListener :Editor, IPostprocessBuildWithReport, IPostBuildPlayerScriptDLLs,
     IProcessSceneWithReport, IPreprocessShaders, IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        //LogManager.Log("OnPreprocessBuild");
    }
    public void OnPostBuildPlayerScriptDLLs(BuildReport report)
    {
        //LogManager.Log("OnPostBuildPlayerScriptDLLs！");
    }
    //public static bool isBuildFinished = false;
    public void OnPostprocessBuild(BuildReport report)
    {
        //isBuildFinished = true;
        var summary = report.summary;
        var strippingInfo = report.strippingInfo;
        var files = report.files;
        var steps = report.steps;

        List<string> Buidlst = new List<string>();
        Buidlst.Add("基本部署：");
        Buidlst.Add("\t输出包路径:\t" + summary.outputPath);
        Buidlst.Add("\t打包平台:\t" + summary.platform.ToString());
        Buidlst.Add("\t打包成功状态:\t" + summary.result.ToString());
        Buidlst.Add("\t打包目标平台:\t" + summary.platformGroup.ToString());
        Buidlst.Add("\t打包总时间:\t" + summary.totalTime);
        Buidlst.Add("\t打包开始时间:\t" + summary.buildStartedAt.ToLongTimeString());
        Buidlst.Add("\t打包结束时间:\t" + summary.buildEndedAt.ToLongTimeString());

        if (strippingInfo != null && strippingInfo.includedModules != null)
        {
            Buidlst.Add("\r\n----------------------------------------------------------------------------");
            Buidlst.Add("脚本模块：");

            foreach (var item in strippingInfo.includedModules)
            {
                Buidlst.Add(item);
            }
        }

        Buidlst.Add("\r\n----------------------------------------------------------------------------");
        Buidlst.Add("部署文件：");
        foreach (var item in files)
        {
            Buidlst.Add(Path.GetFileNameWithoutExtension(item.path));
            Buidlst.Add("\t path:\t" + item.path);
            Buidlst.Add("\t role:\t" + item.role);
            Buidlst.Add("\t size:\t" + GetSizeToStr(item.size));
            Buidlst.Add("\n");
        }
  
        Buidlst.Add("\r\n----------------------------------------------------------------------------");
        Buidlst.Add("部署顺序：");
        foreach (var item in steps)
        {
            Buidlst.Add("\t name:\t" + item.name);
            Buidlst.Add("\t duration:\t" + item.duration);
            Buidlst.Add("\t depth:\t" + item.depth);
            Buidlst.Add("\t messages:\t");
            if (item.messages != null && item.messages.Length > 0)
            {
                int length = item.messages.Length;
                for (int i = 0; i < length; i++)
                {
                    Buidlst.Add("\t\t type:\t" + item.messages[i].type);
                    Buidlst.Add("\t\t content:\t" + item.messages[i].content);
                }
            }
            Buidlst.Add("\n");
        }

       

        File.WriteAllLines("build.txt", Buidlst.ToArray());

		LogManager.LogColor("green","Build Finished！");
	}

    private string GetSizeToStr(ulong size)
    {
        ulong kb = size / 1024;
        ulong m = kb / 1024;
        ulong g = m / 1024;
        if (g > 0)
        {
            return string.Format("{0:f3}G", (float)size / 1024f / 1024f / 1024f);
        }
        else if (m > 0)
        {
            return string.Format("{0:f3}M", (float)size / 1024f / 1024f);
        }
        else if (kb > 0)
        {
            return string.Format("{0:f3}kB", (float)size / 1024f );
        }
        else
        {
            return string.Format("{0}字节", size);
        }
    }

    public void OnProcessScene(Scene scene, BuildReport report)
    {
       
        //LogManager.Log("OnProcessScene！",scene);
    }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        //LogManager.Log("OnProcessShader！");
    }
}
