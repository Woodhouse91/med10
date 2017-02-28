using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmusicbox2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            AkSoundEngine.PostEvent("playSoundOnHeadphones", gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        AkSoundEngine.AddSecondaryOutput((uint)AkAudioOutputType.AkOutput_NumBuiltInOutputs /*Player ID (first player)*/, AkAudioOutputType.AkOutput_Main, 1 /*Listener 1*/);

        }
    }
}
