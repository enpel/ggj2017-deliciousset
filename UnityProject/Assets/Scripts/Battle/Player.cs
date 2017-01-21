using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Player : MonoBehaviour
{
	public PlayerWeapon weapon { get; private set; }
	public Hp hp {get; private set;}

	void Awake()
	{
		weapon = GetComponent<PlayerWeapon>();
		hp = GetComponent<Hp>();

		hp.IsDead.Where(dead => dead)
				.Subscribe(dead =>
				{
					Debug.Log("Playerがやられたので、多分ゲームオーバーだと思う。");
				});
	}

	void Start()
	{
		MyInput.GetInputStream()
			.Subscribe(x => weapon.Shot());
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var bullet = other.GetComponent<BulletBase>();
		var damage = bullet.CurrentPower;
		hp.OnDamage(damage);
		Debug.Log(string.Format("{0}は{1}に{2}ポイントのダメージ{3}", other.name, gameObject.name, damage, hp.ToDisplayString()));
	}
}
