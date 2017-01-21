using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : BulletBase
{
	private Rigidbody2D bulletRigidbody;

	void SetSize()
	{
		trans.localScale = Vector3.one * Mathf.Sqrt(CurrentPower);
	}

	void Start()
	{
		OnStart();

		SetSize();
		bulletRigidbody = GetComponent<Rigidbody2D>();
		bulletRigidbody.AddForce(direction * Speed);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(string.Format("{0}にあたった", other.name, CurrentPower) );
		Destroy(this.gameObject);
	}
}
