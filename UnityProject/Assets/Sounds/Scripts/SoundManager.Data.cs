using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using SoundType = SoundManager.SoundType;

//いろんな場所で使われるので、ネームスペース外に置いて使いやすいようにする。
public enum SoundId
{
	BgmStart,
	ggj01_01,
	ggj01_02,
	ggj02_01,

	BgmEnd,
	SeStart,

	ggj_se01,
	ggj_se02,
	ggj_se03,
	ggj_se04,
	ggj_se05,
	ggj_se06,
	ggj_se07,
	ggj_se08,
	ggj_se09,
	ggj_se10,
	ggj_se11,
	ggj_se12,
	ggj_se13,
	ggj_se14,
	ggj_se15,
	ggj_se16,
	ggj_se17,
	ggj_se18,
	ggj_se19,
	ggj_se20,
	ggj_se21,
	/// <summary>
	/// 弾１として使う
	/// </summary>
	ggj_se22,
	ggj_se23,
	ggj_se24,

	SeEnd,
	None,
	Max,
};

public partial class SoundManager
{
	public enum SoundType
	{
		Bgm,
		Se,
		Max,
	};
}

public static class SoundExt
{
	public static string ToDispString(this SoundId value)
	{
		switch (value)
		{
		default:
			return value.ToString();
		}
	}

	public static SoundId NextRandom(this SoundId value,int num)
	{
		var selectNum = Random.Range(0, num);
		return value + selectNum;
	}

	public static string ToResourceName(this SoundId value)
	{
		//var num = 0;
		switch (value)
		{
			default:
				return value.ToString();
		}
		//var selectNum = Random.Range(0, num)+1;
		//return string.Format("{0}_{1}", value, selectNum);
	}

	public static SoundType ToSoundType(this SoundId value)
	{
		if (value > SoundId.BgmStart && value < SoundId.BgmEnd)
		{
			return SoundType.Bgm;
		}
		else if (value > SoundId.SeStart && value < SoundId.SeEnd)
		{
			return SoundType.Se;
		}
		else
		{
			return SoundType.Max;
		}
	}

	public static bool IsLoop(this SoundId value)
	{
		if (value.ToSoundType() == SoundType.Bgm)
		{
			return true;
		}

		return false;
	}

	public static SoundId ToSoundId(this string value)
	{
		try
		{
			var id = (SoundId)Enum.Parse(typeof(SoundId), value);
			return id;
		}
		catch(ArgumentException ex)
		{
			Debug.Log(string.Format("{0}をSoundIdに変換できませんでした。\n{1}",value,ex));
			return SoundId.None;
		}
	}

	public static bool IsAutoClearCache(this SoundId value)
	{
		var soundType = value.ToSoundType();
		return soundType != SoundType.Se;
	}

	public static float ToDefaultVolume(this SoundId value)
	{
		if(value.ToSoundType() == SoundType.Bgm)
		{
			return 0.8f;
		}

		return 1.0f;
	}

	public static float ToBpm(this SoundId value)
	{
		switch(value)
		{
			case SoundId.ggj01_01: return 145;
			default:
				return -1;
		}
	}

	public static float ToOneMeasureTime(this SoundId value)
	{
		var bpm = value.ToBpm();
		if(bpm == -1) return 1;
		return SoundUtil.MeasureToTime(1,bpm);
	}

	public static SoundController Play(this SoundId value)
	{
		return SoundManager.Instance.Play(value);
	}

	public static void Play(this SoundId value, float delayTime, Action<SoundController> playedAction = null)
	{
		SoundManager.Instance.Play(value,delayTime,playedAction);
	}

	public static void PlayWithRandomPitch(this SoundId value, float range = 0.01f)
	{
		SoundManager.Instance.Play(value,0,(sc)=>{
			var pitch = 1 + Random.Range(-range,range);
			sc.ChangePitch(pitch);
		});
	}

	public static void PlayWithPitch(this SoundId value, float pitch)
	{
		SoundManager.Instance.Play(value,0,(sc)=>{
			sc.ChangePitch(pitch);
		});
	}

	public static void PlayWithPitchScale(this SoundId value, int scale)
	{
		SoundManager.Instance.Play(value,0,(sc)=>{
			sc.ChangePitchScale(scale);
		});
	}

	public static void PlayWithPitchWhiteScale(this SoundId value, int scale)
	{
		SoundManager.Instance.Play(value,0,(sc)=>{
			sc.ChangePitch(SoundUtil.GetPitchWhite(scale));
		});
	}

	public static SoundController Play(this SoundId value,bool truth, SoundId falseId)
	{
		var soundId = truth ? value : falseId;
		return soundId.Play();
	}

	public static float? SinglePlayStartTime(this SoundId to)
	{
		var startMeasure = 0.0f;

		if (startMeasure == 0.0f) return null;

		var toBpm = to.ToBpm();
		return SoundUtil.MeasureToTime(startMeasure,toBpm);
	}

	public static float? CrossFadeSeekTime(this SoundId to,SoundController fromCt)
	{
		/*
		var from = fromCt._soundId;
		var fromBpm = from.ToBpm();
		var toBpm = to.ToBpm();
		*/
		return null;
	}
}