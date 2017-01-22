using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegionData : ScriptableObject<EnemyRegion>
{
	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Create/Battle/EnemyRegion")] // MenuItemの名前は適当でいいが、Assets/Create/内に入れておくと便利
	public static void Create()
	{
		Create<EnemyRegionData>(); // Create<>の中にはこのクラスの名前を書く
	}
	#endif
}



