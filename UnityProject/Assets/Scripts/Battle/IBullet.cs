using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
	float RequireEnergy {get;}
	void Init(Transform shooter, Transform target, float multipliePower);
	void Init(Transform shooter, Vector2 direction, float multipliePower);
}
