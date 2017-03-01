using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currency : MonoBehaviour {
    public bool InheritVelocity = false;
    Rigidbody rig;
    public float CurrencyValue;
    public bool Placed = false;
    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        if (!InheritVelocity)
            rig.velocity = Vector3.zero;
    }
    
}
