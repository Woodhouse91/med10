using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtualscreen : MonoBehaviour {
    [SerializeField]
    private Transform screen;

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    public void Setpos () {
        transform.position = screen.position;
        transform.rotation = screen.rotation;
    }
}
