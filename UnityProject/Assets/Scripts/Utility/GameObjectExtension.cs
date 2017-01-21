using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utility.UnityObject
{
	//http://qiita.com/baba_s/items/a0490ea36f6a5bcf8b07
	public static class GameObjectExtension
	{
		public static T SafeGetComponent<T>( this GameObject self ) where T : Component
		{
			return self.GetComponent<T>() ?? self.AddComponent<T>();
		}

		public static T SafeGetComponent<T>( this Component self ) where T : Component
		{
			return self.GetComponent<T>() ?? self.gameObject.AddComponent<T>();
		}

		public static GameObject[] GetChildren( this GameObject self, bool includeInactive = false )
		{
			return self
				.GetComponentsInChildren<Transform>( includeInactive )
				.Where( c => c != self.transform )
				.Select( c => c.gameObject )
				.ToArray();
		}

		public static GameObject[] GetChildren( this Component self, bool includeInactive = false )
		{
			return self
				.GetComponentsInChildren<Transform>( includeInactive )
				.Where( c => c != self.transform )
				.Select( c => c.gameObject )
				.ToArray();
		}

		public static GameObject[] GetChildrenWithoutGrandchildren( this GameObject self )
		{
			var result = new List<GameObject>();
			foreach ( Transform n in self.transform )
			{
				result.Add( n.gameObject );
			}
			return result.ToArray();
		}

		public static GameObject[] GetChildrenWithoutGrandchildren( this Component self )
		{
			var result = new List<GameObject>();
			foreach ( Transform n in self.transform )
			{
				result.Add( n.gameObject );
			}
			return result.ToArray();
		}

		public static T[] GetComponentsInChildrenWithoutSelf<T>( this GameObject self, bool includeInactive = false ) where T : Component
		{
			return self
				.GetComponentsInChildren<T>( includeInactive )
				.Where( c => self != c.gameObject )
				.ToArray();
		}

		public static T[] GetComponentsInChildrenWithoutSelf<T>( this Component self, bool includeInactive = false ) where T : Component
		{
			return self
				.GetComponentsInChildren<T>( includeInactive )
				.Where( c => self.gameObject != c.gameObject )
				.ToArray();
		}

		public static void RemoveComponent<T>( this GameObject self ) where T : Component
		{
			Object.Destroy( self.GetComponent<T>() );
		}

		public static void RemoveComponent<T>( this Component self ) where T : Component
		{
			Object.Destroy( self.GetComponent<T>() );
		}

		public static void RemoveComponentImmediate<T>( this GameObject self ) where T : Component
		{
			GameObject.DestroyImmediate( self.GetComponent<T>() );
		}

		public static void RemoveComponentImmediate<T>( this Component self ) where T : Component
		{
			GameObject.DestroyImmediate( self.GetComponent<T>() );
		}

		public static void RemoveComponents<T>( this GameObject self ) where T : Component
		{
			foreach( var n in self.GetComponents<T>() )
			{
				GameObject.Destroy( n );
			}
		}

		public static void RemoveComponents<T>( this Component self ) where T : Component
		{
			foreach( var n in self.GetComponents<T>() )
			{
				GameObject.Destroy( n );
			}
		}

		public static bool HasComponent<T>( this GameObject self ) where T : Component
		{
			return self.GetComponent<T>() != null;
		}

		public static bool HasComponent<T>( this Component self ) where T : Component
		{
			return self.GetComponent<T>() != null;
		}

		public static Transform Find( this GameObject self, string name )
		{
			return self.transform.Find( name );
		}

		public static Transform Find( this Component self, string name )
		{
			return self.transform.Find( name );
		}

		public static GameObject FindGameObject( this GameObject self, string name )
		{
			var result = self.transform.Find( name );
			return result != null ? result.gameObject : null;
		}

		public static GameObject FindGameObject( this Component self,  string name )
		{
			var result = self.transform.Find( name );
			return result != null ? result.gameObject : null;
		}

		public static T FindComponent<T>( this GameObject self, string name ) where T : Component
		{
			var t = self.transform.Find( name );
			if ( t == null )
			{
				return null;
			}
			return t.GetComponent<T>();
		}

		public static T FindComponent<T>( this Component self, string name ) where T : Component
		{
			var t = self.transform.Find( name );
			if ( t == null )
			{
				return null;
			}
			return t.GetComponent<T>();
		}

		public static GameObject FindDeep( this GameObject self, string name, bool includeInactive = false )
		{
			var children = self.GetComponentsInChildren<Transform>( includeInactive );
			foreach ( var transform in children )
			{
				if ( transform.name == name )
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		public static GameObject FindDeep( this Component self, string name, bool includeInactive = false )
		{
			var children = self.GetComponentsInChildren<Transform>( includeInactive );
			foreach ( var transform in children )
			{
				if ( transform.name == name )
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		public static void SetParent( this GameObject self, Component parent )
		{
			self.transform.SetParent( parent.transform );
		}

		public static void SetParent( this GameObject self, GameObject parent )
		{
			self.transform.SetParent( parent.transform );
		}

		public static void SetParent( this Component self, Component parent )
		{
			self.transform.SetParent( parent.transform );
		}

		public static void SetParent( this Component self, GameObject parent )
		{
			self.transform.SetParent( parent.transform );
		}

		public static bool HasChild( this GameObject self )
		{
			return 0 < self.transform.childCount;
		}

		public static bool HasChild( this Component self )
		{
			return 0 < self.transform.childCount;
		}

		public static bool HasParent( this GameObject self )
		{
			return self.transform.parent != null;
		}

		public static bool HasParent( this Component self )
		{
			return self.transform.parent != null;
		}

		public static GameObject GetChild( this GameObject self, int index )
		{
			var t = self.transform.GetChild( index );
			return t != null ? t.gameObject : null;
		}

		public static GameObject GetChild( this Component self, int index )
		{
			var t = self.transform.GetChild( index );
			return t != null ? t.gameObject : null;
		}

		public static GameObject GetParent( this GameObject self )
		{
			var t = self.transform.parent;
			return t != null ? t.gameObject : null;
		}

		public static GameObject GetParent( this Component self )
		{
			var t = self.transform.parent;
			return t != null ? t.gameObject : null;
		}

		public static GameObject GetRoot( this GameObject self )
		{
			var root = self.transform.root;
			return root != null ? root.gameObject : null;
		}

		public static GameObject GetRoot( this Component self )
		{
			var root = self.transform.root;
			return root != null ? root.gameObject : null;
		}

		public static void SetLayer( this GameObject self, string layerName )
		{
			self.layer = LayerMask.NameToLayer( layerName );
		}

		public static void SetLayer( this Component self, string layerName )
		{
			self.gameObject.layer = LayerMask.NameToLayer( layerName );
		}

		public static void SetLayerRecursively( this GameObject self, int layer )
		{
			self.layer = layer;

			foreach ( Transform n in self.transform )
			{
				SetLayerRecursively( n.gameObject, layer );
			}
		}

		public static void SetLayerRecursively( this Component self, int layer )
		{
			self.gameObject.layer = layer;

			foreach ( Transform n in self.gameObject.transform )
			{
				SetLayerRecursively( n, layer );
			}
		}

		public static void SetLayerRecursively( this GameObject self, string layerName )
		{
			self.SetLayerRecursively( LayerMask.NameToLayer( layerName ) );
		}

		public static void SetLayerRecursively( this Component self, string layerName )
		{
			self.SetLayerRecursively( LayerMask.NameToLayer( layerName ) );
		}

		public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
		{
			return go.GetComponent<T>() ?? go.AddComponent<T>();
		}

		public static T GetOrAddComponent<T>(this Transform trans) where T : UnityEngine.Component
		{
			return trans.gameObject.GetComponent<T>() ?? trans.gameObject.AddComponent<T>();
		}

		public static Transform SafeAddChild(this Component parent, string name, Vector3? localPos = null, Vector3? eulerAngle = null)
		{
			return parent.SafeAddChild(null,name,localPos,eulerAngle);
		}

		public static GameObject SafeAddChild(this GameObject parent, string name, Vector3? localPos = null, Vector3? eulerAngle = null)
		{
			return parent.SafeAddChild(null,name,localPos,eulerAngle);
		}

		public static Transform SafeAddChild(this Component parent, GameObject prefab, string name, Vector3? localPos = null, Vector3? eulerAngle = null)
		{
			var trans = parent.transform.Find(name);
			if(trans != null) return trans;

			var go = prefab == null ? new GameObject() : GameObject.Instantiate<GameObject>(prefab);
			go.name = name;

			if (parent != null)
			{
				trans = go.transform;
				trans.parent = parent.transform;
				trans.localPosition = localPos ?? Vector3.zero;
				trans.localRotation = Quaternion.Euler(eulerAngle ?? Vector3.zero);
				trans.localScale = Vector3.one;
				go.layer = parent.gameObject.layer;
			}
			return trans;
		}

		public static GameObject SafeAddChild(this GameObject parent, GameObject prefab, string name, Vector3? localPos = null, Vector3? eulerAngle = null)
		{
			return parent.transform.SafeAddChild(prefab,name,localPos,eulerAngle).gameObject;
		}
	}
}