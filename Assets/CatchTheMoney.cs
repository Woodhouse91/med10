using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTheMoney : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public float raiseEachTime = 0.001f;
    public float LayDownSpeed;
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ignoreCur") && !other.GetComponent<Rigidbody>().isKinematic)
        {
            StartCoroutine(LayDown(other.transform));
            transform.position += transform.up* raiseEachTime;
            //other.gameObject.layer = LayerMask.NameToLayer("Currency");
        }
    }
    IEnumerator LayDown(Transform other)
    {
        other.GetComponent<Rigidbody>().isKinematic = true;
        float dist = Vector3.Distance(other.position, other.transform.position - transform.up * (other.position.y - transform.position.y));
        Vector3 startPos = other.transform.position;
        Vector3 endPos = other.transform.position - transform.up * (other.position.y - transform.position.y);
        Quaternion startRot = other.rotation;
        float roty = startRot.eulerAngles.y;
        float rotz = startRot.eulerAngles.z;
        if (rotz < 180f)
            rotz = 0f;
        else
            rotz = 180f;
        Quaternion endRot;
        if (other.tag == "50kr"|| other.tag == "100kr"|| other.tag == "200kr"|| other.tag == "500kr"|| other.tag == "1000kr")
            endRot = Quaternion.identity * Quaternion.AngleAxis(90f,Vector3.right) * Quaternion.AngleAxis(roty, Vector3.forward) * Quaternion.AngleAxis(rotz, Vector3.up);
        else
            endRot = Quaternion.identity * Quaternion.AngleAxis(startRot.y, Vector3.up);
        float t= 0;
        while (t<1)
        {
            t += Time.deltaTime / (dist*LayDownSpeed);
            other.position = Vector3.Lerp(startPos, endPos, t);
            other.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }
        Destroy(other.GetComponent<Rigidbody>());
        yield return null;
    }
}
