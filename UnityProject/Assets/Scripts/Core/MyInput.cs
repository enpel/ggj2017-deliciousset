using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MyInput
{
	static IObservable<Unit> onInput;

	public static IObservable<Unit> GetInputStream()
	{
		if (onInput == null)
		{
			onInput = CreateInputStream();
		}

		return onInput;
	}

	static IObservable<Unit> CreateInputStream()
	{
		var updateHolder = new GameObject();
		var stream = updateHolder.UpdateAsObservable()
						.Where(_ => 
						{
							if (Application.platform == RuntimePlatform.Android
							|| Application.platform == RuntimePlatform.IPhonePlayer)
							{
								return Input.GetMouseButtonDown(0);
							}
							else
							{
								return Input.GetKeyDown(KeyCode.Space);
							}
						})
						.Publish()
						.RefCount();
		return stream;
	}
}
