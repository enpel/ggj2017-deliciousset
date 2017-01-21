using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InGameState : SceneState
{
	[SerializeField]
	SceneState nextState;
	[SerializeField]
	GameObject playerPrefab;
	[SerializeField]
	Transform defaultPlayerPosition;
	[SerializeField]
	Transform defaultEnemySpawnPosition;
	IObservable<Unit> battleStartStream;
	Player player;

	public override void Initialize()
	{
		UIManager.Instance.SwitchPhase (UIPhase.INGAME);

		if (player != null)
			Destroy (player.gameObject);

		player = GameObject.Instantiate (playerPrefab, defaultPlayerPosition).GetComponent<Player>();
		player.transform.localPosition = Vector3.zero;
		player.transform.localScale = Vector3.one;

		player.hp.IsDead.Where(isDead => isDead).Subscribe (isDead => {
				GameManager.Instance.State.Value = nextState;
		}).AddTo (this);
	}

	public override void End()
	{
		if (player != null)
			Destroy (player.gameObject);
	}

}

