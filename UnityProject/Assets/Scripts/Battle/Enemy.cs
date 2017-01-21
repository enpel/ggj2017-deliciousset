using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.name + "にあたった" );
	}
}
