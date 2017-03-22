using UnityEngine;
using System;
public class OptimizedCameraTex : MonoBehaviour
{
	public Material material;
	public Transform target;
    private const float aspect = 1.330435f, du = 0.5f, dv = -0.4999999f;
    private Vector2 texOffSet = new Vector2(0.25f, 0.75f), texScale = new Vector2(du, dv);
    private Vector3 scale = new Vector3(1.0f, 1.0f / aspect, 1);
    Texture2D texture;
    SteamVR_TrackedCamera.VideoStreamTexture source;
    void OnEnable()
	{
        source = SteamVR_TrackedCamera.Undistorted();
		source.Acquire();

		if (!source.hasCamera)
			enabled = false;
	}

	private void OnDisable()
	{
		material.mainTexture = null;
        source = SteamVR_TrackedCamera.Undistorted();
        source.Release();
	}
    private void OnApplicationQuit()
    {
        material.mainTexture = null;
        source = SteamVR_TrackedCamera.Undistorted();
        source.Release();
    }

    void Update()
	{
        source = SteamVR_TrackedCamera.Undistorted();
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

