using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currency : MonoBehaviour {
    public bool InheritVelocity = false;
    float followSpeed = 1f;
    Rigidbody rig;
    public float CurrencyValue;
    public bool Placed = false;
    public bool hasOwner = false;
    public Transform myListObj;
    public Transform tar;


    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
        CurrencyHandler ch = FindObjectOfType<CurrencyHandler>();
        ch.AddCurrency(GameObject.Find("ColumnSection").transform, transform);
    }
	
	// Update is called once per frame
	void Update () {
		if(!Placed && transform.parent != null)
        {
            if(tar!=null)
                transform.position = Vector3.Slerp(transform.position, tar.position - Vector3.forward * 0.01f, followSpeed * Time.deltaTime);
            else if(transform.parent.tag == "ColumnSection")
            {
            }
        }
	}
    private void FixedUpdate()
    {
        if (!InheritVelocity)
            rig.velocity = Vector3.zero;
    }
    


}
