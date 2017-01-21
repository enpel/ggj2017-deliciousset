using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タックル それは 自機と同じ当たり判定を持った自爆攻撃 
public class Tackle : BulletBase
{
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(string.Format("{0}にあたった", other.name, CurrentPower) );
		Destroy(this.gameObject);
	}
}
