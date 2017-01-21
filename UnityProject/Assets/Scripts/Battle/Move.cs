using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	public Transform target;
	public float Speed;

	void Update()
	{
		var direction = new Vector2(-1, 0);
		transform.Translate(direction * Speed * Time.deltaTime);
	}
}
