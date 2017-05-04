using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hint_ActivateWorldHint : MonoBehaviour {
    [SerializeField]
    private Transform myHint, myHintPosActive, myHintPosDeactive;
    private bool firstActivate = true;
    private float hintTimerStay = 10f;
    private float hintTimerReset = 10f;
    private float activateTimer = .33f;
    private float timer = 0;
    public bool LuxusHint = false;
    bool isActive = false;
    bool prevState = false;
	// Use this for initialization
	void Start () {
        myHint.gameObject.SetActive(true);
        EventManager.OnExcelDataLoaded += loadedDeactivate;
	}

    private void loadedDeactivate()
    {
        firstActivate = true;
        StartCoroutine(deactivate());
    }
    private void Update()
    {
        if (prevState != isActive)
        {
            if (isActive)
                StartCoroutine(activate());
            else
                StartCoroutine(deactivate());
            prevState = isActive;
        }
    }

    private IEnumerator waitForReset()
    {
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = myHintPosDeactive.position;
        Quaternion tarRot = myHintPosDeactive.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.zero;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
        }
        while (isActive)
            yield return null;
        timer = 0;
    }

    public void Activate(bool b)
    {
        if (LuxusHint && !LuxusSegmentHandler.activeTable)
            return;
        isActive = b;
    }
    private IEnumerator deactivate()
    {
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = myHintPosDeactive.position;
        Quaternion tarRot = myHintPosDeactive.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.zero;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
            if (isActive)
                yield break;
        }
        yield break;
    }
    private IEnumerator activate()
    {
        float t = 0;
        if (!firstActivate)
        {
            while (t < hintTimerReset)
            {
                t += Time.deltaTime;
                if (!isActive)
                    yield break;
                yield return null;
            }
        }
        firstActivate = false;
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = myHintPosActive.position;
        Quaternion tarRot = myHintPosActive.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.one;
        t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
            if (!isActive)
                yield break;
        }
        t = 0;
        while (t < hintTimerStay)
        {
            t += Time.deltaTime;
            yield return null;
            if (!isActive)
                yield break;
        }
        t = 0;
        orgPos = myHint.position;
        orgRot = myHint.rotation;
        tarPos = myHintPosDeactive.position;
        tarRot = myHintPosDeactive.rotation;
        orgScale = myHint.localScale;
        tarScale = Vector3.zero;
        while (t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
        }
        yield break;
    }
}
