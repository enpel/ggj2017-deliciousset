using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyWeapon : MonoBehaviour , IWeapon
{
	public GameObject bulletPrefab;

	public float MaxEnegry = 5;
	public float BaseEnegryChargeRate = 0;

	float _currentEnergy = 0;
	public float CurrentEnegry
	{
		get
		{
			return _currentEnergy;
		}
		private set
		{
			_currentEnergy = Mathf.Min(MaxEnegry, value);
		}
	}
		
	void Start ()
	{
		CurrentEnegry = 0;
	}

	void Update()
	{
		CurrentEnegry += BaseEnegryChargeRate * Time.deltaTime;
	}

	public void Shot()
	{
		var bullet = bulletPrefab.GetComponent<IBullet>();

		if (CurrentEnegry < bullet.RequireEnergy) 
		{
			Debug.Log("エネルギーが足りなくて、タックルが打てませんでした");
			return;
		}

		var parent = this.transform.parent;
		var prefab = GameObject.Instantiate<GameObject>(bulletPrefab, parent);
		prefab.transform.localPosition = this.transform.localPosition;

		var generateBulet = prefab.GetComponent<IBullet>();
		var direction = new Vector2(-1, 0);
		generateBulet.Init(transform, direction, 1);

		CurrentEnegry = 0;
	}
}

