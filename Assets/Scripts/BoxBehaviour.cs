using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {

    RipTheTape tape;
    Transform leftLid, rightLid;
    Rigidbody rig;
    public Vector3 FORCEIT;
    public float SpawnRate;
    private Transform spawnArea;
    private float Ax,Ay,Az;

    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
        leftLid = transform.GetChild(0);
        rightLid = transform.GetChild(1);
        tape = GetComponentInChildren<RipTheTape>();
        spawnArea = transform.GetChild(2); //the lids are 0 and 1
        Ax = spawnArea.lossyScale.x / 2f;
        Ay = spawnArea.lossyScale.y / 2f;
        Az = spawnArea.lossyScale.z / 2f;
    }


    // Update is called once per frame
    public void tapeRipped()
    {
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
        Vector3 endPos;
        if (GetComponent<AddMoneyToTable>() != null)
        {
            endPos = EventManager.Table.position + EventManager.Table.up * 2f + EventManager.Table.right/2f;
        }
        else
        {
            endPos = EventManager.Table.position + EventManager.Table.up * 2f - EventManager.Table.right/2f;
        }
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
        if(GetComponent<AddMoneyToTable>() != null)
            GetComponent<AddMoneyToTable>().ShowMeTheMoney(); // SPAWN ALL THE MONIES PLZ
        else // SPAWN ALL THE MODELS
        {
            int categoryInt = DataHandler.expenseData[0, EventManager.CurrentCategory];
            print("CategoryINT: " + categoryInt);
            int numOfObj = 0;
            for (int i = 1; i < 13; i++)
            {
                if (DataHandler.expenseData[i, EventManager.CurrentCategory] > 0)
                    numOfObj++;
            }
            for (int i = 0; i < numOfObj; i++)
            {
                Transform model = CategoryModelHandler.GetAt(categoryInt).transform; // MODELS HERE THO
                Vector3 pos = new Vector3(Random.Range(-Ax, Ax), Random.Range(-Ay, Ay), Random.Range(-Az, Az));
                pos = transform.TransformPoint(pos);
                Instantiate(model,pos, Quaternion.AngleAxis(Random.Range(1, 360), Vector3.right) * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.up) * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.forward));
                yield return new WaitForSeconds(1f/SpawnRate);
            }
            yield return new WaitForSeconds(0.5f);
            EventManager.BoxEmptied();
            Throw();
        }
        yield break;
    }
    public void Throw()
    {
        rig.isKinematic = false;
        Vector3 forceDir = EventManager.Table.right * FORCEIT.x + EventManager.Table.forward * FORCEIT.z + EventManager.Table.up * FORCEIT.y;
        rig.AddForce(forceDir);
        rig.AddTorque(forceDir);
    }
}