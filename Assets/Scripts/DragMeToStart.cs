using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragMeToStart : MonoBehaviour {

    Color glow;
    Image mat;
	// Use this for initialization
	void Start () {
        mat = GetComponent<Image>();
        glow = mat.color;
	}
	
	// Update is called once per frame
	void Update () {

        glow.b = Time.time * 0.50f%1;
        glow.g = Time.time * 0.50f % 1;
        glow.r = Time.time * 0.50f % 1;
        mat.color = glow;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right*0.1f);
        }
	}
}
