using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAbleObject : MonoBehaviour
{
    private enum DirBehaviour { Horizontal, Vertical, TowardsTarget, Free }
    private enum DestBehaviour { Stay, Destroy, ReturnToOrigin }
    private Vector3 orgPos;
    [SerializeField]
    private DirBehaviour Direction;
    [SerializeField]
    private DestBehaviour DestinationSetting;
    private Transform target, slider, area;
    private float dist, normDist;
    [SerializeField]
    private float returnSpeed;
    TheNewestMarker owner;
    private bool returning = false;
    private BoxBehaviour bh;

    private void Start()
    {
        target = transform.GetChild(1);
        area = transform.GetChild(2);
        slider = transform.GetChild(0);
        bh = FindObjectOfType<BoxBehaviour>();
        EventManager.OnUIPlaced += SetVariables;
    }
    private void Unsub()
    {
        EventManager.OnUIPlaced -= SetVariables;
    }
    public void TakeControl(TheNewestMarker script)
    {
        if (owner == null)
        {
            owner = script;
        }
    }
    public void ReleaseControl()
    {
        owner = null;
        if (!returning)
        {
            StartCoroutine(Return());
        }
    }
    private void SetVariables()
    {
        orgPos = slider.position;
        switch (Direction)
        {
            case DirBehaviour.Horizontal:
                target.position = new Vector3(target.position.x, slider.position.y, target.position.z);
                break;
            case DirBehaviour.Vertical:
                target.position = new Vector3(slider.position.x, target.position.y, target.position.z);
                break;
            default:
                break;
        }
        normDist = 0;
        dist = Vector3.Distance(slider.position, target.position);
    }
    public void setOwnerPosition(Vector3 pos)
    {
        Vector3 moddedPos = pos;
        switch (Direction)
        {
            case DirBehaviour.Horizontal:
                moddedPos.y = orgPos.y;
                break;
            default:
                break;
        }
        normDist = 1f - Vector3.Distance(moddedPos, target.position) / dist;
        if (normDist<0)
        {
            normDist = 0;
            moddedPos = orgPos;
        }
        if(normDist >= 0.95f)
        {
            normDist = 1;
            moddedPos = target.position;
            owner.releaseSlider();
            owner = null;
            switch (DestinationSetting)
            {
                case DestBehaviour.Destroy:
                    if (GetComponent<SlideSpecialDestroy>() == null)
                        Destroy(slider.parent);
                    else
                        GetComponent<SlideSpecialDestroy>().invoke();
                    break;
                case DestBehaviour.ReturnToOrigin:
                    StartCoroutine(Return());
                    break;
            }
        }
        bh.setTapeRip(normDist);
        slider.position = moddedPos;
    }


    private IEnumerator Return()
    {
        if(returnSpeed==0)
        {
            slider.position = orgPos;
            yield break;
        }
        returning = true;
        Vector3 dir = -(slider.position - orgPos).normalized;
        while (Vector3.Distance(slider.position, orgPos)>0.01f && owner==null)
        {
            slider.position += dir * returnSpeed * Time.deltaTime;
            yield return null;
        }
        returning = false;
        if(owner == null)
            yield break;
        slider.localPosition = orgPos;
    }

    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }

}
