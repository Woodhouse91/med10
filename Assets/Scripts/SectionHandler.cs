using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionHandler : MonoBehaviour {

    List<GameObject> sections;
    public GameObject sectionPrefab;
    Vector3 startPos;
    float startHorizontalLength = 0.6f;
    public float horizontalLength;
    bool placed = false;
    bool isOffset = false, isMoving;
    int prevMainSection = 0;
    public Transform lookat;
    Rigidbody rig;
    // Use this for initialization
    void Start () {
        startPos = new Vector3(-0.175f, 0.05f, 0.0f);
        sections = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            sections.Add(transform.GetChild(i).gameObject);
        }
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {


        //FOR AT IKKE LAVE PÅ MARKØREN:
        horizontalLength = (0.075f + sections.Count * 0.175f);
        if(!placed)
        {
            rig.velocity += transform.right * Input.GetAxis("HorizontalKL")* 0.05f;
        }

        //Bounds
        if(transform.localPosition.x < startHorizontalLength - horizontalLength)
        {
            rig.velocity += -transform.right * (transform.localPosition.x - (startHorizontalLength - horizontalLength)) * 2f;
        }
        if(transform.localPosition.x > 0)
        {
            rig.velocity += -transform.right * transform.localPosition.x * 2f;
        }
        if (rig.velocity.magnitude > 0.01f)
        {
            isMoving = true;
            isOffset = false;
        }
        else
        {
            isMoving = false;
        }
        
        // place the nearest 3
        if (rig.velocity.magnitude < 0.05f && !placed)
        {
            PlaceSections();
        }

        rotateSections();

    }
    IEnumerator doOffset(Transform obj, Vector3 dir, float magnitude)
    {
        float t = 0f;
        Vector3 orgPos = obj.localPosition;
        dir *= magnitude;
        while (t < 0.1f)
        {
            obj.localPosition = orgPos + dir * t * 10f;
            t += Time.deltaTime;
            if (t > 0.1)
            {
                obj.localPosition = orgPos + dir;
            }
            yield return null;
        }
        yield break;
    }
    void rotateSections()
    {
        Vector3 sideOffset = Vector3.zero;
        print(rig.velocity.magnitude);
        float magn = rig.velocity.magnitude / 0.3f > 1f ? 0f : 1f - rig.velocity.magnitude / 0.3f; //VELOCITY DEPENDENT
        float xf = transform.localPosition.x; // minus sections * 0.175
        for (int i = 0; i < sections.Count; i++)
        {
            float offset = Mathf.Abs((xf + (i-1) * 0.175f) * 0.25f);
            if (xf + (float)i * 0.175f >= 0.405f)
                sideOffset = new Vector3(0.03f*magn, 0, 0);
            else if (xf + (float)i * 0.175f <= -0.05f)
                sideOffset = new Vector3(-0.03f*magn, 0, 0);
            else
                sideOffset = new Vector3(0, 0, 0);
            if (xf + (float)i * 0.175f <= 0.405f && xf + (float)i * 0.175f >= -0.05f)
            {
                offset = 0.0575f;
                
            }
            Vector3 moveForward = startPos + new Vector3(i*0.175f, 0, -offset + 0.0575f );
            if (moveForward.z < -0.012f)
            {
                moveForward += sideOffset;
                sections[i].transform.LookAt(lookat);
                sections[i].transform.localEulerAngles = new Vector3(0, sections[i].transform.localEulerAngles.y, 0);
            }
            else
                sections[i].transform.localRotation = Quaternion.identity;
            sections[i].transform.localPosition = moveForward;
        }
    
    }
    void ImplementFromData(string Data)
    {
        //use data from string or whatever to implement all the sections and money for the sections here
    }


    public void AddSection()
    {
        startPos = new Vector3(-0.175f, 0.05f, 0.0f);
        GameObject newPrefab = Instantiate(sectionPrefab,transform);
        newPrefab.transform.localPosition = startPos + new Vector3(0.175f * sections.Count, 0, 0);

        sections.Add(newPrefab);
    }
    public Vector3 OffsetSections(int x, bool set, int k)
    {
        if(set && !isOffset)
        {
            if (k < x)
            {
                return  new Vector3(-0.030f, 0, 0);
            }
            if (k > x+3)
            {
                return  new Vector3(0.030f, 0, 0);
            }
        }
        if(!set && isOffset)
        {

            if (k < x)
            {
                return new Vector3(0.030f, 0, 0);
            }
            if (k > x + 3)
            {
                return new Vector3(-0.030f, 0, 0);
            }
        }
        return Vector3.zero;
    }
    public void PlaceSections()
    {
        placed = true;
        rig.velocity = Vector3.zero;
        float xf = transform.localPosition.x;
        xf /= -0.175f;
        int x = Mathf.RoundToInt(xf);
        // x = 0 is furthest left //x = sections.count -2 is the furthest right
        print("x: " + x);
        prevMainSection = x;
        StartCoroutine(PlaceTheClosestSection(x));

    }
    IEnumerator PlaceTheClosestSection(int x)
    {
       
        Vector3 setPos = new Vector3(x * -0.175f,0,0);
        float t = 0; 
        while (t<0.5f)// The time it takes to set the sections in place (in seconds)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, setPos, t); 
            t += Time.deltaTime;
            yield return null;
        }
        rotateSections();
        //OffsetSections(x, true);
        placed = false;
        yield return null;
    }
    public void ResetSectionList()
    {
        if(sections != null)
            print("deleting: " + sections.Count+ " SectionCubes");
        else
        {
            print("creating new list..");
            sections = new List<GameObject>();
            return;
        }
        for (int i = 0; i < sections.Count; i++)
        {
            DestroyImmediate(sections[i]);
        }
        sections = new List<GameObject>();
    }

}
