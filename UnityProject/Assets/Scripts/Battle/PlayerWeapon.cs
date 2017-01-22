using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerWeapon : MonoBehaviour , IWeapon
{
	public List<GameObject> bulletPrefabs;
	public List<BulletData> bulletDatas;

	public class BulletData
	{
		public GameObject Prefab {get; private set;}
		public IBullet Bullet {get; private set;}
		public TechnologyType PowerTechnology {get; private set;}

		public BulletData(GameObject prefab, IBullet bullet, TechnologyType powerTechnology)
		{
			Prefab = prefab;
			Bullet = bullet;
			PowerTechnology = powerTechnology;
		}
	}

	public float MaxEnegry = 5;
	public float BaseEnegryChargeRate = 2;

	public float ChargeRateForTechnology
	{
		get
		{
			return 1 + TechnologyManager.Instance.currentTechnologys[TechnologyType.EnegryChargeRate] * 0.1f;
		}
	}

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
		bulletDatas = bulletPrefabs.Select(prefab =>
			{
				var bullet = prefab.GetComponent<BulletBase>();
				return new BulletData(prefab, bullet, bullet.PowerTechnology);
			}).ToList();
	}

	void Update()
	{
		CurrentEnegry += BaseEnegryChargeRate * ChargeRateForTechnology * Time.deltaTime;
	}

	public void Shot()
	{
		var selectedBullet = bulletDatas.FindLast(bulletData => bulletData.Bullet.RequireEnergy <= CurrentEnegry);

		var parent = this.transform.parent;
		var prefab = GameObject.Instantiate<GameObject>(selectedBullet.Prefab, parent);
		prefab.transform.localPosition = this.transform.localPosition;

		var bullet = prefab.GetComponent<BulletBase>();

		var technologyPower = 1 + TechnologyManager.Instance.currentTechnologys[bullet.PowerTechnology] * 0.1f;

		var power = CurrentEnegry / MaxEnegry;
		var multipulPower = power * technologyPower;
		var direction = new Vector2(1, 0);
		bullet.Init(transform, direction, multipulPower);

		CurrentEnegry = 0;
	}
}
