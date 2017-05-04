using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookatplayer : MonoBehaviour {
    Quaternion rot;
    Transform actor;
	// Use this for initialization
	void Start () {
        actor = GameObject.Find("Camera (eye) WORLD").transform;
	}
	
	// Update is called once per frame
	void Update () {
        rot = Quaternion.LookRotation(transform.position - actor.position);
        transform.rotation = rot;
	}
}
