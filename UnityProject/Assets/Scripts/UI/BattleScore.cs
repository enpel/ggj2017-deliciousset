using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BattleScore : MonoBehaviour {

	Player player;
	Text text;

	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update () {
	}
}
