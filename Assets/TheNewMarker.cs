using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheNewMarker : MonoBehaviour {

    private Transform TargetColumn, prevMarked, curTar, prevTar;
    private CurrencyHandler CH;
    private Ray ray;
    private RaycastHit hit;
    private int dropLayer, currencyLayer, columnLayer;
    private bool dropCur, firstFrame = true, oppositeDrag;
    private Vector3 prevPos, dragDir, orgDir, orgDrop, curDir, prevDir;
    private float dropAmount;

    private void Awake()
    {
        columnLayer = 1 << LayerMask.NameToLayer("Column");
        currencyLayer = 1 << LayerMask.NameToLayer("Currency");
        dropLayer = 1 << LayerMask.NameToLayer("DropSection");
        CH = FindObjectOfType<CurrencyHandler>();
    }

    private void FixedUpdate()
    {
        ray.origin = transform.position - transform.forward * 0.5f;
        ray.direction = transform.forward;
        dragDir = transform.position - prevPos;
        if (Physics.Raycast(ray, out hit, 1, columnLayer))
            TargetColumn = hit.transform;

        if (Physics.Raycast(ray, out hit, 1, currencyLayer))
        {
            prevTar = curTar;
            curTar = hit.transform;
            prevDir = curDir.normalized;
            curDir = dragDir.normalized;
            if(Vector3.Dot(orgDir, curDir) < -.5f || !oppositeDrag)
            {
                if (prevTar.GetComponent<currency>().isMarked)
                    CH.MarkCurrency(prevTar, transform);
            }
            else if (prevTar != curTar)
            {
                CH.MarkCurrency(curTar, transform);
                orgDir = dragDir.normalized;
            }
        }

        if (Physics.Raycast(ray, out hit, 1, dropLayer))
        {
            if (!dropCur)
            {
                orgDrop = hit.point;
                dropCur = true;
            }
            else
                dropAmount = orgDrop.y - hit.point.y;
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
        if (TargetColumn == null)
        {
            gameObject.SetActive(false);
            return; // HER SKAL DEN I MONEYBALL
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
