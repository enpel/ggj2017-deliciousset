using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using Random = UnityEngine.Random;

public enum TechnologyType
{
	PowerUpBasicBullet1,	// 威力アップ
	PowerUpBasicBullet2,
	PowerUpBasicBullet3,
	OptionalBasicBullet1,	// 弾数アップ
	OptionalBasicBullet2,
	OptionalBasicBullet3,
	EnegryChargeRate,		// チャージ速度アップ
}

// テクノロジー
// アプリを終了したら消える
// 次のゲームには引き継がれる
public class TechnologyManager : SingletonMonoBehaviour<TechnologyManager>
{
	public ReactiveDictionary<TechnologyType, int> currentTechnologys = new ReactiveDictionary<TechnologyType, int>();

	public void Initialize()
	{
		for(var i = 0; i < Enum.GetNames(typeof(TechnologyType)).Length; i++)
		{
			var type = (TechnologyType)i;
			currentTechnologys.Add(type, 5);
		}
	}

	public int AddRandomTechnology(float num)
	{
		// 端数の確率を加味した数値に変換
		var addNum = (int)(num + Random.Range(0, 1));

		if (addNum == 0) return 0;

		// 同じものを一気に取得するような感じ
		var type = (TechnologyType)Random.Range(0, Enum.GetNames(typeof(TechnologyType)).Length);
		currentTechnologys[type] += addNum;

		SoundId.ggj_se01.Play(0, sc =>{
			sc.ChangePitchScale(3);
		});

		Debug.Log(string.Format("{0}を{1}個入手しました。", type, addNum));

		return addNum;
	}
}