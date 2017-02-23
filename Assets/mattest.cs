using UnityEngine;
using System.Collections;

public class mattest : MonoBehaviour {
    private Texture2D myTex;
    [SerializeField]
    private bool UnDistorted = false;

	// Use this for initialization
	void Start () {

        //myTex = SteamVR_TrackedCamera.Source(false).texture;
        //myTex.wrapMode = TextureWrapMode.Clamp;
	
	}
	
	// Update is called once per frame
	void Update () {
        myTex = SteamVR_TrackedCamera.Source(UnDistorted).texture;
        myTex.wrapMode = TextureWrapMode.Clamp;
        GetComponent<Projector>().material.SetTexture("_ShadowTex",myTex);
        
    }
}
