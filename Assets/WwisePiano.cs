using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwisePiano : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.OnExcelDataLoaded += StartPiano;
	}

    private void StartPiano()
    {
        print("play");
        AkSoundEngine.PostEvent("Ambient", gameObject);
    }


    private void Unsub()
    {
        EventManager.OnExcelDataLoaded -= StartPiano;
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

    // Update is called once per frame
    void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 1f)
        {
            print("piano hit with velocity of: "+collision.relativeVelocity.magnitude);
            AkSoundEngine.SetRTPCValue("PianoHitVelocity", collision.relativeVelocity.magnitude);
            AkSoundEngine.PostEvent("HitPiano", gameObject);
        }
    }
}
