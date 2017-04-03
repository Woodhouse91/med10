using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeBlack : MonoBehaviour {

    MeshRenderer mat;
    public float fadeTime;
    GameObject global;

	// Use this for initialization
	void Start () {
        mat = GetComponent <MeshRenderer>();
        global = GameObject.Find("Global Components");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOut());
        }
	}

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while(t<1f)
        {
            t += Time.deltaTime / fadeTime;
            mat.material.SetFloat("_Fade", 1-t*t);
            yield return null;
        }
        mat.enabled = false;
        global.GetComponent<OptimizedCameraTex>().changeCamera();
        yield return null;
    }
    public void fade()
    {
        StartCoroutine(FadeOut());
    }
}
