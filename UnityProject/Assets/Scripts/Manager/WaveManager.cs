using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class WaveManager : MonoBehaviour {


	[SerializeField]
	Transform spawnPosition;
	[SerializeField]
	List<EnemyWaveData> waves = new List<EnemyWaveData> ();
	EnemyWave currentEnemyWave;
	public ReactiveProperty<ThreatRank> CurrentThreatRank = new ReactiveProperty<ThreatRank> (ThreatRank.Rank1);

	void Start()
	{
		CurrentThreatRank.Subscribe (rank => {
			currentEnemyWave = waves.FirstOrDefault(x => x.Data.rank == rank).Data;
		}).AddTo (this);
	}

	public List<Enemy> SpawnCurrentThreatWave()
	{
		var region = RandomRegionByCurrentThreat ();
		List<Enemy> enemies = new List<Enemy> ();
		region.spawnData.ForEach (spawn => {
			enemies.Add(SpawnEnemy(spawn));
		});

		return enemies;
	}

	private EnemyRegion RandomRegionByCurrentThreat()
	{
		var region = currentEnemyWave.regions.ElementAtOrDefault (UnityEngine.Random.Range(0, currentEnemyWave.regions.Count));
		return region.Data;
	}

	private Enemy SpawnEnemy(EnemySpawnData spawnData)
	{
		var enemy = EnemyManager.Instance.SpawnEnemy(spawnData.enemyId, spawnPosition);
		enemy.transform.localPosition = spawnData.spawnLocalPosition * 100;
		return enemy;
	}
}

public enum ThreatRank
{
	Rank1,
	Rank2,
	Rank3,
	EndIt,
	Max
}

[Serializable]
public class EnemyWave
{
	public ThreatRank rank;
	public List<EnemyRegionData> regions = new List<EnemyRegionData> ();
}

[Serializable]
public class EnemyRegion
{
	public List<EnemySpawnData> spawnData = new List<EnemySpawnData> ();
}

[Serializable]
public class EnemySpawnData
{
	public EnemyManager.EnemyId enemyId;
	public Vector3 spawnLocalPosition;
}


