using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currency : MonoBehaviour {
    public bool InheritVelocity = false;
    float followSpeed = 0.50000f;
    Rigidbody rig;
    public float CurrencyValue;
    public bool Placed = false;
    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		if(!Placed && transform.parent != null)
        {
            if(transform.parent.tag == "Marker")
                transform.position = Vector3.Slerp(transform.position, transform.parent.position - Vector3.forward * 0.01f, followSpeed * Time.deltaTime);
            else if(transform.parent.tag == "ColumnSection")
            {
                // Troede jeg skulle skrive mere her, men nej systemet er for godt til det
                //Placed = true;
            }
        }
	}
    private void FixedUpdate()
    {
        if (!InheritVelocity)
            rig.velocity = Vector3.zero;
    }
    


}
