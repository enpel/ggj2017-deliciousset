using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BattleItemPanel : MonoBehaviour {

	public GameObject itemTemplateObject;
	// Use this for initialization
	void Start () {
		/*
		currentTechnologys.ObserveReplace()
			.Subscribe(x => Debug.Log(x.Key+" “+
				x.NewValue));

		TechnologyManager.Instance
		*/

	}
	void OnEnable()
	{
		DestroyAllChild ();
		RelayoutCurrentTechnorogys ();
		TechnologyManager.Instance.currentTechnologys.ObserveReplace ().Subscribe (
			x => {
				Debug.Log(x.Key+" : "+ x.NewValue);
				RelayoutOneItem(new KeyValuePair<TechnologyType, int>(x.Key,x.NewValue));
			}
		);
	}

	void DestroyAllChild()
	{
		while (transform.childCount > 0)
		{
			Transform child = transform.GetChild(0);
			child.parent = null;
			Destroy(child.gameObject);
		}
	}
	void RelayoutCurrentTechnorogys()
	{
		foreach (var item in TechnologyManager.Instance.currentTechnologys) {
			AddItem (item);
		}

	}
	void RelayoutOneItem(KeyValuePair<TechnologyType, int> kp)
	{
		for (int i = 0; i < transform.childCount; i++) {
			BattleItem bi = transform.GetChild (i).GetComponent<BattleItem> ();
			if (bi == null)
				continue;

			if (bi.technorogytype == kp.Key) {
				bi.value = kp.Value;
				bi.Refresh ();
			}
		}
	}
	void AddItem(KeyValuePair<TechnologyType, int> kp)
	{
		GameObject go = GameObject.Instantiate (itemTemplateObject);
		go.transform.parent = this.transform;
		go.transform.localScale = new Vector3 (1, 1, 1);
		BattleItem bi = go.GetComponent<BattleItem> ();
		bi.technorogytype = kp.Key;
		bi.value = kp.Value;
		bi.Refresh ();

	}

	// Update is called once per frame
	void Update () {
		
	}
}
