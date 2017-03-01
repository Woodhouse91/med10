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
    private Vector2 texOffSet = new Vector2(0.25f, 0.75f), texScale = new Vector2(du, dv), _texAdjust = Vector2.zero;
    private Vector3 scale = new Vector3(1.0f, 1.0f / aspect, 1);
    public Vector2 texAdjust
    {
        set
        {
            _texAdjust = value;
        }
    }

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
        material.mainTextureOffset = texOffSet + _texAdjust;
        material.mainTextureScale = texScale;
		if (source.hasTracking)
		{
			var t = source.transform;
			target.localPosition = t.pos;
			target.localRotation = t.rot;
		}
    }
}

