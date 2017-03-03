using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Display.displays.Length > 1)
            for(int x = 0; x<Display.displays.Length; ++x)
                Display.displays[x].Activate();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
