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

	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		
	}
	public void Refresh()
	{
		havecountText.text = "x " + value.ToString ();
	}
}
