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

	public ReactiveProperty<IState> State = new ReactiveProperty<IState>();

	// Use this for initialization
	void Start () {
		State.Value = defaultState;
		State.Subscribe (state => {
			state.Initialize();
		}).AddTo (this);
	}
		
}

public interface IState
{
	void Initialize();
}
