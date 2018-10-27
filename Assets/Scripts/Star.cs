using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    float pitch;
    int mode;
    string chuckCode;

    bool hasNext;
    float nextPitch;
    float nextDistance;

	// Use this for initialization
	void Start () {
        hasNext = false;
        mode = 1;
        pitch = (10+transform.position.y) * 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider collision)
	{
        if (string.Equals(tag, "noteOn") && string.Equals(collision.tag, "playhead"))
        {
            GetComponent<ChuckSubInstance>().RunCode(chuckCode);
        }
	}

    public void HasNext(bool next)
    {
        hasNext = next;
    }

    public void SetNext(float pitch, float distance)
    {
        nextPitch = pitch;
        nextDistance = distance;
    }

    public float GetPitch()
    {
        return pitch;
    }

    public void SetMode(int newMode)
    {
        mode = newMode;
        if (mode == 1)
        {
            transform.GetChild(0).GetComponent<Light>().range = 0.5f;
            chuckCode = @"
                SinOsc foo => ADSR a => Echo e => dac;
                e.delay(800::ms);
                e.mix(0.2);
                a.set( 10::ms, 8::ms, .5, 500::ms );
                " + pitch + @"=> foo.freq;
                a.keyOn();
                100::ms => now;
                a.keyOff();
                1::second => now;
            ";
        }
        // 50 frames per second
        else if (mode == 2)
        {
            if (hasNext)
            {
                chuckCode = @"
                    SinOsc foo => dac;
                    " + nextPitch + @" => float targetPitch;
                    " + pitch + @" => float startingPitch;
                    0 => float t;
                    (" + nextDistance + @"/70)*1000 => float targetTime;

                    targetTime/(Std.fabs(targetPitch-startingPitch)) => float timeStep;

                    startingPitch => float currPitch;

                    while (t < targetTime)
                    {
                        currPitch => foo.freq;
                        if (targetPitch > startingPitch)
                        {
                            1 +=> currPitch;
                        }
                        else
                        {
                            1 -=> currPitch;
                        }
                        timeStep +=> t;
                        timeStep::ms => now;
                    }
                ";
            }
            //else{
            //    chuckCode = @"
            //        SinOsc foo => ADSR a => Echo e => dac;
            //        e.delay(800::ms);
            //        e.mix(0.2);
            //        a.set( 10::ms, 8::ms, .5, 500::ms );
            //        " + pitch + @"=> foo.freq;
            //        a.keyOn();
            //        100::ms => now;
            //        a.keyOff();
            //        1::second => now;
            //    ";
            //}
        }
    }
}
