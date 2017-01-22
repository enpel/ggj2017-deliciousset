using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Enemy : MonoBehaviour
{
	public IWeapon weapon { get; private set; }
	public Move move{get; private set;}
	public Hp hp {get; private set;}

	[SerializeField]
	private int baseScore;

	[SerializeField]
	private float attackArea; // 攻撃する範囲
	[SerializeField]
	private SoundId deadSE; // やられた時に鳴る音
	[SerializeField]
	private GameObject deadEffectPrefab; // やられた時に出るエフェクト

	private Transform target;
	private Transform trans;

	void Awake()
	{
		weapon = GetComponent<IWeapon>();
		move = GetComponent<Move>();
		hp = GetComponent<Hp>();
		hp.MaxHP *= GameManager.Instance.Inflation;

		hp.IsDead.Where(dead => dead)
			.Subscribe(_ => OnDead())
			.AddTo(this);
	}

	void OnDead()
	{
		if (deadEffectPrefab != null)
		{
			var go = GameObject.Instantiate(deadEffectPrefab);
			var effectTrans = go.transform;
			effectTrans.SetParent(transform.parent);	// 弾は消えるので親オブジェクトに関連付ける
			effectTrans.localPosition = transform.localPosition;
			effectTrans.localScale = Vector3.one * Mathf.Sqrt(baseScore); // 敵によってエフェクトの大きさを変更してみる
		}

		this.GetComponent<Collider2D>().enabled = false;
		deadSE.Play();
		Destroy(gameObject);
	}

	void Start()
	{
		trans = transform;
		target = GameObject.FindWithTag("Player").transform;
	}

	void Update()
	{
		if (target != null)
		{
			var sqrMagnitude = (trans.position - target.position).sqrMagnitude;
			if (attackArea * attackArea > sqrMagnitude)
			{
				weapon.Shot();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var bullet = other.GetComponent<BulletBase>();
		var damage = bullet.CurrentPower;
		hp.OnDamage(damage);
		/*
		Debug.Log(string.Format("{0}は{1}に{2}ポイントのダメージ{3}", other.name, gameObject.name, damage, hp.ToDisplayString()));
		*/
		// 弾にあたって死んだ時のみ、スコアが増える
		if (hp.IsDead.Value)
		{
			// 後でスコアを敵ごとに設定する
			var score = (int)Mathf.Max(1, baseScore * Mathf.Sqrt(hp.CurrentHP.Value));
			GameManager.Instance.Score.Value += score;
			// 敵を倒すと必ずアイテムを入手できる
			if ((baseScore - Random.Range (0, 5)) > 0) {
				TechnologyManager.Instance.AddRandomTechnology(1);
			}

			// 上のIsDeadでもコライダーを外しているが、１フレーム遅れている可能性があるのでこの場でもコライダーを消す
			this.GetComponent<Collider2D>().enabled = false;
		}
	}
}
