using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioSourceManager : MonoBehaviour
{
	[SerializeField] private int _polyphony = 1;		//同時発音数
#pragma warning disable 649
	[SerializeField] private AudioMixerGroup _mixerGroup;
#pragma warning restore 649
	[SerializeField] private float _fadeTime = 0.5f;

	// ReSharper disable once UnusedMember.Local
	void Start()
	{
		//SoundController関係でバグが出そうなので、最適化は後回しにしています。
		//後にパフォーマンスが気になったら、最初にAudioSourceを作ります。
		/*
		var root = gameObject;
		for (var i = 0; i < _polyphony; i++)
		{
			
			var go = NGUITools.AddChild(root);
			go.name = string.Format("{0}{1}", root.name, i);
			var ac = go.AddComponent<AudioSource>();
			ac.outputAudioMixerGroup = _mixerGroup;
			go.AddComponent<SoundController>();
		}
		*/
	}

	int AudioNum()
	{
		return gameObject.transform.childCount;
	}

	//数が多い場合は古いオーディオを削除
	void DestroyOverOldAudio(float fadeTime)
	{
		var audioNum = AudioNum();
		if (audioNum < _polyphony) return;

		var sc = GetLowPrioritySoundController();
		if (sc == null) return;
		sc.FadeAndDestroy(fadeTime);
	}

	public SoundController Play(AudioClip clip,bool loop = false, float? fadeTime = null)
	{
		if(!fadeTime.HasValue)
		{
			fadeTime = _fadeTime;
		}
		DestroyOverOldAudio(fadeTime.Value);
		var go = loop ? PlayLoopSound(clip) : PlayOneShot(clip);
		var sc = go.AddComponent<SoundController>();
		return sc;
	}

	GameObject PlayLoopSound(AudioClip clip)
	{
		var go = CreateAudioObject(clip);
		var ac = go.GetComponent<AudioSource>();
		ac.loop = true;
		ac.Play();
		return go;
	}

	GameObject PlayOneShot(AudioClip clip)
	{
		var go = CreateAudioObject(clip);
		var ac = go.GetComponent<AudioSource>();
		ac.Play();
		Destroy(go, clip.length + 0.3f);
		return go;
	}

	GameObject CreateAudioObject(AudioClip clip)
	{
		//オーディオソースの作成
		var go = new GameObject(clip.name);
		go.transform.SetParent(gameObject.transform);
		var ac = go.AddComponent<AudioSource>();
		ac.outputAudioMixerGroup = _mixerGroup;
		ac.clip = clip;
		return go;
	}

	public AudioSource GetFirstAudioSource()
	{
		var sc = GetFirstSoundController();
		if (sc == null) return null;
		return sc.GetComponent<AudioSource>();
	}

	public SoundController GetFirstSoundController()
	{
		var scs = gameObject.GetComponentsInChildren<SoundController>();
		if (scs == null || scs.Length == 0) return null;
		return scs.FirstOrDefault(sc => sc._alive);
	}

	public SoundController GetLowPrioritySoundController()
	{
		var scs = gameObject.GetComponentsInChildren<SoundController>();
		if (scs == null || scs.Length == 0) return null;
		return scs.FirstOrDefault(sc => sc._alive);
	}

	public IEnumerable<SoundController> GetLoopSoundController()
	{
		var scs = gameObject.GetComponentsInChildren<SoundController>();
		if (scs == null || scs.Length == 0) return null;
		return scs.Where(sc => sc._audioSource.loop);
	}
	
	public List<SoundController> GetSoundControllers()
	{
		return gameObject.GetComponentsInChildren<SoundController>().ToList();
	}

	public List<SoundController> GetAliveSoundControllers()
	{
		return gameObject.GetComponentsInChildren<SoundController>().Where(sc=>sc._alive).ToList();
	}
}