using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BattleHP : MonoBehaviour
{
	RectTransform rt;

	public void SubscribeHPStream(Player player)
	{
		rt = GetComponent<RectTransform> ();
		player.hp.CurrentHP.Subscribe(x => {
			var s = player.hp.Rate;
			rt.localScale = new Vector3 (s, 1, 1);
		}).AddTo(this);
	}
}
