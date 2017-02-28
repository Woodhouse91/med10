using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render2Texture : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().targetTexture = Resources.Load<RenderTexture>("ViveFrontCamTexture");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
