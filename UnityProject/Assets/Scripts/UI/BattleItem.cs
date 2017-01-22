using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItem : MonoBehaviour {

	public Text havecountText;
	public Image itemIcon;

	// Use this for initialization
	//public KeyValuePair<TechnologyType,int> kp {get; set;}
	public TechnologyType technorogytype { get; set; }
	public int value{ get; set; }

	public Sprite[] sprites;

	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		
	}
	public void Refresh()
	{
		switch (technorogytype) {
		case TechnologyType.EnegryChargeRate:
			itemIcon.overrideSprite = sprites [0];
			break;
		case TechnologyType.PowerUpBasicBullet1:
			itemIcon.overrideSprite = sprites [1];
			break;
		case TechnologyType.OptionalBasicBullet1:
			itemIcon.overrideSprite = sprites [2];
			break;
		case TechnologyType.PowerUpBasicBullet2:
			itemIcon.overrideSprite = sprites [3];
			break;
		case TechnologyType.OptionalBasicBullet2:
			itemIcon.overrideSprite = sprites [4];
			break;
		case TechnologyType.PowerUpBasicBullet3:
			itemIcon.overrideSprite = sprites [5];
			break;
		case TechnologyType.OptionalBasicBullet3:
			itemIcon.overrideSprite = sprites [6];
			break;
		}
		havecountText.text = value.ToString ();
	}
}
