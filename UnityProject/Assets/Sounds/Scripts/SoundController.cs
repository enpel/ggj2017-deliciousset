using System;
using System.Collections;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public AudioSource _audioSource { get { return __audioSource ?? (__audioSource = GetComponent<AudioSource>()); } }
	AudioSource __audioSource;

	public bool _alive {get; private set;}  //フェードアウト中はfalseです。
	public float _adjustVolume{get;private set;}	//補正ボリューム
	public SoundId _soundId = SoundId.None;

	private const float DestroyDelayTime = 0.1f;
	private Action _destroyAction;

	// ReSharper disable once UnusedMember.Local
	void Awake()
	{
		_alive = true;
	}

	void OnDestroy()
	{
		if (_destroyAction == null) return;
		_destroyAction();
	}

	public SoundController DelayFadeAndDestroy(float delaySec = 0.5f,float fadeSec = 0.5f)
	{
		this.Delay(()=>{
			FadeAndDestroy(fadeSec);
		},delaySec);
		return this;
	}

	public SoundController FadeAndDestroy(float sec = 0.5f)
	{
		if(this == null) return this;
		Fade(sec);
		Destroy(gameObject,sec+DestroyDelayTime);
		return this;
	}
	public SoundController FadeIn(float sec = 0.5f,float targetVolume = 1.0f)
	{
		var to = GetVolume() * targetVolume;
		SetVolume(0);
		return Fade(sec, to);
	}

	public SoundController Fade(float sec = 0.5f,float targetVolume = 0.0f)
	{
		if(this == null) return this;
		StartCoroutine(FadeLoop(sec,targetVolume));
		return this;
	}

	IEnumerator FadeLoop(float sec, float targetVolume)
	{
		var time = 0.0f;
		var startVolume = _audioSource.volume;
		if (targetVolume == 0)
		{
			_alive = false;
		}
		while (time < sec)
		{
			time += Time.deltaTime;
			var t = Mathf.Min(1, (time / sec));
			_audioSource.volume = Mathf.Lerp(startVolume,targetVolume,t);
			yield return null;
		}
		_audioSource.volume = targetVolume;
	}

	public SoundController Stop()
	{
		_audioSource.Stop();
		return this;
	}

	public SoundController Pause()
	{
		_audioSource.Pause();
		return this;
	}

	public SoundController Play()
	{
		_audioSource.Play();
		return this;
	}

	public SoundController SetVolume(float volume,bool adjustVolume = false)
	{
		if(adjustVolume)
		{
			_audioSource.volume = volume;
			_adjustVolume = volume;
		}
		else
		{
			_audioSource.volume = volume * _adjustVolume;
		}
		return this;
	}

	public float GetVolume()
	{
		return _audioSource.volume;
	}

	public bool IsPlaying()
	{
		return _audioSource.isPlaying;
	}

	public SoundController SetLoop(bool loop = true)
	{
		_audioSource.loop = loop;
		return this;
	}

	//ピッチ(速度)変更(2.0fで倍速、倍音)
	public SoundController ChangePitch(float pitch)
	{
		_audioSource.pitch = pitch;
		return this;
	}

	//ピッチ(速度)変更(1を入れると鍵盤1個分高くなります)
	public SoundController ChangePitchScale(int scale)
	{
		var pitch = SoundUtil.GetPitch(scale);
		return ChangePitch(pitch);
	}

	public SoundController ChangePan(float panStereo)
	{
		_audioSource.panStereo = panStereo;
		return this;
	}

	public SoundController SeekTime(float sec)
	{
		SetSample((int)(sec * 44100));
		return this;
	}

	public SoundController Seek(float rate)
	{
		//ループを設定しているとループ外にシーク仕様とした場合エラーが出ますが、解決策がなさそうなのでそのままにしてます。
		SetSample((int)(rate * 44100 * _audioSource.clip.length));
		return this;
	}

	void SetSample(int sample)
	{
		if(sample >= _audioSource.clip.samples)
		{
			Debug.LogWarning("SetSample "+ sample+" > "+_audioSource.clip.samples);
			sample = 0;
		}

		sample = Mathf.Max(0, sample);

		try
		{
			//Debug.Log(string.Format("SetTimeSample:{0} clipSample:{1}",sample,_audioSource.clip.samples));
			_audioSource.timeSamples = sample;
		}
		catch(Exception ex)
		{
			Debug.Log("sample "+sample + " ex:"+ ex);
		}
	}

	public float TimeRate()
	{
		return (_audioSource.timeSamples / 44100.0f) / _audioSource.clip.length;
	}

	//adjustTime マイナスだと早めに、プラスだと遅めに。（ただし遅すぎるとdestroyされてるかもです。
	public SoundController SetEndAction(Action endAction,float adjustTime = 0)
	{
		this.Delay(() =>
		{
			endAction();
		},adjustTime+_audioSource.clip.length);
		return this;
	}

	public SoundController SetDestoryAction(Action destroyAction)
	{
		_destroyAction = destroyAction;
		return this;
	}

	public float ClipLength()
	{
		return _audioSource.clip.length;
	}
	
	//再生時間
	public float PlayedTime()
	{
		return _audioSource.time;
	}
}