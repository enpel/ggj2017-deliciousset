using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タックル それは 自爆攻撃 
public class Tackle : MonoBehaviour, IBullet
{
	[SerializeField] float requireEnergy;
	public float RequireEnergy { get { return requireEnergy; } }
	public float BasePower;

	public float CurrentPower
	{
		get { return BasePower * multipliePower; }
	}

	private float multipliePower;

	public void Init(Transform target, float multipliePower)
	{
		
	}

	public void Init(Vector2 direction, float multipliePower)
	{
		this.multipliePower = multipliePower;
	}
		
	void OnTriggerEnter(Collider other)
	{
		var damage = CurrentPower * multipliePower;
		Debug.Log(string.Format("{0}にあたった", other.name, damage) );
		Destroy(this.gameObject);
	}
}
