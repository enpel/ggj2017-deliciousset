using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BattleHP : MonoBehaviour {

	Player player;
	RectTransform rt;
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
		player = transform.parent.parent.GetComponent<Player> ();
		player.hp.CurrentHP.Subscribe (
			f => {
				Debug.Log ("CurrentHP:" + f.ToString ());
				float p = f / player.hp.MaxHP;
				rt.localScale = new Vector3 (p, 1, 1);
			}
		).AddTo(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
