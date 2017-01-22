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
		public TechnologyType OptionalTechnology {get; private set;}

		public BulletData(GameObject prefab, IBullet bullet, TechnologyType powerTechnology, TechnologyType optionalTechnology)
		{
			Prefab = prefab;
			Bullet = bullet;
			PowerTechnology = powerTechnology;
			OptionalTechnology = optionalTechnology;
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
				return new BulletData(prefab, bullet, bullet.PowerTechnology, bullet.OptionalTechnology);
			}).ToList();
	}

	void Update()
	{
		CurrentEnegry += BaseEnegryChargeRate * ChargeRateForTechnology * Time.deltaTime;
	}

	public void Shot()
	{
		var selectedBullet = bulletDatas.FindLast(bulletData => bulletData.Bullet.RequireEnergy <= CurrentEnegry);

		var technologyPower = 1 + TechnologyManager.Instance.currentTechnologys[selectedBullet.PowerTechnology] * 0.1f;
		var optionalBulletNum = TechnologyManager.Instance.currentTechnologys[selectedBullet.OptionalTechnology];

		// メインウェポン
		var power = CurrentEnegry / MaxEnegry;
		var mainBullet = InstantiateBullet(selectedBullet.Prefab);
		var optionalBulletPrefab = mainBullet.optionalBulletPrefab;
		var multipulPower = power * technologyPower;
		{
			var direction = new Vector2(1, 0);
			mainBullet.Init(transform, new Vector2(1, 0), multipulPower);
		}

		if (optionalBulletPrefab != null)
		{
			// オプショナルの弾を生成
			for (var i = 0; i < optionalBulletNum; i++)
			{
				var bullet = InstantiateBullet(optionalBulletPrefab).GetComponent<WaveBullet>();
				var direction = new Vector2(1, 0);
				bullet.Speed = mainBullet.Speed;
				bullet.Init(transform, direction, multipulPower * 0.1f);
				bullet.WaveInit(1, 1, i);
			}
		}

		CurrentEnegry = 0;
	}

	BulletBase InstantiateBullet(GameObject bulletPrefab)
	{
		var parent = this.transform.parent;
		var prefab = GameObject.Instantiate<GameObject>(bulletPrefab, parent);
		prefab.transform.localPosition = this.transform.localPosition;

		var bullet = prefab.GetComponent<BulletBase>();
		return bullet;
	}
}
