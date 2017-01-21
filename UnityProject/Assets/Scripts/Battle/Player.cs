using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Player : MonoBehaviour
{
	public PlayerWeapon weapon { get; private set; }

	void Awake()
	{
		weapon = GetComponent<PlayerWeapon>();
	}

	void Start()
	{
		MyInput.GetInputStream()
			.Subscribe(x => weapon.Shot());
	}
}
