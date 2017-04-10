using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;
using System;
using UnityEngine.UI;

public class introtouch : MonoBehaviour {
    GameObject fader, sibling, overlay;
	// Use this for initialization
	void Start () {
        fader = GameObject.Find("BlackOut");
        TouchManager.Instance.TouchesBegan += go;
	}

    private void go(object sender, TouchEventArgs e)
    {
        fader.GetComponent<fadeBlack>().fade();
        GetComponent<Text>().enabled = false;
    }
    private void OnDisable()
    {
        try
        {
            TouchManager.Instance.TouchesBegan -= go;
        }catch { }
    }
    public void setOverlay(GameObject o)
    {
        overlay = o;
    }
}
