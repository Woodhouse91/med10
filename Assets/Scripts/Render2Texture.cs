using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render2Texture : MonoBehaviour {
    public bool realWorld = false;
	// Use this for initialization
	void Start () {
        if(realWorld)
            GetComponent<Camera>().targetTexture = Resources.Load<RenderTexture>("realworldcam");
        else
            GetComponent<Camera>().targetTexture = Resources.Load<RenderTexture>("ViveFrontCamTexture");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
