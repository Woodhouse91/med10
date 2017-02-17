using UnityEngine;
using System.Collections;

public class followCAM : MonoBehaviour {

    public Transform frontcam, screen;
    private RectTransform can;
    Quaternion look;
    RaycastHit hit;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(Physics.Raycast(frontcam.position, frontcam.forward,out hit, Mathf.Infinity,1<<LayerMask.NameToLayer("win")))
            transform.position = hit.point;
        transform.rotation = Quaternion.Euler(90, 0, 360-frontcam.rotation.eulerAngles.y);
        transform.localScale = Vector3.one*(frontcam.position -transform.position).magnitude;
	}
}
