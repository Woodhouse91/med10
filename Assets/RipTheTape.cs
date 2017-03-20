using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipTheTape : MonoBehaviour {


    float testT= 0f;
    LineRenderer tape;
    public int NumOfPos;
    Vector3[] pos;
    private float distance;
	// Use this for initialization
	void Start () {
        tape = GetComponent<LineRenderer>();
        tape.numPositions = NumOfPos;
        pos = new Vector3[NumOfPos];
        for (int i = 0; i < pos.Length; i++)
        {
            // startpos (0,0,0) endpos (-10,0,0) 
            pos[i] = new Vector3(-10f*((float)i/NumOfPos), 0f, 0f);
        }
        tape.SetPositions(pos);
	}
	
	// Update is called once per frame
	void Update () {
        testT += Time.deltaTime * 0.1f;
        SetTapeDist(testT);
	}
    public void SetTapeDist(float dist)
    {
        if (dist != distance)
        {
            distance = dist;
            float nicovinder = 0;
            for (int i = 0; i < pos.Length; i++)
            {
                nicovinder = (i + 1) / (NumOfPos+1);
                // startpos (0,0,0) endpos (-10,0,0) 
                if ((float)i / NumOfPos <= dist)
                    pos[i] = new Vector3(-(-10f *i / NumOfPos+ dist * 10f)+dist*-10f, 0f, (dist * -(NumOfPos-i)/NumOfPos)*3f);
        
                //pos[i] = new Vector3((NumOfPos / (i + 1)) * AC.Evaluate((float)i / NumOfPos)*5f* distance *-3f + (-10 * ((float)i / NumOfPos)), 0f, 3f* -AC.Evaluate((float)i / NumOfPos) * 5f* distance);
            }
            tape.SetPositions(pos);
        }

        
    }
}
