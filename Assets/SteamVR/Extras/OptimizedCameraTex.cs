//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using System;
public class OptimizedCameraTex : MonoBehaviour
{
	public Material material;
	public Transform target;
	public bool undistorted = true;
	public bool cropped = true;
    private const float aspect = 1.330435f, du = 0.5f, dv = -0.4999999f;
    private Vector2 texOffSet = new Vector2(0.25f, 0.75f), texScale = new Vector2(du, dv);
    private Vector3 scale = new Vector3(1.0f, 1.0f / aspect, 1);
    Texture2D texture;
    SteamVR_TrackedCamera.VideoStreamTexture source;
    void OnEnable()
	{
		source = SteamVR_TrackedCamera.Source(undistorted);
		source.Acquire();

		if (!source.hasCamera)
			enabled = false;
	}

	void OnDisable()
	{
		material.mainTexture = null;
		source = SteamVR_TrackedCamera.Source(undistorted);
		source.Release();
	}

	void Update()
	{
		source = SteamVR_TrackedCamera.Source(undistorted);
        texture = source.texture;
		if (texture == null)
		{
			return;
		}
		material.mainTexture = texture;
        material.mainTextureOffset = texOffSet;
        material.mainTextureScale = texScale;
		if (source.hasTracking)
		{
			var t = source.transform;
			target.localPosition = t.pos;
			target.localRotation = t.rot;
		}
    }
}

