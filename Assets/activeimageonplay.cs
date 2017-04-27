using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class activeimageonplay : MonoBehaviour {
#if (!UNITY_EDITOR)
    // Use this for initialization
    void Start () {
        GetComponent<Image>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
#endif
}
