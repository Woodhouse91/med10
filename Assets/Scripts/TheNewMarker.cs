using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheNewMarker : MonoBehaviour {

    private Transform TargetColumn, prevMarked, curTar, prevTar, freeCur;
    private CurrencyHandler CH;
    private Ray ray;
    private RaycastHit hit;
    private int dropLayer, currencyLayer, columnLayer, touchScreenLayer, dragLayer;
    private bool dropCur, firstFrame = true, oppositeDrag, dropAll;
    private Vector3 prevPos, dragDir, orgDrop;
    private float dropAmount;

    private void Awake()
    {
        columnLayer = 1 << LayerMask.NameToLayer("Column");
        currencyLayer = 1 << LayerMask.NameToLayer("Currency");
        dropLayer = 1 << LayerMask.NameToLayer("DropSection");
        touchScreenLayer = 1 << LayerMask.NameToLayer("TouchScreen");
        dragLayer = 1 << LayerMask.NameToLayer("DragLayer");
        CH = FindObjectOfType<CurrencyHandler>();
    }

    private void FixedUpdate()
    {
        ray.origin = transform.position - transform.forward * 0.5f;
        ray.direction = transform.forward;
        dragDir = transform.position - prevPos;
        if(Physics.Raycast(ray, out hit, touchScreenLayer))
        {

        }
        if (Physics.Raycast(ray, out hit, 1, dragLayer))
        {

        }
        if (Physics.Raycast(ray, out hit, 1, columnLayer))
        TargetColumn = hit.transform;

        //#region Safety Raycast
        //if(Physics.Raycast(transform.position-dragDir*(dragDir.magnitude/2.0f), transform.forward, out hit, 1, currencyLayer))
        //{
        //    prevTar = curTar;
        //    curTar = hit.transform;
        //    prevDir = curDir.normalized;
        //    curDir = dragDir.normalized;
        //    if (prevTar != curTar)
        //    {
        //        if (prevTar != null)
        //        {
        //            if (prevTar.GetComponent<currency>().isMarked && curTar.GetComponent<currency>().isMarked)
        //            {
        //                CH.MarkCurrency(prevTar, transform);
        //                oppositeDrag = true;
        //            }
        //            else
        //            {
        //                oppositeDrag = false;
        //            }
        //        }
        //        if (!oppositeDrag)
        //            CH.MarkCurrency(curTar, transform);
        //    }
        //}
        //else
        //{
        //    if (oppositeDrag && curTar.GetComponent<currency>().isMarked)
        //        CH.MarkCurrency(curTar, transform);
        //    oppositeDrag = false;
        //    curTar = null;
        //    prevTar = null;
        //}
        //#endregion

        if (Physics.Raycast(ray, out hit, 1, currencyLayer))
        {
            prevTar = curTar;
            curTar = hit.transform;
            if (prevTar != curTar)
            {
                if (prevTar != null)
                {
                    if (prevTar.GetComponent<currency>().isMarked && curTar.GetComponent<currency>().isMarked)
                    {
                        CH.MarkCurrency(prevTar, transform);
                        oppositeDrag = true;
                    }
                    else
                    {
                        oppositeDrag = false;
                    }
                }
                if (!oppositeDrag)
                    CH.MarkCurrency(curTar, transform);
            }
        }
        else
        {
            if (oppositeDrag && curTar.GetComponent<currency>().isMarked)
                CH.MarkCurrency(curTar, transform);
            oppositeDrag = false;
            curTar = null;
            prevTar = null;
        }
        if (Physics.Raycast(ray, out hit, 1, dropLayer))
        {
            if (!dropCur || Vector3.Distance(hit.point, orgDrop + transform.up) < Vector3.Distance(hit.point, orgDrop - transform.up))
            {
                orgDrop = hit.point;
                dropCur = true;
                if (firstFrame)
                    dropAll = true;
               // print("NEW POINT");
            }
            else
            {
                Vector3 hyp = hit.point - orgDrop;
                float angle = Vector3.Angle(hyp, transform.right);
                angle = angle > 90f ? 180f - angle : angle;
                float theta = Mathf.Sin(angle*Mathf.Deg2Rad) * Mathf.Rad2Deg;
                dropAmount = theta * hyp.magnitude;
            }
        }
        else
        {
            dropCur = false;
        }
        prevPos = transform.position;
        firstFrame = false;
    }
    public void MyDisable()
    {
        firstFrame = true;
        if (dropAll)
        {
            CH.TransferCurrency(TargetColumn, freeCur);
        }
        CH.TransferCurrency(TargetColumn, transform);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        // keyboard controls
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * 0.15f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.15f);

        if (Input.GetKeyDown(KeyCode.Space))
            MyDisable();
    }

}
