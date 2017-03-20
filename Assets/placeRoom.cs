using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeRoom : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    // Use this for initialization
    private void Awake () {
        EventManager.OnUIPlaced += setRoomPos;
	}

    private void setRoomPos()
    {
        transform.rotation = new Quaternion(0, target.rotation.y, 0, target.rotation.w);
        transform.position = target.position+transform.up*offset.y+transform.right*offset.x+transform.forward*offset.z;
    }

    // Update is called once per frame
    void Update () {
		
	}
    void Unsub()
    {
        EventManager.OnUIPlaced -= setRoomPos;
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }
}
