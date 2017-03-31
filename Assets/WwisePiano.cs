using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwisePiano : MonoBehaviour {

	
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 1f)
        {
            AkSoundEngine.SetRTPCValue("PianoHitVelocity", collision.relativeVelocity.magnitude);
            AkSoundEngine.PostEvent("HitPiano", gameObject);
        }
    }
}
