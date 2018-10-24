using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    float pitch;

	// Use this for initialization
	void Start () {
        pitch = Random.Range(200, 600);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider collision)
	{
        
        if (string.Equals(tag, "noteOn") && string.Equals(collision.tag, "playhead"))
        {
            Debug.Log("here");
            GetComponent<ChuckSubInstance>().RunCode(@"
            SinOsc foo => dac;
            " + pitch + @"=> foo.freq;
            100::ms => now;
        ");
        }
	}
}
