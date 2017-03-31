using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeBlack : MonoBehaviour {

    MeshRenderer mat, projTar;
    GameObject proj;
    public float fadeTime;

	// Use this for initialization
	void Start () {
        mat = GetComponent <MeshRenderer>();
        proj = GameObject.Find("RealWorldProj");
        projTar = GameObject.Find("ProjectorTarget").GetComponent<MeshRenderer>();
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
            proj.GetComponent<Projector>().material.SetFloat("_Alpha", 1 - t * t);
            yield return null;
        }
        mat.enabled = false;
        proj.SetActive(false);
        GameObject.Find("REALWORLD").SetActive(false);
        yield return null;
    }
    public void fade()
    {
        StartCoroutine(FadeOut());
    }
}
