using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSpecialDestroy : MonoBehaviour {
    private enum DestroyBehaviour { Fade}
    [SerializeField]
    private DestroyBehaviour DestroySetting;
    SlideAbleObject so;
    private float fadeTime = 1.5f;

    // Use this for initialization
    void Start () {
        so = GetComponent<SlideAbleObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void invoke()
    {
        switch (DestroySetting)
        {
            case DestroyBehaviour.Fade:
                StartCoroutine(FadeOut());
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        float alpha;
        Color orgS = so.slider.GetComponent<SpriteRenderer>().color, orgA = so.area.GetComponent<SpriteRenderer>().color, orgT = so.target.GetComponent<SpriteRenderer>().color;
        while (t <= fadeTime)
        {
            alpha = 1f - t / fadeTime;
            so.slider.GetComponent<SpriteRenderer>().color = orgS * alpha;
            so.area.GetComponent<SpriteRenderer>().color = orgA * alpha;
            so.target.GetComponent<SpriteRenderer>().color = orgT * alpha;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }
}
