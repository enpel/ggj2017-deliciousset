using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase  : MonoBehaviour, IBullet
{
	[SerializeField] float requireEnergy;
	public float RequireEnergy { get { return requireEnergy; } }
	public float BasePower;
	public float Speed;
	public SoundId ShotSE;

	public float CurrentPower
	{
		get { return BasePower * multipliePower; }
	}

	protected float multipliePower;
	protected Transform trans;
	protected Transform target;
	protected Vector2 direction;

	public void Init(Transform target, float multipliePower)
	{
		this.target = target;
		this.multipliePower = multipliePower;
	}

	public void Init(Vector2 direction, float multipliePower)
	{
		this.multipliePower = multipliePower;
		this.direction = direction;
	}

	protected void OnStart()
	{
		trans = transform;
		ShotSE.Play();
	}

	void OnBecameInvisible()
	{
		Destroy (this.gameObject);
	}
}
