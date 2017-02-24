using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtualscreen : MonoBehaviour {
    [SerializeField]
    private Transform screen;
    private float behind = 0.001f;
    private FollowUI fui;

	// Use this for initialization
	void Start () {
        fui = FindObjectOfType<FollowUI>();
    }

    // Update is called once per frame
    public void Setpos () {
        if (fui == null)
            fui = FindObjectOfType<FollowUI>();
        transform.position = screen.position + screen.forward * behind;
        transform.rotation = screen.rotation;
        fui.setPos();
    }
}
