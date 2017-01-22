using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	[SerializeField]
	UIManager uiManager;
	[SerializeField]
	SceneState defaultState;
	[SerializeField]
	TechnologyManager technologyManager;
	public TechnologyManager TechnologyManager { get { return technologyManager; } }

	public ReactiveProperty<IState> State = new ReactiveProperty<IState>();

	public ReactiveProperty<int> Wave = new ReactiveProperty<int>(1);
	public ReactiveProperty<int> Score = new ReactiveProperty<int>(0);

	// Use this for initialization
	void Start () {
		TechnologyManager.Instance.Initialize();

		uiManager.SwitchPhase (UIPhase.NONE);
		State.Pairwise().Subscribe(statePair => {
			if (statePair.Previous != null)
				statePair.Previous.End();
			if (statePair.Current != null)
				statePair.Current.Initialize();
		}).AddTo (this);

		State.Value = defaultState;
	}
		
}

public interface IState
{
	void Initialize();
	void End();
}
