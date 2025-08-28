
using System.Collections.Generic;
/// <summary>
/// 生成UI路径配置
/// </summary>
public class UIPathConfig
{
    //场景池目录路径
    public const string ScenePoolPath = "LuaScript/Library/Pool/";
    public const string UIViewPath = "LuaScript/Game/UI";
    public const string ESpriteAtlasResourcePath = "LuaScript/Game/Scene/ESpriteAtlasResource.lua";

    public static Dictionary<string, string> DirNames = new Dictionary<string, string>{
        {"Scene","View" }
    };
}
