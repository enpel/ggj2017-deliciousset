using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InGameState : SceneState
{
	[SerializeField]
	SceneState nextState;
	IObservable<Unit> battleStartStream;

	public override void Initialize()
	{
		UIManager.Instance.SwitchPhase (UIPhase.INGAME);
	}

}

