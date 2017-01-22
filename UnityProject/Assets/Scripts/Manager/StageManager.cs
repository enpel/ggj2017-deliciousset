using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
	[SerializeField] List<Stage> stages;

	int currentStageNum;

	void Start()
	{
		foreach(var stage in stages)
		{
			stage.Hide();
		}

		GameManager.Instance.Wave.Subscribe(waveNum =>{
			var newStageNum = (waveNum - 1) % stages.Count;
			ChangeStage(newStageNum);
		});
	}

	void ChangeStage(int newStageNum)
	{
		stages[currentStageNum].Hide();

		stages[newStageNum].Show();
		currentStageNum = newStageNum;
	}
}
