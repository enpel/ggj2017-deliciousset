using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Player : MonoBehaviour
{
	IWeapon weapon;

	void Awake()
	{
		weapon = GetComponent<IWeapon>();
	}

	void Start()
	{
		MyInput.GetInputStream()
			.Subscribe(x => weapon.Shot());
	}
}
