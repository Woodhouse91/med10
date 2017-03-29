using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwisePiano : MonoBehaviour {

	
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
