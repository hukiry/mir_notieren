using UnityEditor;

#if UNITY_ANDROID

[InitializeOnLoad]
public class AndroidKeystoreSign
{
    static AndroidKeystoreSign()
    {

#if UNITY_2019_2_OR_NEWER
        PlayerSettings.Android.useCustomKeystore = true;
#endif
        //路径包含后缀名
        PlayerSettings.Android.keystoreName = "KeyStore/LuckyGrass_lucky444syc.keystore";
        PlayerSettings.Android.keystorePass = "lucky444syc";
        //alias or Pass
        PlayerSettings.Android.keyaliasName = "LuckyGrass";
        PlayerSettings.Android.keyaliasPass = "lucky444syc";

    }
}
#endif
