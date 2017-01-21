using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Hp : MonoBehaviour
{
	public float MaxHP;

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
}
