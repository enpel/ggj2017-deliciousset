using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
	public enum EnemyId
	{
		Walker,
		Army,
		BossGigant,
		Runner,
		ZakoMan,
		BossTotem,
	}
	
	[Serializable]
	public class EnemyPair
	{
		public EnemyId enemyId; 
		public GameObject enemyPrefab;

		public Enemy Instantiate(Transform parent)
		{
			var enemy = GameObject.Instantiate (enemyPrefab, parent).GetComponent<Enemy>();
			enemy.transform.localScale = Vector3.one;
			enemy.transform.localPosition = Vector3.zero;

			return enemy;
		}
	}

	[SerializeField]
	List<EnemyPair> enemies = new List<EnemyPair>();

	public Enemy SpawnEnemy(EnemyId enemyId, Transform parent)
	{
		return enemies.FirstOrDefault (x => x.enemyId == enemyId).Instantiate(parent);
	}

}

