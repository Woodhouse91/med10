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
    bool isClicking = false;
    private enum HitTarget { FullTextField, SliderHorizontal, SliderNextSlide, SliderFlagCategories, None}
    HitTarget hitTarget = HitTarget.None;
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
        if(hitTarget == HitTarget.None)
        {
            ray.origin = transform.position - transform.forward * 0.5f;
            ray.direction = transform.forward;
            if (Physics.Raycast(ray, out hit, 1, textLayer))
            {
                print(hit);
                if (hit.transform.parent.name == "FullTextField")
                {
                    hitTarget = HitTarget.FullTextField;
                    StartCoroutine(HitTextField(hit));
                    return;
                }
                if (hit.transform.parent.name == "SliderHorizontal")
                {
                    hitTarget = HitTarget.SliderHorizontal;
                    StartCoroutine(HitBoxSlider(hit));
                    return;
                }
                if (hit.transform.parent.name == "SliderNextSlide")
                {
                    hitTarget = HitTarget.SliderNextSlide;
                    StartCoroutine(HitNextSlidePlz(hit));
                    return;
                }
                if (hit.transform.parent.name == "SliderFlagCategories")
                {
                    hitTarget = HitTarget.SliderFlagCategories;
                    StartCoroutine(HitFlagSlider(hit));
                    return;
                }

            }
        }
    }

    private IEnumerator HitFlagSlider(RaycastHit hit) //SliderFlagCategories
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
                hit.transform.parent.localPosition = scrollStartPos + Vector3.right * Vector3.Distance(transform.position, startPos);
            }
            yield return null;
        }
        isClicking = true;
        yield return null;

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
    private IEnumerator HitNextSlidePlz(RaycastHit hit) //SliderNextSlide
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
        yield return null;
    }
    IEnumerator HitBoxSlider(RaycastHit hit) //SliderHorizontal
    {
        draggedTar = hit.transform;
        dragOffset = transform.position - draggedTar.position;
        draggedTar.parent.GetComponent<SlideAbleObject>().TakeControl(this);

        while(gameObject.activeSelf)
        {
            draggedTar.parent.GetComponent<SlideAbleObject>().setOwnerPosition(transform.position - dragOffset);
            yield return null;
        }
        yield return null;
    }
    IEnumerator HitTextField(RaycastHit hit) //FullTextField
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
                Bis.SlideTextfieldLeft(hit.transform,false);
                Bis.isScrolling = true;
                //scrolling left/right
            }
            yield return null;
        }
        yield return null; 
    }

    private void OnDisable()
    {
        print(hitTarget);
        switch (hitTarget)
        {
            case HitTarget.FullTextField:
                Bis.isScrolling = false;
                Bis.SlideTextfieldLeft(hit.transform, true);
                if (clickTarget != null)
                {
                    Bis.ClickTextField(clickTarget.GetSiblingIndex());
                    clickTarget = null;
                }
                break;
            case HitTarget.SliderHorizontal:
                slideReleaseWait = true;
                if(draggedTar != null)
                    draggedTar.parent.GetComponent<SlideAbleObject>().ReleaseControl();
                draggedTar = null;
                break;
            case HitTarget.SliderNextSlide:
                Bis.isScrolling = false;
                break;
            case HitTarget.SliderFlagCategories:
                break;
            default:
                break;
        }
        hitTarget = HitTarget.None;
    }
}
