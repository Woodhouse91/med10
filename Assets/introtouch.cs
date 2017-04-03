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
        sibling = transform.parent.GetChild(1).gameObject;
        overlay = GameObject.Find("Overlay (Canvas)");
        overlay.SetActive(false);
        //sibling.GetComponent<ListBudgets>().enabled = false;
        sibling.SetActive(false);
        fader = GameObject.Find("BlackOut");
        TouchManager.Instance.TouchesBegan += go;
	}

    private void go(object sender, TouchEventArgs e)
    {
        fader.GetComponent<fadeBlack>().fade();
        GetComponent<Text>().enabled = false;
        StartCoroutine(lateActive());
    }
    IEnumerator lateActive()
    {
        yield return new WaitForSeconds(fader.GetComponent<fadeBlack>().fadeTime);
        sibling.SetActive(true);
        sibling.AddComponent<ListBudgets>();
        sibling.GetComponent<ListBudgets>().setOverlay(overlay);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        try
        {
            TouchManager.Instance.TouchesBegan -= go;
        }catch { }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
