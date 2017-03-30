using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeBlack : MonoBehaviour {

    MeshRenderer mat;
    public float fadeTime;

	// Use this for initialization
	void Start () {
        mat = GetComponent <MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOut());
            GameObject.Find("ScreenInFace").SetActive(false);
        }
	}

    private IEnumerator FadeOut()
    {
        print("bye");
        float t = 0f;
        while(t<1f)
        {
            t += Time.deltaTime / fadeTime;
            mat.material.SetFloat("_Fade", 1-t*t);
            yield return null;
        }
        mat.enabled = false;
        yield return null;
    }
}
