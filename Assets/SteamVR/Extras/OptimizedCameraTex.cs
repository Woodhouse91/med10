using UnityEngine;
using System;
using System.Collections;

public class OptimizedCameraTex : MonoBehaviour
{
	private Material material;
    public Material[] settingMat;
	public Transform target;
    public GameObject[] CameraSurface, Projector;
    private const float aspect = 1.330435f, du = 0.5f, dv = -0.4999999f;
    private Vector2 texOffSet = new Vector2(0.25f, 0.75f), texScale = new Vector2(du, dv);
    private Texture2D texture;
    private bool distorted;
    private int curTex = 1;
    private struct camData
    {
        public float dv, du, texOffsetX, texOffsetY;
    }
    private camData[] camSetting;
    SteamVR_TrackedCamera.VideoStreamTexture source;
    public void changeCamera()
    {
        CameraSurface[curTex].SetActive(false);
        Projector[curTex].SetActive(false);
        if(material != null)
            material.mainTexture = null;
        curTex = curTex + 1 > 1 ? 0 : 1;
        distorted = curTex == 1;
        CameraSurface[curTex].SetActive(true);
        Projector[curTex].SetActive(true);
        material = settingMat[curTex];
        texOffSet = new Vector2(camSetting[curTex].texOffsetX, camSetting[curTex].texOffsetY);
        texScale = new Vector2(camSetting[curTex].du, camSetting[curTex].dv);

    }
    void Start()
	{
        if(camSetting==null)
        {
            camSetting = new camData[2];
            camSetting[1].dv = -0.4999999f;
            camSetting[1].du = 0.5f;
            camSetting[1].texOffsetX = 0.25f;
            camSetting[1].texOffsetY = 0.75f;
            camSetting[0].dv = -1f;
            camSetting[0].du = 1f;
            camSetting[0].texOffsetX = 0f;
            camSetting[0].texOffsetY = 1f;
        }
        source = SteamVR_TrackedCamera.Source(distorted);
		source.Acquire();
        changeCamera();
		if (!source.hasCamera)
			enabled = false;
	}

	private void OnDisable()
	{
		material.mainTexture = null;
        source = SteamVR_TrackedCamera.Source(distorted);
        source.Release();
	}
    private void OnApplicationQuit()
    {
        material.mainTexture = null;
        source = SteamVR_TrackedCamera.Source(distorted);
        source.Release();
    }

    void Update()
	{
        source = SteamVR_TrackedCamera.Source(distorted);
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

