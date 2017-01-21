using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
	float RequireEnergy {get;}
	void Init(Transform target, float multipliePower);
	void Init(Vector2 direction, float multipliePower);
}
