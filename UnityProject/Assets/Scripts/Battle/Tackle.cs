using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タックル それは 自機と同じ当たり判定を持った自爆攻撃 
public class Tackle : BulletBase
{
	[SerializeField] GameObject hitEffectPrefab;

	void Start()
	{
		OnStart();
	}

	void Update()
	{
		if (shooter == null)
		{
			Destroy(this.gameObject);
			return;
		}
		if (trans == null) return;

		// 敵と同じ位置に移動する
		trans.localPosition = shooter.localPosition;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (hitEffectPrefab != null)
		{
			var go = GameObject.Instantiate(hitEffectPrefab);
			var effectTrans = go.transform;
			effectTrans.SetParent(transform.parent);	// 弾は消えるので親オブジェクトに関連付ける
			effectTrans.localPosition = transform.localPosition;
			effectTrans.localScale = Vector3.one;
		}

		HitSE.Play();

		Destroy(this.gameObject);
		Hp hp = shooter.gameObject.GetComponent<Hp> ();
		if (hp != null)
			hp.OnDamage(hp.MaxHP);
		else 
			Destroy(shooter.gameObject);
	}
}
