using System.IO;
using UnityEngine;

public class ScriptableObject<T> : ScriptableObject
{
	[SerializeField] private T _data;

	public T Data
	{
		get { return _data; }
		set { _data = value; }
	}

	public static void Create<T2>() where T2 : ScriptableObject<T>, new()
	{
		#if UNITY_EDITOR
		var scriptableObject = new T2();

		Debug.Log(typeof(T).Name);
		var fileName = typeof(T).Name + ".asset";

		var path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(Path.Combine(GetSelectingAssetDirectory(), fileName));

		UnityEditor.AssetDatabase.CreateAsset(scriptableObject, path);
		UnityEditor.AssetDatabase.SaveAssets();
		#endif
	}

	#if UNITY_EDITOR
	private static string GetSelectingAssetDirectory()
	{
		if (UnityEditor.Selection.assetGUIDs.Length == 0) return "Assets";

		var directoryPath = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.Selection.assetGUIDs[0]);

		return Directory.Exists(directoryPath)
			? directoryPath
				: Path.GetDirectoryName(directoryPath);

	}
	#endif
}
