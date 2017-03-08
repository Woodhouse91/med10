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
    CurrencyHandler ch;
    public Transform myListObj;
    public Transform tar;
    public bool canPickUp
    {
        get
        {
            return tar == null;
        }
    }

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        ch = FindObjectOfType<CurrencyHandler>();
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
        if(Input.GetKeyDown(KeyCode.Y))
            ch.AddCurrency(GameObject.FindGameObjectWithTag("ColumnSection").transform, transform);
    }
    private void FixedUpdate()
    {
        if (!InheritVelocity)
            rig.velocity = Vector3.zero;
    }
    


}
