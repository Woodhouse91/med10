using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintDrawLineTo : MonoBehaviour {

    public Transform target;
    private LineRenderer lr;
    private Vector3[] lines;
	// Use this for initialization
	void Start () {
        lines = new Vector3[2];
        lr = GetComponent<LineRenderer>();
        lines[0] = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        lines[1] = target.position;
        lines[0] = transform.position;
        lr.SetPositions(lines);
	}
}
