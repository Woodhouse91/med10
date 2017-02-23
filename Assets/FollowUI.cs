using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour {
    [SerializeField]
    Transform tar;

	// Use this for initialization
	public void setPos () {
        transform.position = tar.position-tar.forward*1;
        transform.rotation = tar.rotation;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
