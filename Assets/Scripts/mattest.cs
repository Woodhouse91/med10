using UnityEngine;
using System.Collections;
using System;

public class mattest : MonoBehaviour {
    private Texture2D texture;
    [SerializeField]
    private bool undistorted = false;
    private const float aspect = -1.330435f;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Transform target;

    private Single du, dv;
    private Valve.VR.VRTextureBounds_t bounds;
    private Vector2 texOffSet, texScale;
    // Use this for initialization
    void OnEnable()
    {
        var source = SteamVR_TrackedCamera.Source(undistorted);
        source.Acquire();

        // Auto-disable if no camera is present.
        if (!source.hasCamera)
            enabled = false;
        bounds = source.frameBounds;
        du = bounds.uMax - bounds.uMin;
        dv = bounds.vMax - bounds.vMin;
        texOffSet = new Vector2(bounds.uMin, bounds.vMin);
        texScale = new Vector2(du, dv);
        target.localScale = new Vector3(1, 1.0f / aspect, 1);
    }

    // Update is called once per frame
    void Update () {
        var source = SteamVR_TrackedCamera.Source(undistorted);
        material.mainTexture = source.texture;
        material.mainTextureOffset = texOffSet;
        material.mainTextureScale = texScale;
       

        // Apply the pose that this frame was recorded at.
        if (source.hasTracking)
        {
            var t = source.transform;
            target.localPosition = t.pos;
            target.localRotation = t.rot;
        }
        material.mainTexture.wrapMode = TextureWrapMode.Clamp;
        GetComponent<Projector>().material.SetTexture("_ShadowTex", material.mainTexture);
        
    }
    void OnDisable()
    {
        var source = SteamVR_TrackedCamera.Source(undistorted);
        source.Release();
    }
}
