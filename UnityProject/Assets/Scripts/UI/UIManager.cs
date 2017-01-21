using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public enum UIPhase
{
	NONE,
	TITLE,
	INGAME,
	GAMEOVER,
	GAMECLEAR,
}
public class UIManager : SingletonMonoBehaviour<UIManager> {

	// Use this for initialization
	public GameObject group_title;
	public GameObject group_ingame;
	public GameObject group_gameover;
	public GameObject group_gameclear;

	public Button button_start;
	public Button button_retry;
	public Button button_backtotitle;

	IObservable<Unit> onClickStart;
	IObservable<Unit> onClickRetry;
	IObservable<Unit> onClickBackToTitle;

	void Start () {
	}

	public IObservable<Unit> GetOnClickStartStream()
	{
		if (onClickStart == null)
		{
			onClickStart = button_start.OnClickAsObservable();
		}
		return onClickStart;
	}
	public IObservable<Unit> GetOnClickRetryStream()
	{
		if (onClickRetry == null)
		{
			onClickRetry = button_retry.OnClickAsObservable();
		}
		return onClickRetry;
	}
	public IObservable<Unit> GetOnClickBackToTitleStream()
	{
		if (onClickBackToTitle == null)
		{
			onClickBackToTitle = button_backtotitle.OnClickAsObservable();
		}
		return onClickBackToTitle;
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void SwitchPhase(UIPhase phase)
	{
		switch (phase) {
		case UIPhase.NONE:
			group_title.SetActive (false);
			group_ingame.SetActive (false);
			group_gameover.SetActive (false);
			group_gameclear.SetActive (false);
			break;
		case UIPhase.TITLE:
			group_title.SetActive (true);
			group_ingame.SetActive (false);
			group_gameover.SetActive (false);
			group_gameclear.SetActive (false);
			break;
		case UIPhase.INGAME:
			group_title.SetActive (false);
			group_ingame.SetActive (true);
			group_gameover.SetActive (false);
			group_gameclear.SetActive (false);
			break;
		case UIPhase.GAMEOVER:
			group_title.SetActive (false);
			group_ingame.SetActive (false);
			group_gameover.SetActive (true);
			group_gameclear.SetActive (false);
			break;
		case UIPhase.GAMECLEAR:
			group_title.SetActive (false);
			group_ingame.SetActive (false);
			group_gameover.SetActive (false);
			group_gameclear.SetActive (true);
			break;
		}
	}
}
