using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タックル それは 自機と同じ当たり判定を持った自爆攻撃 
public class Tackle : BulletBase
{
	[SerializeField] GameObject hitEffectPrefab;

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

		Destroy(shooter.gameObject);
		Destroy(this.gameObject);
	}
}
