using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Enemy : MonoBehaviour
{
	public IWeapon weapon { get; private set; }
	public Move move{get; private set;}
	public Hp hp {get; private set;}

	[SerializeField]
	private float attackArea; // 攻撃する範囲

	private Transform target;
	private Transform trans;

	void Awake()
	{
		weapon = GetComponent<IWeapon>();
		move = GetComponent<Move>();
		hp = GetComponent<Hp>();

		hp.IsDead.Where(dead => dead)
			.Subscribe(dead =>
			{
				Destroy(gameObject);
			});
	}

	void Start()
	{
		trans = transform;
		target = GameObject.FindWithTag("Player").transform;
	}

	void Update()
	{
		if (target != null)
		{
			var sqrMagnitude = (trans.position - target.position).sqrMagnitude;
			if (attackArea * attackArea > sqrMagnitude)
			{
				weapon.Shot();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var bullet = other.GetComponent<BulletBase>();
		var damage = bullet.CurrentPower;
		hp.OnDamage(damage);
		Debug.Log(string.Format("{0}は{1}に{2}ポイントのダメージ{3}", other.name, gameObject.name, damage, hp.ToDisplayString()));
	}
}
