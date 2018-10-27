using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStars : MonoBehaviour {

    Vector3 start_pos;
    Vector3 end_pos;
    float distance;
    public GameObject line;
    public GameObject star;
    public GameObject linePrefab;
    bool first;
    int mode;

    public Material invisible;
    public Material normal;

    bool spinning;

    public GameObject playhead;

    GameObject startObject;
    GameObject endObject;

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

	// Use this for initialization
	void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

        spinning = false;
        mode = 1;
        first = true;
        for (int i = 0; i < 500; i++)
        {
            Instantiate(star, 10*Random.onUnitSphere, Quaternion.Euler(0,0,0));
        }
	}

    Vector3 GetMidpoint(Vector3 starting, Vector3 ending)
    {
        float x = starting.x + (ending.x - starting.x) / 2;
        float y = starting.y + (ending.y - starting.y) / 2;
        float z = starting.z + (ending.z - starting.z) / 2;
        return new Vector3(x, y, z);
    }

    Quaternion GetRotation(Vector3 starting, Vector3 ending)
    {
        float angle = Vector3.Angle(starting, ending);
        return Quaternion.Euler(0, 0, angle);
    }

	void Update()
	{

        if (Controller.GetHairTrigger())
        {
            Debug.Log("WOOHOO");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mode = 2;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spinning = spinning == false;
        }

        if (spinning)
        {
            playhead.transform.GetComponent<MeshRenderer>().material = invisible;
            transform.Rotate(0, 1, 0);
        }
        else
        {
            playhead.transform.GetComponent<MeshRenderer>().material = normal;
        }
        playhead.transform.Rotate(1, 0, 0);


        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.Split('(')[0] == "Star")
                {
                    if (first)
                    {
                        startObject = hit.transform.gameObject;
                        start_pos = hit.transform.position;
                        startObject.GetComponent<Star>().tag = "noteOn";
                        startObject.GetComponent<Star>().SetMode(1);
                        if (!startObject.GetComponent<ChuckSubInstance>())
                        {
                            startObject.AddComponent<ChuckSubInstance>();
                        }
                        first = false;
                    }
                    else if (hit.transform.position != start_pos)
                    {
                        endObject = hit.transform.gameObject;
                        end_pos = hit.transform.position;
                        float lineDistance = Vector3.Distance(start_pos, end_pos)/2;
                        GameObject newLine = Instantiate(linePrefab);
                        newLine.transform.position = GetMidpoint(start_pos, end_pos);
                        newLine.transform.localScale = new Vector3(0.05f, lineDistance, 0.05f);
                        newLine.transform.rotation = Quaternion.FromToRotation(Vector3.up, end_pos-start_pos);
                        newLine.GetComponent<Line>().SetMode(mode);
                        newLine.GetComponent<Line>().SetStars(startObject, endObject);
                        first = true;
                    }
                }
            }
            Vector3 new_end = ray.GetPoint(10);

            line.SetActive(true);
            distance = Vector3.Distance(start_pos, new_end)/2;
            line.transform.position = GetMidpoint(start_pos, new_end);
            line.transform.localScale = new Vector3(0.05f, distance, 0.05f);
            line.transform.rotation = Quaternion.FromToRotation(Vector3.up, new_end - start_pos);
        }
        else 
        {
            line.SetActive(false);
            first = true;
        }
	}
}
