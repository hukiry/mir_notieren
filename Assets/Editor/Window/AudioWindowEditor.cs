using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[InitializeOnLoad]
public class AudioWindowEditor : ResourcesWindowEditor<AudioWindowEditor>
{

	public class AudioClipData : BaseData
	{
		public AudioClipLoadType loadType;
		public bool preloadAudioData;
		public AudioCompressionFormat compressionFormat;
		public int quality;
		public AudioSampleRateSetting sampleRateSetting;
	}

	public static void OpenAudioEditor()
	{
		_Instance.OpenWindowEditor<AudioClipData>(Selection.assetGUIDs, new AudioClipData());
	}

	public override bool FileFilterExtension(string extName)
	{
		return extName == ".wav" || extName == ".mp3" || extName == ".ogg" || extName == ".aif" || extName == ".midi" || extName == ".wma";
	}

	public override string DrawGUI(BaseData data)
	{
		var audioClipData = data as AudioClipData;

#if UNITY_2018_4_OR_NEWER || UNITY_2018_4_5

		audioClipData.loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup("Load type", audioClipData.loadType);

		audioClipData.preloadAudioData = EditorGUILayout.Toggle("PreloadAudioData", audioClipData.preloadAudioData);

		audioClipData.compressionFormat = (AudioCompressionFormat)EditorGUILayout.EnumPopup("CompressionFormat", audioClipData.compressionFormat, "DropDown");

		audioClipData.quality = EditorGUILayout.IntField("Quality", audioClipData.quality);

		audioClipData.sampleRateSetting = (AudioSampleRateSetting)EditorGUILayout.EnumPopup("SampleRateSetting", audioClipData.sampleRateSetting, "DropDown");
#endif
		return "保存文件";
	}

	public override string WindowTitileName()
	{
		return "音频";
	}
	public override string ExcuteData(BaseData data, EditorBaseData filePath)
	{
		var audioClipData = data as AudioClipData;

		AudioImporter audioImporter = AssetImporter.GetAtPath(filePath.filePath) as AudioImporter;

#if UNITY_2018_4_OR_NEWER || UNITY_2018_4_5

		AudioImporterSampleSettings sample = audioImporter.defaultSampleSettings;

		sample.loadType = audioClipData.loadType;

		sample.quality = audioClipData.quality;

		sample.sampleRateSetting = audioClipData.sampleRateSetting;

		sample.compressionFormat = audioClipData.compressionFormat;

		audioImporter.defaultSampleSettings = sample;

		audioImporter.preloadAudioData = audioClipData.preloadAudioData;

#endif
		return audioImporter.assetPath;
	}
}
