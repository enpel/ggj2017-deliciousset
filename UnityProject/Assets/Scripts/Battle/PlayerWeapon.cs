using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour , IWeapon
{
	public float MaxEnegry = 5;
	public float BaseEnegryChargeRate = 2;

	float _currentEnergy = 0;
	public float CurrentEnegry
	{
		get
		{
			return _currentEnergy;
		}
		private set
		{
			_currentEnergy = Mathf.Min(MaxEnegry, value);
		}
	}

	void Start ()
	{
		CurrentEnegry = 0;
	}

	void Update()
	{
		CurrentEnegry += BaseEnegryChargeRate * Time.deltaTime;
	}

	public void Shot()
	{
		Debug.Log("shot" + CurrentEnegry);
		CurrentEnegry = 0;
	}
}
