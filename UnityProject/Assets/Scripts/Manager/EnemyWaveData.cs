using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EnemyWaveData : ScriptableObject<EnemyWave>
{
	[MenuItem("Assets/Create/Battle/EnemyWave")] // MenuItemの名前は適当でいいが、Assets/Create/内に入れておくと便利
	public static void Create()
	{
		Create<EnemyWaveData>(); // Create<>の中にはこのクラスの名前を書く
	}
}



