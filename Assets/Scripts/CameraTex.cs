//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;

public class CameraTex : MonoBehaviour
{
    public Material material;
    public Transform target;
    public bool undistorted = true;
    public bool cropped = true;

    void OnEnable()
    {

        var source = SteamVR_TrackedCamera.Source(undistorted);
        source.Acquire();

 
        if (!source.hasCamera)
            enabled = false;
    }

    void OnDisable()
    {
        material.mainTexture = null;
        var source = SteamVR_TrackedCamera.Source(undistorted);
        source.Release();
    }

    void Update()
    {
        var source = SteamVR_TrackedCamera.Source(undistorted);
        var texture = source.texture;
        if (texture == null)
        {
            return;
        }
        material.mainTexture = texture;
        var aspect = (float)texture.width / texture.height;
        var bounds = source.frameBounds;
        material.mainTextureOffset = new Vector2(bounds.uMin, bounds.vMin);

        var du = bounds.uMax - bounds.uMin;
        var dv = bounds.vMax - bounds.vMin;
        material.mainTextureScale = new Vector2(du, dv);

        aspect *= Mathf.Abs(du / dv);
        
       // target.localScale = new Vector3(1, 1.0f / aspect, 1);
        if (source.hasTracking)
        {
            var t = source.transform;
            target.localPosition = t.pos;
            target.localRotation = t.rot;
        }
        print("aspect "+aspect);
        print("dv " + dv);
        print("du " + du);
        print("umin " + bounds.uMin);
        print("umax " + bounds.vMin);
    }
}