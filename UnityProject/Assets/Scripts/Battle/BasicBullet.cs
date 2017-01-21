using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour, IBullet
{
	[SerializeField] float requireEnergy;
	public float RequireEnergy { get { return requireEnergy; } }
	public float BasePower;
	public float Speed;

	public float CurrentPower
	{
		get { return BasePower * multipliePower; }
	}

	private float multipliePower;
	private Transform trans;
	private Transform target;
	private Vector2 direction;

	private Rigidbody2D bulletRigidbody;
	
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

	void SetSize()
	{
		trans.localScale = Vector3.one * Mathf.Sqrt(CurrentPower);
	}

	void Start()
	{
		trans = transform;
		SetSize();
		bulletRigidbody = GetComponent<Rigidbody2D>();
		bulletRigidbody.AddForce(direction * Speed);
	}

	void OnTriggerEnter(Collider other)
	{
		var damage = CurrentPower * multipliePower;
		Debug.Log(string.Format("{0}にあたった", other.name, damage) );
		Destroy(this.gameObject);
	}
}
