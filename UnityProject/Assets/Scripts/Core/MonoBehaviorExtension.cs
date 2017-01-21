using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class MonoBehaviourExtension
{
	//使い方
	//this.Delay(()=>{Debug.Log("test")},0.5f);
	public static void Delay(this MonoBehaviour mono, Action action, float delay, bool realtime = false)
	{
		mono.StartCoroutine(mono._Delay(action, delay, realtime));
	}

	private static IEnumerator _Delay(this MonoBehaviour mono, Action action, float delay, bool realtime)
	{
		if (delay <= 0.0f)
		{
			action();
			yield break;
		}
		if(realtime)
		{
			yield return new WaitForSecondsRealtime(delay);
		}
		else
		{
			yield return new WaitForSeconds(delay);
		}
		action();
	}

	//使い方
	//this.Wait(()=>{return x;},()=>{Debug.Log("test");});
	public static void Wait(this MonoBehaviour mono, Func<bool> func, Action action)
	{
		mono.StartCoroutine(mono._Wait(func, action));
	}

	private static IEnumerator _Wait(this MonoBehaviour mono, Func<bool> func, Action action)
	{
		yield return new WaitWhile(func);
		action();
	}

#if UNITY_EDITOR
	public static void SetValue<T>(this MonoBehaviour target, string propertyName, T value)
	{
		var fieldInfo = target.GetType().GetField (propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		fieldInfo.SetValue(target,value);
	}
#endif
}
