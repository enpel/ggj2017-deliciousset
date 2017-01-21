using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class WaveManager : MonoBehaviour {

	[Serializable]
	public class EnemySpawnData
	{
		public EnemyManager.EnemyId enemyId;
		public Vector3 spawnLocalPosition;
	}

	[SerializeField]
	Transform spawnPosition;
	[SerializeField]
	List<EnemySpawnData> wave = new List<EnemySpawnData> ();

	public List<Enemy> SpawnNextWave(int step)
	{
		return wave.Select (x => {
			var enemy = EnemyManager.Instance.SpawnEnemy(x.enemyId, spawnPosition);
			enemy.transform.localPosition = x.spawnLocalPosition;
			return enemy;
		}).ToList();
	}
}
