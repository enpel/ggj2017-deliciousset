using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
	List<ParticleSystem> particles;

	void Start()
	{
		particles = GetComponentsInChildren<ParticleSystem>().ToList();
	}

	public void Show()
	{
		foreach(var particle in particles)
		{
			var emit = particle.emission;
			emit.enabled = true;
		}
	}

	public void Hide()
	{
		foreach(var particle in particles)
		{
			var emit = particle.emission;
			emit.enabled = false;
		}
	}
}
