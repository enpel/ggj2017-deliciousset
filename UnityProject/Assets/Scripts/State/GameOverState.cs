using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameOverState : SceneState
{
	[SerializeField]
	SceneState nextState;

	public override void Initialize ()
	{
		base.Initialize ();

		UIManager.Instance.SwitchPhase (UIPhase.GAMEOVER);

		MyInput.GetInputStream().Merge(UIManager.Instance.GetOnClickRetryStream ()).First().Subscribe (x => {
			GameManager.Instance.State.Value = nextState;
		}).AddTo (this);

		GameObject.FindObjectsOfType<Enemy> ().ToList ().ForEach (enemy => {
			enemy.hp.OnDamage(enemy.hp.MaxHP);
		});
	}

	public override void End ()
	{
		base.End ();
	}
}
