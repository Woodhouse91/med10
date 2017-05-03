using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RipTapeSlider : MonoBehaviour {

    Transform tSlide, tTarget;
    Vector3 startPos, endPos;
    public float Speed = 2;
    public bool Returning;
    BoxInterfaceScreen bis;
    float startDist;
	// Use this for initialization
	void Awake () {
        EventManager.OnRipTapeSliderDone += RipTapeSliderDone;
        tSlide = transform.GetChild(0);
        tTarget = transform.GetChild(1);
        startPos = tSlide.localPosition;
        endPos = tTarget.localPosition;
        startDist = Vector3.Distance(startPos, endPos);
    }
    private void Unsub()
    {
        EventManager.OnRipTapeSliderDone -= RipTapeSliderDone;
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }

    private void RipTapeSliderDone()
    {
        gameObject.SetActive(false);
        tSlide.localPosition = startPos;
        GetComponentInChildren<Collider>().enabled = true;
        tSlide.GetComponent<SpriteRenderer>().enabled = true;
        tSlide.GetComponent<Image>().enabled = true;
        Returning = false;
    }

    public float Dist(BoxInterfaceScreen bis)
    {
        if(this.bis != bis)
            this.bis = bis;
        float dist = Vector3.Distance(tSlide.localPosition, endPos);
        dist /= startDist;
        if (tSlide.localPosition.x > endPos.x)
            return 1.0f;
        return 1.0f-dist;
    }
    public void ReturnToStart()
    {
        Returning = true;
        StartCoroutine(ReturnToStartAni());
    }
    IEnumerator ReturnToStartAni()
    {
        bool Ripped = false;
        if (1.0f - (Vector3.Distance(tSlide.localPosition, endPos) / startDist) > 0.95f)
        {
            tSlide.GetComponent<SpriteRenderer>().enabled = false;
            tSlide.GetComponent<Image>().enabled = false;
            Ripped = true;
        }
        while (tSlide.localPosition != startPos)
        {
            tSlide.localPosition = Vector3.MoveTowards(tSlide.localPosition, startPos, Time.deltaTime * Speed);
            if(!Ripped)
                bis.TapeRipSlide(Dist(bis));
            yield return null;
        }
        GetComponentInChildren<Collider>().enabled = true;
        Returning = false;
        yield return null;
    }

    private void LateUpdate()
    {
        //FRAME LORTET
        if (tSlide.localPosition.x < startPos.x)
            tSlide.localPosition = startPos;
        if (tSlide.localPosition.x > endPos.x)
            tSlide.localPosition = endPos;
    }
}
