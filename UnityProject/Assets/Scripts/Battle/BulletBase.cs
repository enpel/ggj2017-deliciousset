using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour, IBullet
{
	public GameObject TrailEffectPrefab;
	[SerializeField] float requireEnergy;
	public float RequireEnergy { get { return requireEnergy; } }
	public float BasePower;
	public float Speed;
	public SoundId ShotSE;
	public SoundId HitSE;
	public Color Color;

	// 初期値は関係のない物を設定しておく…（ゴミ実装だ…）
	public TechnologyType PowerTechnology = TechnologyType.EnegryChargeRate;
	public TechnologyType OptionalTechnology = TechnologyType.EnegryChargeRate;
	public GameObject optionalBulletPrefab;

	public float CurrentPower
	{
		get { return BasePower * multipliePower; }
	}

	protected float multipliePower;
	protected Transform shooter;
	protected Transform trans;
	protected Transform target;
	protected Vector2 direction;

	public void Init(Transform shooter, Transform target, float multipliePower)
	{
		this.shooter = shooter;
		this.target = target;
		this.multipliePower = multipliePower;
	}

	public void Init(Transform shooter, Vector2 direction, float multipliePower)
	{
		this.shooter = shooter;
		this.multipliePower = multipliePower;
		this.direction = direction;
	}

	public void GenerateTrailEffectPrefab()
	{
		if (TrailEffectPrefab != null)
		{
			var parent = this.transform;
			var prefab = GameObject.Instantiate<GameObject>(TrailEffectPrefab, parent);
			prefab.transform.localPosition = this.transform.localPosition;
		}
	}

	protected void OnStart()
	{
		trans = transform;
		ShotSE.Play();
	}

	void LateUpdate()
	{
		if (trans == null) return;
		if (this.gameObject == null) return;

		var posX = trans.localPosition.x + trans.parent.localPosition.x;
		// 画面外なら消す、んだけど敵は画面外からタックルしてくるので右端は1300の場所にした。
		if (posX < -800 || posX > 1300)
		{
			Destroy (this.gameObject);
		}
	}

	void OnBecameInvisible()
	{
		Destroy (this.gameObject);
	}
}
