using UnityEngine;
using UnityEditor;

public sealed class CustomAudioImporter : AssetPostprocessor
{
	public override int GetPostprocessOrder()
	{
		return 0;
	}

	//TODO
	void OnPreprocessAudio()
	{
		if(assetPath.Contains("Sounds"))
		{
			SetVorbisQuality(assetPath.Contains("Bgm") ? 0.25f : 0.37f);
		}
	}

	void SetVorbisQuality(float quality)
	{
		var audioImporter = assetImporter as AudioImporter;
		var aiss = new AudioImporterSampleSettings();
		aiss.compressionFormat = AudioCompressionFormat.Vorbis;
		aiss.quality = quality;
		aiss.loadType = AudioClipLoadType.CompressedInMemory;
		audioImporter.defaultSampleSettings = aiss;
	}
}