using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheNewestMarker : MonoBehaviour
{

    private Ray ray;
    private RaycastHit hit;
    private int touchScreenLayer, dragLayer, textLayer;
    private Vector3 prevPos, orgDrop, dragOffset;
    private Transform draggedTar, hoverTar;
    private bool slideReleaseWait = true;
    bool isHitting = false;
    bool isClicking = false;
    Transform clickTarget;
    MoneyIntoButton mb;
    BoxInterfaceScreen Bis;


    private void Awake()
    {
        Bis = FindObjectOfType<BoxInterfaceScreen>();
        touchScreenLayer = 1 << LayerMask.NameToLayer("TouchScreen");
        dragLayer = 1 << LayerMask.NameToLayer("DragLayer");
        textLayer = 1 << LayerMask.NameToLayer("TextField");
    }

    private void Update()
    {
        ray.origin = transform.position - transform.forward * 0.5f;
        ray.direction = transform.forward;
        
        if(!isHitting)
        {
            if (Physics.Raycast(ray, out hit, 1, textLayer))
            {
                isHitting = true;
                if (hit.transform.parent.name == "FullTextField")
                    StartCoroutine(HitTextField(hit));
                if (hit.transform.parent.name == "SliderHorizontal")
                    StartCoroutine(HitBoxSlider(hit));
                if (hit.transform.parent.name == "SliderNextSlide")
                    StartCoroutine(HitNextSlidePlz(hit));
            }
        }
    }

   

    public void releaseSlider()
    {
        draggedTar = null;
        StartCoroutine(slideWait());
    }
    IEnumerator slideWait()
    {
        slideReleaseWait = false;
        yield return new WaitForSeconds(.5f);
        slideReleaseWait = true;
        yield break;
    }
    private IEnumerator HitNextSlidePlz(RaycastHit hit)
    {
        Vector3 startPos = transform.position;
        Vector3 scrollStartPos = hit.transform.parent.localPosition;
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                //waiting for OnDisable()
            }
            else
            {
                Bis.isScrolling = true;
                hit.transform.parent.localPosition = scrollStartPos - Vector3.right * Vector3.Distance(transform.position,startPos);
            }
            yield return null;
        }
        isClicking = true;
        yield return isHitting = false;
    }
    IEnumerator HitBoxSlider(RaycastHit hit)
    {
        draggedTar = hit.transform;
        dragOffset = transform.position - draggedTar.position;
        draggedTar.parent.GetComponent<SlideAbleObject>().TakeControl(this);

        while(gameObject.activeSelf)
        {
            draggedTar.parent.GetComponent<SlideAbleObject>().setOwnerPosition(transform.position - dragOffset);
            yield return null;
        }
        yield return isHitting = false;
    }
    IEnumerator HitTextField(RaycastHit hit)
    {
        Vector3 startPos = transform.localPosition;
        Vector3 scrollStartPos = hit.transform.parent.localPosition;
        Vector3 clickTargetStartPos = hit.transform.localPosition;
        clickTarget = hit.transform;
        isClicking = true;
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.localPosition, startPos) < 0.01f && isClicking == true)
            {
                //waiting for OnDisable()
            }
            else if (Mathf.Abs(transform.localPosition.y - startPos.y) > Mathf.Abs(transform.localPosition.x - startPos.x)  && Mathf.Abs(transform.localPosition.x - startPos.x) < 0.1f)
            {
                //clickTarget = null;
                isClicking = false;
                hit.transform.parent.localPosition = scrollStartPos + Vector3.up * (transform.localPosition.y - startPos.y);
                Bis.isScrolling = true;
                //Scrolling up/down
            }
            else
            {
                //clickTarget = null;
                isClicking = false;
                hit.transform.localPosition = clickTargetStartPos + Vector3.right * (transform.localPosition.x - startPos.x);
                if (hit.transform.localPosition.x > clickTargetStartPos.x)
                    hit.transform.localPosition = clickTargetStartPos;
                Bis.SlideTextfieldLeft(hit.transform.localPosition.x);
                Bis.isScrolling = true;
                //scrolling left/right
            }
            yield return null;
        }
        yield return isHitting = false; 
    }

    private void OnDisable()
    {
        isHitting = false;
        Bis.isScrolling = false;
        if (clickTarget != null)
        {
            Bis.ClickTextField(clickTarget.GetSiblingIndex());
            clickTarget = null;
        }

        slideReleaseWait = true;
        if (draggedTar != null)
        {
            draggedTar.parent.GetComponent<SlideAbleObject>().ReleaseControl();
            draggedTar = null;
        }
        if (hoverTar != null)
        {
            mb.curState = MoneyIntoButton.state.Pressed;
            hoverTar = null;
            mb = null;
        }
    }

}
