using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class WaveManager : MonoBehaviour {

	[Serializable]
	public class EnemySpawnData
	{
		public EnemyManager.EnemyId enemyId;
		public Vector3 spawnLocalPosition;
	}

	[Serializable]
	public class WaveBGM
	{
		public int waveRank;
		public SoundId soundId;
	}

	[SerializeField]
	Transform spawnPosition;
	[SerializeField]
	List<EnemySpawnData> wave = new List<EnemySpawnData> ();
	[SerializeField]
	List<WaveBGM> waveBGM = new List<WaveBGM> ();
	public ReactiveProperty<int> CurrentEnemyCount = new ReactiveProperty<int>(0);
	private IObservable<bool> clearCurrentWave;

	public IObservable<bool> SpawnNextWave()
	{
		CurrentEnemyCount.Value = wave.Count;
		wave.ForEach (x => {
			var enemy = EnemyManager.Instance.SpawnEnemy(x.enemyId, spawnPosition);
			enemy.transform.localPosition = x.spawnLocalPosition;
			enemy.hp.IsDead.First(isDead => isDead).Subscribe(isDead=> {
				CurrentEnemyCount.Value --;
			}).AddTo(this);
		});

		if (clearCurrentWave == null)
			clearCurrentWave = CurrentEnemyCount.Select (x => x <= 0).Publish().RefCount();

		return clearCurrentWave;
	}

}
