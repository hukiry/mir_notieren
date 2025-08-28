using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class UGUIImportAtlas : AssetPostprocessor {

	private string assetShortPath = "/AtlasImages/";
	void OnPreprocessTexture() {
		TextureImporter importer = assetImporter as TextureImporter;
		if (importer != null && importer.textureType != TextureImporterType.Sprite) {
			OnImportTexture(importer);
		}
	}

    //所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
    //public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    //{
    //}

    //导入音频 音频格式压缩
    void OnPostprocessAudio()
    {
        AudioImporter importer = assetImporter as AudioImporter;
        if (importer != null )
        {
            AudioImporterSampleSettings sampleSettings = importer.defaultSampleSettings;
            sampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
            sampleSettings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
            sampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
            sampleSettings.quality = 0.3F;
            sampleSettings.conversionMode = 0;
#if UNITY_ANDROID
            string platform = "Android";
#elif UNITY_IOS || UNITY_IPHONE
            string platform = "iPhone";
#else
            string platform = "Window";
#endif
            importer.name = platform;
            importer.forceToMono = false;
            importer.preloadAudioData = true;
            importer.loadInBackground = false;
            importer.ambisonic = false;
            importer.SetOverrideSampleSettings(platform, sampleSettings);
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
    }

    public static void ImportSpriteAtlas()
    {
        List<string> linePath = Hukiry.HukiryUtilEditor.GetAssetsPath<SpriteAtlas>();
        StartImportSpriteAtlas(linePath.ToArray());
 
    }

    private void OnImportTexture(TextureImporter importer) {
		if (importer.assetPath.Contains(assetShortPath)) {
			if (importer != null) {
				string AtlasName = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(assetPath)).Name;
				importer.textureType = TextureImporterType.Sprite;
				importer.spriteImportMode = SpriteImportMode.Single;
				importer.spritePixelsPerUnit = 100;
				//importer.spritePackingTag = AtlasName;

				TextureImporterSettings textureImportSetting = new TextureImporterSettings();
				importer.ReadTextureSettings(textureImportSetting);
				textureImportSetting.spriteMeshType = SpriteMeshType.Tight;
				textureImportSetting.spriteExtrude = 1;
				textureImportSetting.spriteGenerateFallbackPhysicsShape = false;
				importer.SetTextureSettings(textureImportSetting);

				importer.mipmapEnabled = false;
				importer.isReadable = false;
				importer.wrapMode = TextureWrapMode.Clamp;
				importer.filterMode = FilterMode.Bilinear;
				importer.alphaIsTransparency = true;
				importer.alphaSource = TextureImporterAlphaSource.FromInput;
				importer.sRGBTexture = true;

            

#if UNITY_IPHONE || UNITY_IOS
                TextureImporterPlatformSettings platformSetting = importer.GetPlatformTextureSettings("iPhone");
				platformSetting.maxTextureSize = 2048;
				platformSetting.overridden = true;
                platformSetting.textureCompression = TextureImporterCompression.Compressed;
				platformSetting.format = TextureImporterFormat.ASTC_6x6;
#elif UNITY_ANDROID
                TextureImporterPlatformSettings platformSetting = importer.GetPlatformTextureSettings("Android");
				platformSetting.format = TextureImporterFormat.ASTC_6x6;
                platformSetting.overridden = true;
                platformSetting.textureCompression = TextureImporterCompression.Compressed;
                platformSetting.maxTextureSize = 2048;
#else
				TextureImporterPlatformSettings platformSetting = importer.GetPlatformTextureSettings("Standalone");
                platformSetting.maxTextureSize = 2048;
                platformSetting.textureCompression = TextureImporterCompression.Compressed;
                platformSetting.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
                platformSetting.overridden = true;
                platformSetting.textureCompression = TextureImporterCompression.Compressed;
                platformSetting.format = TextureImporterFormat.DXT5;
#endif
                importer.SetPlatformTextureSettings(platformSetting);
                //importer.SaveAndReimport();
            }
		}
	}

    //导入图集格式压缩
    private static void StartImportSpriteAtlas(string[] importedAsset, bool isProgressbar = true)
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE||UNITY_WEBGL
        int len = importedAsset.Length;
        int index = 0;
        foreach (string str in importedAsset)
        {
            index++;
            string extension = Path.GetExtension(str);
            if (extension == ".spriteatlas")
            {
                SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(str);
#if UNITY_ANDROID
                TextureImporterPlatformSettings settings = spriteAtlas.GetPlatformSettings("Android");
                settings.format = TextureImporterFormat.ASTC_RGBA_6x6;
#elif UNITY_IOS || UNITY_IPHONE
                TextureImporterPlatformSettings settings = spriteAtlas.GetPlatformSettings("iPhone");
                settings.format = TextureImporterFormat.ASTC_RGBA_5x5;
#endif
                settings.overridden = true;
                settings.textureCompression = TextureImporterCompression.Compressed;
                settings.maxTextureSize = 2048;
                settings.compressionQuality = 50;
                spriteAtlas.SetPlatformSettings(settings);
                EditorUtility.SetDirty(spriteAtlas);
                SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);

            }
            else if (extension == ".png")
            {
                TextureImporter importer = TextureImporter.GetAtPath(str) as TextureImporter;
                TextureImporterPlatformSettings platformSettings = importer.GetPlatformTextureSettings("Android");
#if UNITY_ANDROID
                if (importer.textureType != TextureImporterType.Sprite) continue;
                importer.name = "Android";
                importer.androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
                platformSettings.format = TextureImporterFormat.ASTC_RGBA_6x6;
#elif UNITY_IOS || UNITY_IPHONE
                importer.name = "iPhone";
                platformSettings.format = TextureImporterFormat.ASTC_RGBA_6x6;
#elif UNITY_WEBGL && UNITY_2021_3_OR_NEWER
                importer.name = "WebGL";
                platformSettings.name = "WebGL";
                platformSettings.format = TextureImporterFormat.ASTC_8x8;
#endif
                importer.textureCompression = TextureImporterCompression.Compressed;
                platformSettings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
                platformSettings.overridden = true;
                importer.sRGBTexture = true;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 100;
                importer.alphaIsTransparency = true;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.filterMode = FilterMode.Bilinear;
                importer.maxTextureSize = 2048;
                importer.compressionQuality = 50;
                importer.SetPlatformTextureSettings(platformSettings);
                importer.SaveAndReimport();
            }
            else if (extension == ".wav" || extension == ".mp3" || extension == ".ogg")
            {
                AudioImporter importer = AudioImporter.GetAtPath(str) as AudioImporter;
                AudioImporterSampleSettings sampleSettings = importer.defaultSampleSettings;
                sampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
                sampleSettings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
                sampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
                sampleSettings.quality = 0.3F;
                sampleSettings.conversionMode = 0;
#if UNITY_ANDROID
                string platform = "Android";
#elif UNITY_IOS || UNITY_IPHONE
                string platform = "iPhone";
#endif
                importer.name = platform;
                importer.forceToMono = false;
                importer.preloadAudioData = true;
                importer.loadInBackground = false;
                importer.ambisonic = false;
                importer.SetOverrideSampleSettings(platform, sampleSettings);
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }

            if (isProgressbar)
            {
                EditorUtility.DisplayProgressBar("导入中", str, index / (float)len);
            }
        }

        if (isProgressbar)
        {
            EditorUtility.ClearProgressBar();
        }
#endif
    }
}
