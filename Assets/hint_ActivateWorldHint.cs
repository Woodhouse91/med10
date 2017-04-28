using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hint_ActivateWorldHint : MonoBehaviour {
    [SerializeField]
    private Transform myHint, myHintPosActive, myHintPosDeactive;

    private bool activating = false;
    private float activateTimer = 1f;
	// Use this for initialization
	void Start () {
        StartCoroutine(deactivate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate(bool b)
    {
        print("activating");
        if (b && !activating)
            StartCoroutine(activate());
        if (!b && activating)
            StartCoroutine(deactivate());
    }
    private IEnumerator deactivate()
    {
        activating = false;
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = myHintPosDeactive.position;
        Quaternion tarRot = myHintPosDeactive.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.zero;
        float t = 0;
        while (!activating && t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
        }
        yield break;
    }
    private IEnumerator activate()
    {
        activating = true;
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = myHintPosActive.position;
        Quaternion tarRot = myHintPosActive.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.one;
        float t = 0;
        while(activating && t < 1)
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
