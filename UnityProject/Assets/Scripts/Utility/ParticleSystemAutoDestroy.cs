using UnityEngine;
using System.Collections;

// http://answers.unity3d.com/questions/219609/auto-destroying-particle-system.html
public class ParticleSystemAutoDestroy : MonoBehaviour 
{
	private ParticleSystem ps;


	public void Start() 
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Update() 
	{
		if(ps)
		{
			if(!ps.IsAlive())
			{
				Destroy(gameObject);
			}
		}
	}
}