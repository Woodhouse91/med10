﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTo : MonoBehaviour {
    Transform Parent;
    Vector3 posOffset;
    Vector3 rotOffset;
    public void Initiate(Transform parent)
    {
        Parent = parent;
        if(GetComponent<Rigidbody>()!=null)
            Destroy(GetComponent<Rigidbody>());
       //if (GetComponent<Collider>() != null)
       //    Destroy(GetComponent<Collider>());
        if (GetComponentInChildren<Rigidbody>() != null)
            Destroy(GetComponentInChildren<Rigidbody>());
       //if (GetComponentInChildren<Collider>() != null)
       //    Destroy(GetComponentInChildren<Collider>());
        posOffset = transform.position - parent.position;
    }
    void Update () {
        if(Parent != null)
        {
            transform.position = Parent.position + posOffset;
        }
	}
}
