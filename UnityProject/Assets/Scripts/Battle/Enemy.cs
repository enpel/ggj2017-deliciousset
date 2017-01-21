using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Enemy : MonoBehaviour
{
	public IWeapon weapon { get; private set; }
	public Move move{get; private set;}
	public Hp hp {get; private set;}

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

	void OnTriggerEnter2D(Collider2D other)
	{
		var bullet = other.GetComponent<BulletBase>();
		var damage = bullet.CurrentPower;
		hp.OnDamage(damage);
		Debug.Log(string.Format("{0}は{1}に{2}ポイントのダメージ{3}", other.name, gameObject.name, damage, hp.ToDisplayString()));
	}
}
