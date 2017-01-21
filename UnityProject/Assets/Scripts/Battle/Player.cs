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
	}

	void Start()
	{
		MyInput.GetInputStream()
			.Subscribe(x => weapon.Shot());
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.name + "にあたった" );
	}
}
