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
    private enum HitTarget { FullTextField, SliderHorizontal, SliderNextSlide, ColorImage, None}
    HitTarget hitTarget = HitTarget.None;
    Transform clickTarget;
    MoneyIntoButton mb;
    BoxInterfaceScreen Bis;
    Vector3 FollowVelocity;

    private void Awake()
    {
        Bis = FindObjectOfType<BoxInterfaceScreen>();
        EventManager.OnDisableAllMarkers += DisableMarker;
        touchScreenLayer = 1 << LayerMask.NameToLayer("TouchScreen");
        dragLayer = 1 << LayerMask.NameToLayer("DragLayer");
        textLayer = 1 << LayerMask.NameToLayer("TextField");
    }
    private void Unsub()
    {
        EventManager.OnDisableAllMarkers -= DisableMarker;
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
   
    private void DisableMarker()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(hitTarget == HitTarget.None)
        {
            ray.origin = transform.position - transform.forward * 0.5f;
            ray.direction = transform.forward;
            if (Physics.Raycast(ray, out hit, 1, textLayer))
            {
                if (hit.transform.parent.name == "FullTextField")
                {
                    hit.collider.enabled = false;
                    hitTarget = HitTarget.FullTextField;
                    StartCoroutine(HitTextField(hit));
                    return;
                }
                if (hit.transform.parent.name == "SliderHorizontal")
                {
                    hit.collider.enabled = false;
                    hitTarget = HitTarget.SliderHorizontal;
                    StartCoroutine(HitBoxSlider(hit));
                    return;
                }
                if (hit.transform.parent.name == "SliderNextSlide")
                {
                    hit.collider.enabled = false;
                    hitTarget = HitTarget.SliderNextSlide;
                    StartCoroutine(HitNextSlidePlz(hit));
                    return;
                }
                if(hit.transform.name == "ColorImage")
                {
                    hit.collider.enabled = false;
                    hitTarget = HitTarget.ColorImage;
                    StartCoroutine(HitColorImage(hit));
                    return;
                }
            }
        }
    }

    IEnumerator HitColorImage(RaycastHit hit)
    {
       
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position,hit.transform.position)< 0.025f)
            {
                isClicking = true;
                hit.transform.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1f);
            }
            else
            {
                isClicking = false;
                hit.transform.GetComponent<Image>().color = Color.white;
            }
            yield return null;
        }
        yield return null;

    }

    private IEnumerator HitNextSlidePlz(RaycastHit hit) //SliderNextSlide
    {
        Vector3 startPos = transform.localPosition;
        Vector3 scrollStartPos = hit.transform.parent.localPosition;
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                FollowVelocity = GetComponent<Rigidbody>().velocity;
                //waiting for OnDisable()
            }
            else
            {
                Bis.isScrolling = true;
                Bis.Moving();
                hit.transform.parent.parent.localPosition = scrollStartPos + Vector3.right * (transform.localPosition.x - startPos.x); //Moving BoxInterfaceScreen
                
            }
            yield return null;
        }
        yield return null;
    }
    
    IEnumerator HitBoxSlider(RaycastHit hit) //SliderHorizontal
    {
        RipTapeSlider RTS = hit.transform.parent.GetComponent<RipTapeSlider>();
        BoxInterfaceScreen Bis = transform.parent.GetComponentInChildren<BoxInterfaceScreen>();
        Vector3 startPos = transform.localPosition;
        Vector3 sliderStartPos = hit.transform.localPosition;
        while(gameObject.activeSelf)
        {
            hit.transform.localPosition = sliderStartPos + Vector3.right * (transform.localPosition.x - startPos.x);
            Bis.TapeRipSlide(RTS.Dist(Bis));
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
       // Bis.InheritVelocity(Vector3.zero);
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.localPosition, startPos) < 0.01f && isClicking == true)
            {
               
                //waiting for OnDisable()
            }
            else
            {
                clickTarget = null;
                isClicking = false;
                hit.transform.parent.localPosition = scrollStartPos + Vector3.up * (transform.localPosition.y - startPos.y);
                Bis.isScrolling = true;
                //Scrolling up/down
            }
            
            yield return null;
        }
        yield return null; 
    }

    private void OnDisable()
    {
        print(hitTarget);
        //try
        //{
            switch (hitTarget)
            {
                case HitTarget.FullTextField:
                    hit.collider.enabled = true;
                    Bis.isScrolling = false;
                    if (clickTarget != null)
                    {
                        LuxusSegmentHandler.HighlightCategory(Bis.FindTransform(hit.transform));
                        Bis.ClickTextField(clickTarget.GetSiblingIndex());
                        clickTarget = null;
                    }
                    else
                    {
                      //  Bis.InheritVelocity(FollowVelocity);    
                    }
                    break;
                case HitTarget.SliderHorizontal:
                    hit.transform.parent.GetComponent<RipTapeSlider>().ReturnToStart();
                    break;
                case HitTarget.SliderNextSlide:
                    hit.collider.enabled = true;
                    Bis.isScrolling = false;
                    break;
                case HitTarget.ColorImage:
                    if (isClicking)
                    {
                        Bis.FlagIt(hit.transform.parent);
                    }
                    hit.transform.GetComponent<Image>().color = Color.white;
                    hit.collider.enabled = true;
                    break;
                default:
                    break;
            }
        //}
        //catch { }
        hitTarget = HitTarget.None;
       
    }
}
