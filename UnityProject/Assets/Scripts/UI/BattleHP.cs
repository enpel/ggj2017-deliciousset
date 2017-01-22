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

	}
	
	// Update is called once per frame
	void OnEnable()
	{
		rt = GetComponent<RectTransform> ();
		Invoke ("SubscribeHPStream", 1f);
	}
	void SubscribeHPStream()
	{
		Debug.Log ("SubscribeHPStream");

		foreach(GameObject go in Object.FindObjectsOfType(typeof(GameObject)))
		{
			Player pl = go.GetComponent<Player> ();
			if (pl == null)
				continue;
			
			//player = transform.parent.parent.GetComponent<Player> ();
			pl.hp.CurrentHP.Subscribe (
				f => {
					Debug.Log ("CurrentHP:" + f.ToString ());
					float p = f / pl.hp.MaxHP;
					rt.localScale = new Vector3 (p, 1, 1);
				}
			).AddTo(this);
			break;
		}

	}
	void Update () {
		
	}
}
