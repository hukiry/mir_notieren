using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace Hukiry.SDK
{
    [System.Serializable]
    public class InappScriptableData : ScriptableObject
    {

        public List<ProductDetails> details = new List<ProductDetails>();
#if UNITY_EDITOR
        [UnityEditor.MenuItem("CONTEXT/InappScriptableData/ResetAppStoreSettingsAsset", false)]

        private static void ResetData()
        {
            InappScriptableData inappScriptableData = CreateData();
            UnityEditor.EditorUtility.SetDirty(inappScriptableData);
        }

        private static InappScriptableData CreateData()
        {
            InappScriptableData inappScriptableData = LoadingScriptableObject();
            if (inappScriptableData == null)
            {
                inappScriptableData = CreateInstance<InappScriptableData>();
            }
            inappScriptableData.details.Clear();
            for (int i = 0; i < 10; i++)
            {
                ProductDetails d = new ProductDetails();
                if (i != 9)
                {
                    d.amount = (i * 100 + 99).ToString();
                    inappScriptableData.details.Add(d);
                }

                d = new ProductDetails();
                d.amount = (i * 1000 + 999).ToString();
                inappScriptableData.details.Add(d);
            }
            inappScriptableData.details.Sort((n, m) => int.Parse(n.amount) - int.Parse(m.amount));
            return inappScriptableData;
        }

        private static InappScriptableData LoadingScriptableObject()
        {
            return UnityEditor.AssetDatabase.
                    LoadAssetAtPath<InappScriptableData>(UnityEditor.AssetDatabase.
                    FindAssets($"t:{nameof(InappScriptableData)}").
                    Select(x => UnityEditor.AssetDatabase.
                    GUIDToAssetPath(x)).
                    First(p => Path.
                    GetFileNameWithoutExtension(p) ==
                    nameof(InappScriptableData)));
        }

        public static void CreateAppStoreSettingsAsset()
        {
            InappScriptableData inappScriptableData = CreateData();
            //string path = HukiryUtilEditor.FindAssetPath<UnityEditor.MonoScript>("InappScriptableData").Replace("InappScriptableData.cs", "InappScriptableData.asset");
            string path = UnityEditor.AssetDatabase.
                FindAssets($"t:{nameof(UnityEditor.MonoScript)}").
                Select(x => UnityEditor.AssetDatabase.
                GUIDToAssetPath(x)).
                First(p => Path.
                GetFileNameWithoutExtension(p) ==
                nameof(InappScriptableData)).Replace("InappScriptableData.cs", "InappScriptableData.asset");
            UnityEditor.AssetDatabase.CreateAsset(inappScriptableData, path);
        }


#endif

        internal List<ProductDetails> GetActiveList()
        {
            return details.Where(p => p.isEnable).ToList();
        }

        public List<string> GetProductList()
        {
            var array = this.GetActiveList();
            List<string> temp = new List<string>();
            foreach (var item in array)
            {
                temp.Add($"{item.product_id}{item.amount}");
            }
            return temp;
        }
    }

    [System.Serializable]
    public class ProductDetails
    {
        public string product_id;
        public string amount;
        public bool isEnable;
    }
}