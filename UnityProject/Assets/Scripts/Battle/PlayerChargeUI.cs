using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeUI : MonoBehaviour {

	PlayerWeapon playerweapon;
	RectTransform rt;
	// Use this for initialization
	void Start () {
		playerweapon = transform.parent.GetComponent<PlayerWeapon> ();
		rt = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerweapon) {
			float p = playerweapon.CurrentEnegry / playerweapon.MaxEnegry;
			rt.localScale = new Vector3 (p, p, 1);
		}
	}
}
