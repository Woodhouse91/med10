using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionHandler : MonoBehaviour {

    List<GameObject> sections;
    public GameObject sectionPrefab;
    Vector3 startPos;
    float startHorizontalLength = 0.6f;
    public float horizontalLength;
    Rigidbody rig;
    // Use this for initialization
    void Start () {
        startPos = new Vector3(-0.294f, -0.2f, 0.01f);
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
        rig.velocity += transform.right * Input.GetAxis("HorizontalKL")* 0.05f;
        if(transform.localPosition.x < startHorizontalLength - horizontalLength)
        {
            rig.velocity += -transform.right * (transform.localPosition.x - (startHorizontalLength - horizontalLength)) * 2f;
        }
        if(transform.localPosition.x > 0)
        {
            rig.velocity += -transform.right * transform.localPosition.x * 2f;
        }
        if (rig.velocity.magnitude < 1)
        {
            PlaceSections();
        }
	}

    void ImplementFromData(string Data)
    {
        //use data from string or whatever to implement all the sections and money for the sections here
    }


    public void AddSection()
    {
        startPos = new Vector3(-0.294f, -0.2f, 0.01f);
        GameObject newPrefab = Instantiate(sectionPrefab,transform);
        newPrefab.transform.localPosition = startPos + new Vector3(0.175f * sections.Count, 0, 0);

        sections.Add(newPrefab);
    }
    public void PlaceSections()
    {
        
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
