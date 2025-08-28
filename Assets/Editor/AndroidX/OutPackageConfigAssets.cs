using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Hukiry/OutPackageConfigAssets", order = 4)]

public class OutPackageConfigAssets : CommonAssets<OutPackageConfigAssets>
{
    [Description("AndroidStudio工程路径")]
    public string androidProjectPath;
    [Description("AndroidStudio资源路径")]
    public string androidResourcePath;
    [Description("apk命令参数")]
    public string cmdApkparams;
    [Description("aab命令参数")]
    public string cmdAabParams;
    [Description("aar命令参数")]
    public string cmdAarParams;

    [Description("修改工程的目录")]
    public string changeDir;

    public List<string> GetDirNames()
    {
        if (string.IsNullOrEmpty(changeDir))
        {
            Debug.LogError("changeDir 配置参数不能为空");
            return default;
        }
        return new List<string>(this.changeDir.Split(';', ',', '|'));
    }

    /// <summary>
    /// 获取出包命令行参数
    /// </summary>
    public string GetCmdParams(bool isAab) => isAab ? cmdAabParams: cmdApkparams;
}
