﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenParticleEnd : MonoBehaviour {

	ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ps) {
			if (ps.IsAlive() == false) {
				Destroy (gameObject);
			}
		}
	}
}
