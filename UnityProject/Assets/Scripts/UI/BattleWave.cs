using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BattleWave : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		GameManager.Instance.Wave.Subscribe (
			i => 
			{
				text.text = i.ToString().PadLeft(2, '0');
			}
		);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
