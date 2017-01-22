using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChargeUI : MonoBehaviour {

	PlayerWeapon playerweapon;
	RectTransform rt;

	Image image;

	// Use this for initialization
	void Start () {
		playerweapon = transform.parent.GetComponent<PlayerWeapon> ();
		rt = GetComponent<RectTransform> ();
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerweapon) {
			float p = playerweapon.CurrentEnegry / playerweapon.MaxEnegry;
			rt.localScale = new Vector3 (p, p, 1);

			var bullet = playerweapon.CurrentBullet;
			image.color = Color.Lerp(image.color, bullet.Bullet.Color, 0.1f);
		}
	}
}
