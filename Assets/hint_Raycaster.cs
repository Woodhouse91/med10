using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hint_Raycaster : MonoBehaviour {
    RaycastHit hit;
    private Transform myhit;
    private int hintLayer = 1;
	// Use this for initialization
	void Start () {
        hintLayer = 1 << LayerMask.NameToLayer("hintlayer");
	}
	
	// Update is called once per frame
	void Update () {
		if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, hintLayer))
        {
            if (myhit == null || hit.transform!=myhit){
                if (myhit != null)
                    myhit.GetComponent<hint_ActivateWorldHint>().Activate(false);
                myhit = hit.transform;
                myhit.GetComponent<hint_ActivateWorldHint>().Activate(true);
            }
        }
        else if(myhit!=null)
        {
            myhit.GetComponent<hint_ActivateWorldHint>().Activate(false);
            myhit = null;
        }

	}
}
