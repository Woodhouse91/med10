using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segmenthider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnTriggerExit(Collider other){
        other.GetComponent<MeshRenderer>().enabled = true;
    }
}
