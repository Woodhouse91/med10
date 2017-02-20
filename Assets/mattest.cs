using UnityEngine;
using System.Collections;

public class mattest : MonoBehaviour {
    Texture2D myTex;
    public bool UnDistorted = false;
    [SerializeField]
    private float Cutoff;
    [SerializeField]
    Color handCol, cutCol;

	// Use this for initialization
	void Start () {

        //myTex = SteamVR_TrackedCamera.Source(false).texture;
        //myTex.wrapMode = TextureWrapMode.Clamp;
	
	}
	
	// Update is called once per frame
	void Update () {
        myTex = SteamVR_TrackedCamera.Source(UnDistorted).texture;
        myTex.wrapMode = TextureWrapMode.Clamp;
        //for(int y = 0; y<myTex.height; ++y)
        //{
        //    for(int x = 0; x<myTex.width; ++x)
        //    {
        //        if (myTex.GetPixel(x, y).b < Cutoff)
        //            myTex.SetPixel(x, y, cutCol);
        //        else
        //            myTex.SetPixel(x, y, handCol);
        //    }
        //}
        GetComponent<Projector>().material.SetTexture("_ShadowTex",myTex);
        
    }
}
