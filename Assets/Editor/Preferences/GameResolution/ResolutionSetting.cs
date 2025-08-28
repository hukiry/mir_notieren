using Hukiry.Editor;
using System.Collections.Generic;
using UnityEditor;

public class ResolutionSetting : ProjectSettingsScriptable<ResolutionSetting>
{
    public GameViewSizeGroupType gameViewSizeGroupType;
    public List<GameResolutionSize> m_Custom = new List<GameResolutionSize>();

    private GameViewResolution gameViewResolution;

    public void OnStart()
    {
        if (gameViewResolution == null)
        {
            var group = GetGameViewSizeGroup();
            if (group != gameViewSizeGroupType)
            {
                gameViewSizeGroupType = group;
                m_Custom.Clear();
            }
            gameViewResolution = new GameViewResolution(gameViewSizeGroupType);
            if (m_Custom.Count == 0)
            {
                m_Custom = gameViewResolution.GetCustomResolution();
            }
        }
    }

    public void Remove(int index)
    {
        gameViewResolution?.Remove(index);
    }

    public override void Refresh()
    {
        foreach (var item in m_Custom)
        {
            gameViewResolution?.SetGameViewFixedResolution(item.width, item.height, item.baseText);
        }

    }

    private GameViewSizeGroupType GetGameViewSizeGroup()
    {
        GameViewSizeGroupType gameViewSizeGroupType = GameViewSizeGroupType.Standalone;
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneOSX:
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneLinux64:
                gameViewSizeGroupType = GameViewSizeGroupType.Standalone;
                break;
            case BuildTarget.iOS:
            case BuildTarget.tvOS:
                gameViewSizeGroupType = GameViewSizeGroupType.iOS;
                break;
            case BuildTarget.Android:
                gameViewSizeGroupType = GameViewSizeGroupType.Android;
                break;
            case BuildTarget.WebGL:
                gameViewSizeGroupType = GameViewSizeGroupType.WebPlayer;
                break;
            case BuildTarget.PS4:
            case BuildTarget.PS5:
                gameViewSizeGroupType = GameViewSizeGroupType.PS3;
                break;
            default:
                break;
        }
        return gameViewSizeGroupType;
    }
}
