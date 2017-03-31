using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetShader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Projector>().material.SetFloat("_Alpha", 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
