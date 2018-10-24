using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus : MonoBehaviour {

    Collider halfCollider;
	// Use this for initialization
	void Start () {
        halfCollider = GetComponent<Collider>();
        halfCollider.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
