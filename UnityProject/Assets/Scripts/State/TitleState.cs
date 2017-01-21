using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TitleState : SceneState
{
	[SerializeField]
	SceneState nextState;
	IObservable<Unit> battleStartStream;
	
	public override void Initialize()
	{
		UIManager.Instance.SwitchPhase (UIPhase.TITLE);
		if (battleStartStream == null) {
			battleStartStream = UIManager.Instance.GetOnClickStartStream ()
				.Publish ()
				.RefCount ();
			battleStartStream.Subscribe (x => {
				GameManager.Instance.State.Value = nextState;
			}).AddTo (this);
		}

		SoundManager.Instance.Play (SoundId.ggj01_01);
	}

	public override void End ()
	{
	}

}
