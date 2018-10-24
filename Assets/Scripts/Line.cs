using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    GameObject star1;
    GameObject star2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetStars(GameObject first, GameObject second)
    {
        star1 = first;
        star1.tag = "noteOn";
        star2 = second;
        star2.tag = "noteOn";
    }

	private void OnDestroy()
	{
        star1.tag = "noteOff";
        star2.tag = "noteOff";
	}
}
