//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using System;
public class SteamVR_TestTrackedCamera : MonoBehaviour
{
	public Material material;
	public Transform target;
	public bool undistorted = true;
	public bool cropped = true;
    private const float aspect = 1.33043478261f, du = 0.5f, dv = -0.5f;
    private Vector2 texOffSet = new Vector2(0.3f, 0.8f), texScale = new Vector2(du, dv);
    private Vector3 scale = new Vector3(1.0f, 1.0f / aspect, 1);

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
        material.mainTextureOffset = texOffSet;
        material.mainTextureScale = texScale;
        target.localScale = scale;
		if (source.hasTracking)
		{
			var t = source.transform;
			target.localPosition = t.pos;
			target.localRotation = t.rot;
		}
    }
}

