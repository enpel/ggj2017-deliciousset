using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class WaveManager : MonoBehaviour {

	[SerializeField]
	Transform spawnPosition;
	[SerializeField]
	GameObject enemyPrefab;

	void Start()
	{
		Observable.Interval (TimeSpan.FromSeconds (2.0f))
			.Subscribe (x => {
				var clone = GameObject.Instantiate(enemyPrefab, spawnPosition.transform);
				clone.transform.localScale = Vector3.one;
				clone.transform.localPosition = Vector3.zero;
		}).AddTo (this);
		
	}

	void Update()
	{
		
	}
}
