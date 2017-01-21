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
	[SerializeField]
	WaveManager waveManager;
	IObservable<Unit> battleStartStream;
	Player player;
	public ReactiveProperty<int> ClearWaveCount = new ReactiveProperty<int>(0);

	public override void Initialize()
	{
		UIManager.Instance.SwitchPhase (UIPhase.INGAME);

		if (player != null)
			Destroy (player.gameObject);

		player = GameObject.Instantiate (playerPrefab, defaultPlayerPosition).GetComponent<Player>();
		player.transform.localPosition = Vector3.zero;
		player.transform.localScale = Vector3.one;

		// GameOver
		player.hp.IsDead.Where(isDead => isDead).Subscribe (isDead => {
			GameManager.Instance.State.Value = nextState;
		}).AddTo (this);

		// Clear Wave
		waveManager.SpawnNextWave ().First(x => x).Subscribe(x => {
			GameManager.Instance.State.Value = nextState;
		});

	}

	public override void End()
	{
		if (player != null)
			Destroy (player.gameObject);
	}

}

