using System;
using UnityEngine;
namespace HukiryInitialize
{
    [Serializable]
    public class GameVersion
    {
        /// <summary>
        /// 资源版本
        /// </summary>
        public string version = "1.0.0";
        /// <summary>
        /// 资源热更新地址，拼接地址
        /// <see cref="PathConifg.AssetsUrlPath"/>
        /// </summary>
        public string webUrl = string.Empty;
        /// <summary>
        /// 从服务器下载ip地址
        /// </summary>
        public string jsonUrl = string.Empty;
        /// <summary>
        /// 出包模式
        /// </summary>
        public int workMode;
        /// <summary>
        /// 强更地址
        /// </summary>
        public string strongerUrl = string.Empty;
        /// <summary>
        /// App版本，累加且不可修改
        /// </summary>
        public int appVersion;


        public string GetUpdatePackageVersionName()
        {
            string versionDirName = string.Empty;
            if (string.IsNullOrEmpty(versionDirName))
            {
                string[] packs = version.Split('.');
                if (packs.Length == 3)
                {
                    int large = int.Parse(packs[0]);
                    int midle = int.Parse(packs[1]);
                    versionDirName = $"v{large}.{midle}";
                }
                else
                {
                    LogManager.LogError("version params is not empty：", version);
                }
            }
            return versionDirName;
        }

        /// <summary>
        /// 是否更新app，不强更新
        /// </summary>
        /// <param name="packApp">包内版本</param>
        /// <param name="cacheApp">缓存或服务器版本</param>
        /// <returns></returns>
        public static bool IsUpdateApp(int packApp, int cacheApp)
        {
            return cacheApp > packApp;
        }

        /// <summary>
        /// 资源版本对比，强更新包
        /// </summary>
        /// <param name="packApp">包内版本</param>
        /// <param name="cacheApp">缓存或服务器版本</param>
        /// <returns></returns>
        public static bool IsUpdatePackVersion(string packVersion, string cacheVersion)
        {
            if (packVersion == null || cacheVersion == null)
            {
                LogManager.LogError("params is not empty：", packVersion, cacheVersion);
                return false;
            }
            string[] packs = packVersion.Split('.');
            string[] caches = cacheVersion.Split('.');
            if (packs.Length == 3 && caches.Length == 3)
            {
                int packInt = int.Parse(packs[0]) * 1000 + int.Parse(packs[1]);
                int cacheInt = int.Parse(caches[0]) * 1000 + int.Parse(caches[1]);
                return packInt > cacheInt;
            }
            else
            {
                LogManager.LogError("异常错误：", packVersion, cacheVersion);
                return false;
            }
        }

        /// <summary>
        /// 资源版本比较，更新资源
        /// </summary>
        /// <param name="packApp">包内版本</param>
        /// <param name="cacheApp">缓存或服务器版本</param>
        /// <returns></returns>
        public static bool IsUpdateResourceVersion(string serverVersion, string cacheVersion)
        {
            if (serverVersion == null || cacheVersion == null)
            {
                LogManager.LogError("参数不能为空：", serverVersion, cacheVersion);
                return false;
            }
            string[] servers = serverVersion.Split('.');
            string[] caches = cacheVersion.Split('.');
            if (servers.Length == 3 && caches.Length == 3)
            {
                int serverInt = int.Parse(servers[0]) * 1000 + int.Parse(servers[1]);
                int serverSmall = int.Parse(servers[2]);

                int cacheInt = int.Parse(caches[0]) * 1000 + int.Parse(caches[1]);
                int cacheSmall = int.Parse(caches[2]);

                return serverInt == cacheInt && (serverSmall > cacheSmall);
            }
            else
            {
                LogManager.LogError("异常错误：", serverVersion, cacheVersion);
                return false;
            }
        }
    }
}