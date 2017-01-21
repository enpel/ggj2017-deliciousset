using UnityEngine;

//http://terasur.blog.fc2.com/blog-entry-311.html
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = (T)FindObjectOfType (typeof(T));

				if (_instance == null) {
					//Debug.LogError (typeof(T) + "is nothing");
				}
			}

			return _instance;
		}
	}

	protected virtual void Awake()
	{
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if( this == Instance){ return true;}
		DestroyImmediate(this);
		return false;
	}

	protected void OnDestroy()
	{
		if(_instance == this)
		{
			_instance = null;
		}
	}
}