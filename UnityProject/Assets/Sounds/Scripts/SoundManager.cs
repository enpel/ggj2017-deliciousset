using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public partial class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	const string SoundRootPath = "Sounds/";
	const string BgmPath = "Bgm/";
	const string SePath = "Se/";

	public const bool EnableStreamingAssets = false;
	public bool _skipSound;

#pragma warning disable 649
	[SerializeField] private AudioSourceManager[] _audioSourceManagers;
#pragma warning restore 649

	[SerializeField] private AudioMixer _mixer;
	private Dictionary<string, AudioClip> _clipCaches = new Dictionary<string, AudioClip>();

	public AudioClip GetClip(SoundId id)
	{
		var path = GetClipPath(id);
		if (!_clipCaches.ContainsKey(path))
		{
			var clip = SoundUtil.LoadClip(path);
			_clipCaches.Add(path, clip);
		}
		return _clipCaches[path];
	}

	public string GetClipPath(SoundId id)
	{
		switch (id.ToSoundType())
		{
			case SoundType.Bgm:
				return SoundRootPath + BgmPath + id.ToResourceName();
			case SoundType.Se:
				return SoundRootPath + SePath + id.ToResourceName();
			default:
				return null;
		}
	}

	public void ClearClipCache()
	{
		_clipCaches.Clear();
	}

	public AudioClip PreLoad(SoundId id)
	{
		return GetClip(id);
	}

	//複数先読み
	public IEnumerator PreLoadAsync(SoundId id)
	{
		yield return StartCoroutine(PreLoadAsyncProgress(id));
	}

	public void PreLoadAsync(SoundId startId, SoundId endId)
	{
		var num = endId - startId + 1;
		if (num < 0) return;
		for (var i = 0; i < num; i++)
		{
			var id = startId + i;
			StartCoroutine(PreLoadAsyncProgress(id));
		}
	}

	public void PreLoadAsync(List<SoundId> soundIds)
	{
		foreach(var id in soundIds)
		{
			StartCoroutine(PreLoadAsyncProgress(id));
		}
	}

	public IEnumerator PreLoadAsyncProgress(SoundId id, Action<AudioClip> finishAction = null)
	{
		var path = GetClipPath(id);
		//キャッシュにあるオーディオなら
		if (_clipCaches.ContainsKey(path))
		{
			var clip = _clipCaches[path];
			if (finishAction != null)
			{
				finishAction.Invoke(clip);
			}
		}
		else
		{
			/*
			if(EnableStreamingAssets)
			{
				yield return StartCoroutine(SoundUtil.LoadStreamingClipAsync(path, clip => {
					if(clip != null)
					{
						if (!_clipCaches.ContainsKey(path))
						{
							_clipCaches.Add(path, clip);
						}
						finishAction.Exec<AudioClip>(clip);
					}
				}));
			}
			*/
			yield return StartCoroutine(SoundUtil.LoadClipAsync(path, clip => {
				if (!_clipCaches.ContainsKey(path))
				{
					_clipCaches.Add(path, clip);
				}
				if (finishAction != null)
				{
					finishAction.Invoke(clip);
				}
			}));
		}
	}

	public IEnumerator PreLoadAsyncProgress(SoundId startId, int num)
	{
		//一気に投げたほうが早いので、一気にロードを投げる
		int count = 0;
		for (var i = 0; i < num; i++)
		{
			var id = startId + i;
			StartCoroutine(PreLoadAsyncProgress(id,(clip)=>{
				count++;
			}));
		}

		while(count < num)
		{
			yield return null;
		}
	}

	public IEnumerator PreLoadAsyncProgress(List<SoundId> soundIds)
	{
		int count = 0;
		var num = soundIds.Count;

		foreach(var id in soundIds)
		{
			StartCoroutine(PreLoadAsyncProgress(id,(clip)=>{
				count++;
			}));
		}

		while(count < num)
		{
			yield return null;
		}
	}

	public void Play(SoundId id, float delayTime,Action<SoundController> playedAction = null)
	{
		var callTime = Time.time;
		StartCoroutine(PreLoadAsyncProgress(id,(clip)=>{
			var currentTime = Time.time;
			delayTime = Mathf.Max(0,delayTime - (currentTime - callTime));
			this.Delay(()=>{
				var sc= Play(id);
				if(playedAction == null) return;
				playedAction(sc);
			},delayTime);
		}));
	}

	public SoundController Play(SoundId id)
	{
		if (id == SoundId.None) return null;
		if (_skipSound) return null;

		var soundType = id.ToSoundType();
		var loop = id.IsLoop();
		var volume = id.ToDefaultVolume();

		float? seekTime = null;
		float? fadeTime = null;

		if (soundType == SoundType.Bgm)
		{
			var oldBgm = GetMainBgmSoundController();
			if (oldBgm == null)
			{
				seekTime = id.SinglePlayStartTime();
			}
			else
			{
				fadeTime = 0.4f;
				//前の曲との繋ぎの組み合わせによるシーク時間算出
				seekTime = id.CrossFadeSeekTime(oldBgm);

				if (id == oldBgm._soundId)
				{
					Debug.Log(string.Format("既に[{0}]を再生していたため、鳴らし直しの再生は行いません。再生中のサウンドコントローラーを返却します。", id.ToDispString()));
					return oldBgm;
				}
			}
		}
		else if(soundType == SoundType.Se)
		{
			var sameSe = GetPrevSoundController(id);
			if(sameSe !=null)
			{
				sameSe.FadeAndDestroy(0.1f);
			}
		}

		var clip = GetClip(id);

		var sc = _audioSourceManagers[(int)soundType].Play(clip,loop,fadeTime);
		/*
		if(id == SoundId.NouveauMonde && seekTime == null)
		{
			seekTime = 1.4117f;
			fadetime = 0;
		}
		*/

		/*
		if(soundType == SoundType.Se)
		{
			sc.ChangePitch(1.025f);
		}
		*/
		sc.SetVolume(volume,true);
		if(seekTime != null)
		{
			sc.SeekTime(seekTime.Value);
		}
		if(fadeTime != null)
		{
			sc.FadeIn(fadeTime.Value);
		}
		sc._soundId = id;
		return sc;
	}

	private void SetMixerVolume(SoundType type,float value)
	{
		var t = value;
		var max = 0;
		var min = -25;
		var threshold = 0.05f;
		if (t < threshold)
		{
			max = min;
			min = -80;			//-80dbはAudioMixerではミュート扱い。
			t = t/threshold;
		}
		_mixer.SetFloat(type+"Volume", Mathf.Lerp(min, max, t));
	}

	public void PauseMainBgm(bool pause)
	{
		var ac = GetMainBgmAudioSource();
		if(ac != null)
		{
			if(pause)
			{
				ac.Pause();
			}
			else
			{
				ac.UnPause();
			}
		}
	}

	public void PauseLoopSound(bool pause)
	{
		foreach(var asm in _audioSourceManagers)
		{
			var scs = asm.GetLoopSoundController();
			if(scs == null) continue;
			foreach(var sc in scs)
			{
				if(pause)
				{
					sc._audioSource.Pause();
				}
				else
				{
					sc._audioSource.UnPause();
				}
			}
		}
	}

	public AudioSource GetMainBgmAudioSource()
	{
		return _audioSourceManagers[(int) SoundType.Bgm].GetFirstAudioSource();
	}

	public SoundController GetMainBgmSoundController()
	{
		return _audioSourceManagers[(int)SoundType.Bgm].GetFirstSoundController();
	}

	public List<SoundController> GetExistBgmSoundControllers()
	{
		return _audioSourceManagers[(int)SoundType.Bgm].GetSoundControllers();
	}

	public SoundId GetMainBgmId()
	{
		var sc = GetMainBgmSoundController();
		return sc._soundId;
	}

	//若干の時間を持たせてBgm止める(破棄)
	public void StopBgm(float sec = 0.5f)
	{
		var sc = GetMainBgmSoundController();
		if(sc == null) return;
		sc.FadeAndDestroy(sec);
	}

	//若干の時間を持たせてSeを止める(破棄)
	public void StopSe(float sec = 0.5f)
	{
		var scs = _audioSourceManagers[(int)SoundType.Se].GetSoundControllers();
		foreach(var sc in scs)
		{
			sc.FadeAndDestroy(sec);
		}
	}

	public SoundController GetPrevSoundController(SoundId id)
	{
		var soundType = id.ToSoundType();
		var audioSourceManager = _audioSourceManagers[(int)soundType];

		var soundControllers = audioSourceManager.GetAliveSoundControllers();

		foreach(var sc in soundControllers)
		{
			if(sc._soundId == id) return sc;
		}

		return null;
	}
	
	public List<SoundController> GetSeSoundControllers()
	{
		return _audioSourceManagers[(int)SoundType.Se].GetSoundControllers();
	}

	public void SeekBgm(float rate)
	{
		var sc = GetMainBgmSoundController();
		if (sc == null) return;
		sc.Seek(rate);
	}

	public float GetBgmTimeRate()
	{
		var sc = GetMainBgmSoundController();
		return sc == null ? 0 : sc.TimeRate();
	}
}