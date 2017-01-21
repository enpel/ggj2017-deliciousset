using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SoundUtil
{
	public static AudioClip LoadClip(string path)
	{
		return Resources.Load(path) as AudioClip;
	}
		
	public static IEnumerator LoadStreamingClipAsync(string fileName, Action<AudioClip> endAction)
	{
		var path = "file://" + Path.Combine( Application.streamingAssetsPath , fileName+".wav");

		using(WWW www = new WWW(path))
		{
			yield return www;
			if(www.error != null)
			{
				Debug.Log(www.error);
				endAction(null);
			}
			else
			{
				endAction(www.audioClip);
			}
		}
	}

	public static IEnumerator LoadClipAsync(string path, Action<AudioClip> endAction)
	{
		var resReq = Resources.LoadAsync<AudioClip>(path);
		yield return new WaitWhile(()=>{return !resReq.isDone;});
		endAction(resReq.asset as AudioClip);
	}

	public static void PlayClipAtPoint (AudioClip clip, Vector3 position,float volume = 1.0f)
	{
		GameObject gameObject = new GameObject ("One shot audio"+clip.name);
		gameObject.transform.position = position;
		AudioSource audioSource = (AudioSource)gameObject.AddComponent (typeof(AudioSource));
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.Play ();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		UnityEngine.Object.Destroy (gameObject, clip.length * Time.timeScale);
	}


	public static GameObject PlayOneShot(AudioClip ac,GameObject parent = null)
	{
		return PlayOneShot(ac,1.0f,1.0f,parent);
	}

	public static GameObject PlayOneShot(AudioClip ac,float volume,float pitch,GameObject parent)
	{
		try
		{
			return PlayClipAtPoint(ac,Vector3.zero,volume,pitch,parent);
		}
		catch(Exception ex)
		{
			//Debug.Log("PlayOneShot : " + data.fileName + " is not found");
			Debug.Log(ex.ToString());
			return null;
		}
	}

	public static GameObject CreateSoundObject(AudioClip clip)
	{
		GameObject V_0 = new GameObject(clip.name);
		UnityEngine.Object.DontDestroyOnLoad(V_0);
		AudioSource V_1 = V_0.AddComponent<AudioSource>();
		V_1.clip = clip;
		return V_0;
	}

	//for Assembly Browser
	public static GameObject PlayClipAtPoint(AudioClip clip, Vector3 position, float volume,float pitch,GameObject parent)
	{
		try
		{
			GameObject V_0 = new GameObject(clip.name);
			V_0.transform.position = position;
			if (parent != null)
			{
				V_0.transform.SetParent(parent.transform);
			}
			//UnityEngine.Object.DontDestroyOnLoad(V_0);
			AudioSource V_1 = V_0.AddComponent<AudioSource>();
			V_1.clip = clip;
			V_1.volume = volume;
			V_1.pitch = pitch;
			V_1.Play();
			UnityEngine.Object.Destroy(V_0, clip.length);
			return V_0;
		}
		catch(Exception ex)
		{
			//Debug.Log("PlayOneShot : " + data.fileName + " is not found");
			Debug.Log(ex.ToString());
			return null;
		}
	}

	public static float GetRandomPitch(int num,float range)
	{
		return UnityEngine.Random.Range(GetPitch(num-range),GetPitch(num+range));
	}

	private static Dictionary<int,float> pitchTable = new Dictionary<int,float>();
	public static float GetPitch(int num)
	{
		if(!pitchTable.ContainsKey(num))
		{
			float pitch = 1.0f;
			if(num > 0)
			{
				for(int i =0; i < num; i++)
				{
					pitch *= 1.059463f;
				}
			}
			else if(num < 0)
			{
				for(int i =0; i > num; i--)
				{
					pitch /= 1.059463f;
				}
			}
			pitchTable[num] = pitch;
		}

		return pitchTable[num];
		/*
	float pitch = 1.0f;
	if(num > 0)
	{
		for(int i =0; i < num; i++)
		{
			pitch *= 1.059463f;
		}
	}
	else if(num < 0)
	{
		for(int i =0; i > num; i--)
		{
			pitch /= 1.059463f;
		}
	}

	return pitch;
	*/
	}

	//大体の精度
	private static float GetPitch(float num)
	{
		int baseNum = (int)num;
		float rate = Mathf.Abs(num - baseNum);
		return GetPitch(baseNum)*(1-rate)+GetPitch(baseNum + baseNum < 0 ? -1 : 1)*rate;
	}

	private static bool[] pitchWhite = new bool[]
	{
		true,false,true,false,true,true,false,true,false,true,false,true
	};

	public static float GetPitchWhite(int num)
	{
		int newNum = 0;
		if(num > 0)
		{
			for(int i =0; i < num; i++)
			{
				if(!pitchWhite[(newNum+1)%12])
				{
					newNum++;
				}
				newNum++;
			}
		}
		else if(num < 0)
		{
			for(int i =0; i > num; i--)
			{
				if(!pitchWhite[(newNum+11+12*8)%12])
				{
					newNum--;
				}
				newNum--;
			}
		}
		//Debug.Log("sound white num=>"+num+"newNum=>"+newNum);
		return GetPitch(newNum);
	}
	public static float GetPitchBlack(int num)
	{
		int newNum = 0;
		if(num > 0)
		{
			for(int i =0; i < num; i++)
			{
				if(pitchWhite[(newNum+1)%12])
				{
					newNum++;
				}
				newNum++;
			}
		}
		else if(num < 0)
		{
			for(int i =0; i > num; i--)
			{
				if(pitchWhite[(newNum+11+12*8)%12])
				{
					newNum--;
				}
				newNum--;
			}
		}
		//Debug.Log("sound num=>"+num+"newNum=>"+newNum);
		return GetPitch(newNum);
	}

	public static float TimeToMeasure(float time,float bpm)
	{
		return time / (2*120/bpm);
	}

	public static float MeasureToTime(float measure,float bpm)
	{
		return measure * (2*120/bpm);
	}
}
