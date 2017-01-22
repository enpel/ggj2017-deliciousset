using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
	public float MaxHP;
	public float Rate { get { return CurrentHP.Value/MaxHP; }}

	public ReactiveProperty<bool> IsDead = new ReactiveProperty<bool>();
	void UpdateDeadState()
	{
		IsDead.Value = (CurrentHP.Value == 0);
	}
	public ReactiveProperty<float> CurrentHP = new ReactiveProperty<float>();
	void UpdateHP(float newHP)
	{
		CurrentHP.Value = Mathf.Clamp(newHP, 0, MaxHP);
		UpdateDeadState();
		UpdateGraphicsColor();
	}

	void Start()
	{
		Init();
	}

	public void Init()
	{
		CurrentHP.Value = MaxHP;
	}

	public void OnDamage(float damage)
	{
		UpdateHP(CurrentHP.Value - damage);
	}

	public void OnRecovery(float recovery)
	{
		UpdateHP(CurrentHP.Value + recovery);
	}

	public string ToDisplayString()
	{
		return string.Format("HP({0}/{1})", CurrentHP, MaxHP);
	}

	void UpdateGraphicsColor()
	{
		//暗くなりすぎると見えないので、一番暗いときはalpha0.3f
		var alpha = (Rate * 0.7f) + 0.3f;
		foreach(var image in GetComponentsInChildren<Image>())
		{
			var color = image.color;
			color.a = alpha;
			image.color = color;
		}
	}
}
