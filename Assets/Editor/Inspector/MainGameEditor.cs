using System.Collections.Generic;
using UnityEngine;
namespace UnityEditor
{
    [CustomEditor(typeof(MainGame), true)]
    public class MainGameEditor : Editor
    {
        MainGame client;
        private Dictionary<EWorkMode, string> dic = new Dictionary<EWorkMode, string>() {
            { EWorkMode.Debug,"https 模式：内网ip地址；出包模式链接外网，无资源热更新"}    ,
            { EWorkMode.Develop,"https 模式：外网ip地址；出包模式链接外网，资源热更新"},
            { EWorkMode.Release,"https 模式：正式ip地址，出包模式全部正式"}
        };

        private void OnEnable()
        {
            client = target as MainGame;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (dic.ContainsKey(client.WorkMode))
            {
                GUILayout.Label(dic[client.WorkMode]);
            }
        }
    }
}
