using System.IO;
using UnityEditor;
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
		var scriptableObject = new T2();

		Debug.Log(typeof(T).Name);
		var fileName = typeof(T).Name + ".asset";

		var path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(GetSelectingAssetDirectory(), fileName));

		AssetDatabase.CreateAsset(scriptableObject, path);
		AssetDatabase.SaveAssets();
	}

	private static string GetSelectingAssetDirectory()
	{
		if (Selection.assetGUIDs.Length == 0) return "Assets";

		var directoryPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

		return Directory.Exists(directoryPath)
			? directoryPath
				: Path.GetDirectoryName(directoryPath);
	}
}