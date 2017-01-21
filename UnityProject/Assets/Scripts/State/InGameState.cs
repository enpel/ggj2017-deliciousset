﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class InGameState : SceneState
{
	[Serializable]
	public class WaveBGM
	{
		public int waveRank;
		public SoundId soundId;
	}
	[SerializeField]
	float threatPerSecond = 45;
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
	List<WaveBGM> waveBGM = new List<WaveBGM> ();

	Player player;
	public ReactiveProperty<int> ClearWaveCount = new ReactiveProperty<int>(0);
	public ReactiveProperty<int> WaveStep = new ReactiveProperty<int>(0);
	IDisposable waveDisposer;

	public override void Initialize()
	{
		ClearWaveCount.Value = 0;
		WaveStep.Value = 0;
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
		float time = 0;
		ThreatRank currentRank = ThreatRank.Rank1;
		var step = this.WaveStep.Value;
		var bgm = waveBGM.Where (x => x.waveRank == step % 2).ToArray ();
		var selectedBGM = bgm.ElementAt (UnityEngine.Random.Range (0, bgm.Count ()));
		SoundManager.Instance.StopBgm ();
		SoundManager.Instance.Play (selectedBGM.soundId);

		// 仮！！！！
		while (currentRank != ThreatRank.EndIt && !player.hp.IsDead.Value) {
			time += Time.deltaTime;
			Debug.Log ("time: " +time);
			if (time > 1) {
				waveManager.SpawnCurrentThreatWave ();
				time -= 1;
			}
			if (time > 30)
				currentRank = ThreatRank.EndIt;
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

