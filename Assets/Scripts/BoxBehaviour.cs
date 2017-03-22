using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {

    RipTheTape tape;
    Transform leftLid, rightLid;
    Rigidbody rig;
    public Vector3 FORCEIT;
	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
        leftLid = transform.GetChild(0);
        rightLid = transform.GetChild(1);
        tape = GetComponentInChildren<RipTheTape>();
	}
	// Update is called once per frame
    public void tapeRipped()
    {
        EventManager.RipTapeSliderDone();
        StartCoroutine(FlipUp());
    }
    public void setTapeRip(float dist)
    {
        tape.SetTapeDist(dist);
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Currency"))
    //    {
    //        StartCoroutine(LayerChange(other.transform, true, 0f));
    //    }

    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("ignoreCur"))
    //    {
    //        StartCoroutine(LayerChange(other.transform, false, Random.Range(1f, 2f)));
    //        StartCoroutine(makeKinematic(Random.Range(2f, 3f), other.transform));
    //    }
    //}

    IEnumerator makeKinematic(float t, Transform obj)
    {
        yield return new WaitForSeconds(t);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        yield break;
    }
    IEnumerator LayerChange(Transform other, bool layer, float t)
    {
        yield return new WaitForSeconds(t);
        if(layer)
            other.gameObject.layer = LayerMask.NameToLayer("ignoreCur");
        else
            other.gameObject.layer = LayerMask.NameToLayer("Currency");
        yield return null;
    }



    IEnumerator FlipUp()
    {
        rig.isKinematic = true;
        //first we raise it.
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up; // RAISE ONE METER
        while (t<1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime;
            yield return null;
        }
        //then we flip it.
        t = 0;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transform.rotation * Quaternion.AngleAxis(90f, Vector3.up) * Quaternion.AngleAxis(170f, Vector3.forward);  // ROTATE 180
        while (t < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
        //then we open the lids
        Quaternion startRotL = leftLid.localRotation;
        Quaternion startRotR = rightLid.localRotation;
        Quaternion endRotL = leftLid.localRotation * Quaternion.AngleAxis(-135f, Vector3.forward); // ROTATE LIDS 135
        Quaternion endRotR = rightLid.localRotation * Quaternion.AngleAxis(135f, Vector3.forward);
        t = 0;
        while (t < 1)
        {
            leftLid.localRotation = Quaternion.Slerp(startRotL, endRotL, t);
            rightLid.localRotation = Quaternion.Slerp(startRotR, endRotR, t);
            t += Time.deltaTime;
            yield return null;
        }
        GetComponent<AddMoneyToTable>().ShowMeTheMoney(); // SPAWN ALL THE MONIES PLZ

        yield break;
    }
    public void Throw()
    {
        EventManager.BoxThrow();
        rig.isKinematic = false;
        rig.AddForce(FORCEIT);
        rig.AddTorque(FORCEIT);
    }
}