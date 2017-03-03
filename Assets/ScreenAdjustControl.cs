using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjustControl : MonoBehaviour {
    [SerializeField]
    private Transform tar1, tar2;
    [SerializeField]
    private float moveDist = 0.001f, rotStrength = 0.001f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        #region Positions
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tar1.transform.position += Vector3.right * moveDist;
            tar2.transform.position += Vector3.right * moveDist;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tar1.transform.position -= Vector3.right * moveDist;
            tar2.transform.position -= Vector3.right * moveDist;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tar1.transform.position += Vector3.up * moveDist;
            tar2.transform.position += Vector3.up * moveDist;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tar1.transform.position -= Vector3.up * moveDist;
            tar2.transform.position -= Vector3.up * moveDist;
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            tar1.transform.position += Vector3.forward * moveDist;
            tar2.transform.position += Vector3.forward * moveDist;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            tar1.transform.position -= Vector3.forward * moveDist;
            tar2.transform.position -= Vector3.forward * moveDist;
        }
        #endregion
        #region Rotations
        if (Input.GetKeyDown(KeyCode.Q))
        {
            tar1.transform.Rotate(Vector3.forward * rotStrength);
            tar2.transform.Rotate(Vector3.forward * rotStrength);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            tar1.transform.Rotate(-Vector3.forward * rotStrength);
            tar2.transform.Rotate(-Vector3.forward * rotStrength);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            tar1.transform.Rotate(Vector3.up * rotStrength);
            tar2.transform.Rotate(Vector3.up* rotStrength);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            tar1.transform.Rotate(-Vector3.up * rotStrength);
            tar2.transform.Rotate(-Vector3.up * rotStrength);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            tar1.transform.Rotate(Vector3.right * rotStrength);
            tar2.transform.Rotate(Vector3.right * rotStrength);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            tar1.transform.Rotate(-Vector3.right * rotStrength);
            tar2.transform.Rotate(-Vector3.right * rotStrength);
        }
        #endregion
    }
}
