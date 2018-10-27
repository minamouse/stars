using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    GameObject star1;
    GameObject star2;
    int mode;

	// Use this for initialization
	void Start () {
        mode = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetStars(GameObject first, GameObject second)
    {
        star1 = first;
        star2 = second;

        if (!star1.GetComponent<ChuckSubInstance>())
        {
            star1.AddComponent<ChuckSubInstance>();
        }

        Vector3 pos1 = new Vector3(star1.transform.position.x, 0, star1.transform.position.z);
        Vector3 pos2 = new Vector3(star2.transform.position.x, 0, star2.transform.position.z);

        float a = Vector3.Cross(pos1, pos2).y;

        if (a > 0)
        {
            star1.GetComponent<Star>().HasNext(true);
            star1.GetComponent<Star>().SetNext(star2.GetComponent<Star>().GetPitch(), a);
        }
        else
        {
            star2.GetComponent<Star>().HasNext(true);
            star2.GetComponent<Star>().SetNext(star1.GetComponent<Star>().GetPitch(), -a);            
        }

        star1.GetComponent<Star>().SetMode(2);
        star1.tag = "noteOn";


        if (!star2.GetComponent<ChuckSubInstance>())
        {
            star2.AddComponent<ChuckSubInstance>();
        }
        star2.GetComponent<Star>().SetMode(2);
        star2.tag = "noteOn";
    }

	private void OnDestroy()
	{
        star1.tag = "noteOff";
        Destroy(star1.GetComponent<ChuckSubInstance>());

        star2.tag = "noteOff";
        Destroy(star2.GetComponent<ChuckSubInstance>());
	}

    public void SetMode(int newMode)
    {
        mode = newMode;
    }
}
