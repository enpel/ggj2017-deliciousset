using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : BulletBase
{
	[SerializeField] GameObject hitEffectPrefab;

	private Rigidbody2D bulletRigidbody;

	void UpdateSize()
	{
		trans.localScale = Vector3.Lerp(trans.localScale, Vector3.one * Mathf.Sqrt(CurrentPower),0.05f);
	}

	void Start()
	{
		OnStart();

		trans.localScale = Vector3.one * Mathf.Sqrt(startScale);

		bulletRigidbody = GetComponent<Rigidbody2D>();
		bulletRigidbody.AddForce(direction * Speed);

		GenerateTrailEffectPrefab();
	}

	void Update()
	{
		UpdateSize();
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
	}
}
