using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MoveButtTest : MonoBehaviour, IDragHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


  

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
