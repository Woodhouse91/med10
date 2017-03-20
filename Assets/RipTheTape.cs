using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipTheTape : MonoBehaviour {


    float testT= 0f;
    LineRenderer tape;
    ParticleSystem ps1,ps2;
    public int NumOfPos;
    Vector3[] pos;
    bool destroyed = false;
    private float distance;
	// Use this for initialization
	void Start () {
        ps1 = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps2 = transform.GetChild(1).GetComponent<ParticleSystem>();

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
        testT += Time.deltaTime * 1f;
        SetTapeDist(testT);
	}
    public void SetTapeDist(float dist)
    {
        if(dist >= 1f && destroyed == false)
        {
            destroyed = true;
            StartCoroutine(fadeOut());
        }
        else if (dist != distance)
        {
            if(dist - distance>0)
            {
                ps1.Emit( (int)((dist-distance)*1000f));
                ps1.transform.localPosition = new Vector3(-10f * dist,0f,0f);
            } 
            
            distance = dist;
            for (int i = 0; i < pos.Length; i++)
            {
                // startpos (0,0,0) endpos (-10,0,0) 
                if ((float)i / NumOfPos <= dist)
                    pos[i] = new Vector3(-(-10f *i / NumOfPos+ dist * 10f)+dist*-10f,0f, (dist * -(NumOfPos-i)/NumOfPos));
            }
            tape.SetPositions(pos);
        }
     
    }
    IEnumerator fadeOut()
    {
        ps2.Emit(100);
        float t = 1f;
        while(t>0)
        {
            tape.startWidth = t;
            tape.endWidth = t;
            t -= Time.deltaTime;
            yield return null;
        }
        GetComponentInParent<BoxBehaviour>().tapeRipped();
        Destroy(gameObject);
        yield return null;
    }
}
