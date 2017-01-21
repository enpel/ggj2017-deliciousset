using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyWeapon : MonoBehaviour , IWeapon
{
	public GameObject bulletPrefab;

	public void Shot()
	{
		var parent = this.transform.parent;
		var prefab = GameObject.Instantiate<GameObject>(bulletPrefab, parent);
		prefab.transform.localPosition = this.transform.localPosition;

		var bullet = prefab.GetComponent<IBullet>();
		var direction = new Vector2(-1, 0);
		bullet.Init(direction, 1);
	}
}

