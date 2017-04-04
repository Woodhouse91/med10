using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSlidePlz : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
            transform.position -= transform.right / 2.5f;
        if(transform.localPosition.x < -1f)
        {
            EventManager.CategoryDone();
            transform.localPosition = Vector3.zero;
        }
	}
}
