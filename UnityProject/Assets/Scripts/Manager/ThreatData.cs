using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThreatData : ScriptableObject<ThreatSetting>
{
	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Create/Battle/ThreatData")] // MenuItemの名前は適当でいいが、Assets/Create/内に入れておくと便利
	public static void Create()
	{
		Create<ThreatData>(); // Create<>の中にはこのクラスの名前を書く
	}
	#endif

}

[Serializable]
public class ThreatSetting
{
	public ThreatRank rank;
	public float spawnRate;
	public float lifeTime;
	public ThreatRank next;
	public List<SoundId> bgmIds = new List<SoundId>();
}
