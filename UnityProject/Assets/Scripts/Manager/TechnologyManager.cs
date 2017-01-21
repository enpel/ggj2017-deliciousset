using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TechnologyManager : MonoBehaviour
{
	int technologyPoint = 0;
	public void DevelopTechnology()
	{
		var develop = UnityEngine.Random.Range (0, 1);

		if (develop == 0)
			return;
		
		Debug.Log ("成功しました");
		technologyPoint++;
	}
}

public enum TechnologyType
{
	PowerUpBullet,
}

[Serializable]
public class TechnologySetting
{
	public TechnologyType technologyType;
}
