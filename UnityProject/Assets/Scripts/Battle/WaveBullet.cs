using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBullet : BulletBase
{
	[SerializeField] GameObject hitEffectPrefab;
	[SerializeField] float baseFrequency = 1;

	private Rigidbody2D bulletRigidbody;

	public float Amplitude;
	public float Frequency;
	public float Phase;

	private float time;

	public void WaveInit(float amplitude, float frequency, float phase)
	{
		Amplitude = amplitude;
		Frequency = frequency;
		Phase = phase;
	}

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
		time = Phase;

		GenerateTrailEffectPrefab();
	}

	void Update()
	{
		time += Time.deltaTime;

		var scalar = Amplitude * Mathf.Sin(time * Frequency * baseFrequency);
		var force = new Vector2(0, 1) * scalar;
		var pos = new Vector3(0, force.y, 0);

		transform.localPosition += pos;

		Debug.Log(force);
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
