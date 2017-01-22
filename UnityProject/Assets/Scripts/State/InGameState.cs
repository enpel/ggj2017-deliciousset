using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class InGameState : SceneState
{
	[SerializeField]
	SceneState gameOverState;
	[SerializeField]
	GameObject playerPrefab;
	[SerializeField]
	Transform defaultPlayerPosition;
	[SerializeField]
	Transform defaultEnemySpawnPosition;
	[SerializeField]
	WaveManager waveManager;
	[SerializeField]
	List<ThreatData> threatData = new List<ThreatData>();

	Player player;
	public ReactiveProperty<int> ClearWaveCount = new ReactiveProperty<int>(0);
	public ReactiveProperty<int> WaveStep = new ReactiveProperty<int>(0);
	IDisposable waveDisposer;

	void Start()
	{
		WaveStep.Subscribe(wave => {
			GameManager.Instance.Wave.Value = (WaveStep.Value + 1);
		});
	}

	public override void Initialize()
	{
		ClearWaveCount.Value = 0;
		WaveStep.Value = 0;
		GameManager.Instance.Score.Value = 0;
		UIManager.Instance.SwitchPhase (UIPhase.INGAME);

		if (player != null)
			Destroy (player.gameObject);

		player = GameObject.Instantiate (playerPrefab, defaultPlayerPosition).GetComponent<Player>();
		player.transform.localPosition = Vector3.zero;
		player.transform.localScale = Vector3.one;

		StartNextWave ();
	}

	private void StartNextWave()
	{
		// Clear Wave
		waveDisposer = Observable.FromCoroutine (WaveCoroutine, false).Subscribe (next => {
		}, () => {
			if (player.hp.IsDead.Value) {
				GameOver ();
			} else {
				WaveStep.Value++;
				StartNextWave ();
			}
		});
	}

	IEnumerator WaveCoroutine()
	{
		float totalTime = 0;
		ThreatRank currentRank = ThreatRank.Rank1;

		var currentThreatData = threatData.FirstOrDefault (x => x.Data.rank == currentRank);
		// 仮！！！！
		float spawnTime = 0;
		float lastTime = Time.time;
		while (currentRank != ThreatRank.EndIt && !player.hp.IsDead.Value) {
			var deltaTime = Time.time - lastTime;
			lastTime = Time.time;
			totalTime += deltaTime;
			spawnTime += deltaTime;

			if (spawnTime > currentThreatData.Data.spawnRate) {
				waveManager.SpawnCurrentThreatWave ().ForEach(x => x.hp.IsDead.First(isDead => isDead).Subscribe());
				spawnTime -= currentThreatData.Data.spawnRate;
			}

			if (totalTime > currentThreatData.Data.lifeTime) {
				currentRank = currentThreatData.Data.next;
				waveManager.CurrentThreatRank.Value = currentRank;
				totalTime = 0;
				currentThreatData = threatData.FirstOrDefault (x => x.Data.rank == currentRank);

				var id = currentThreatData.Data.bgmIds.ElementAtOrDefault (UnityEngine.Random.Range (0, currentThreatData.Data.bgmIds.Count));

				SoundManager.Instance.Play (id);
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	private void GameOver()
	{
		GameManager.Instance.State.Value = gameOverState;
	}

	public override void End()
	{
		if (player != null)
			Destroy (player.gameObject);

		WaveStep.Value = 0;
		if (waveDisposer != null)
			waveDisposer.Dispose ();
	}

}

