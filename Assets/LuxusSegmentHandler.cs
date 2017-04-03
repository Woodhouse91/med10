using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxusSegmentHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.OnBoxAtTable += GenerateTable;
	}

    private void GenerateTable(BoxBehaviour s)
    {
    }

    // Update is called once per frame
    void Update () {
		
	}
    public static void ExposeTable()
    {

    }
    public static void ReleaseTable()
    {

    }

}
