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
    [SerializeField]
    private Transform target;
    private float dist, normDist;
    [SerializeField]
    private float returnSpeed;
    TheNewestMarker owner;
    private bool returning = false;
    [SerializeField]
    private BoxBehaviour bh;

    private void Awake()
    {
        EventManager.OnUIPlaced += SetVariables;
    }
    private void Unsub()
    {
        EventManager.OnUIPlaced -= SetVariables;
    }
    public void TakeControl(TheNewestMarker script)
    {
        if (owner == null)
            owner = script;
    }
    private void SetVariables()
    {
        orgPos = transform.localPosition;
        switch (Direction)
        {
            case DirBehaviour.Horizontal:
                target.localPosition = new Vector3(target.localPosition.x, transform.localPosition.y, target.localPosition.z);
                break;
            case DirBehaviour.Vertical:
                target.localPosition = new Vector3(transform.localPosition.x, target.localPosition.y, target.localPosition.z);
                break;
            default:
                break;
        }
        dist = Vector3.Distance(transform.position, transform.position);
    }

    private void LateUpdate()
    {
        if (owner != null)
        {
            Vector3 moddedPos = transform.localPosition;
            switch (Direction)
            {
                case DirBehaviour.Horizontal:
                    moddedPos = new Vector3(transform.localPosition.x, target.localPosition.y, transform.localPosition.z);
                    break;
                case DirBehaviour.Vertical:
                    moddedPos = new Vector3(target.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                    break;
                case DirBehaviour.TowardsTarget:
                    break;
                case DirBehaviour.Free:
                    break;
            }
            if (Vector3.Distance(moddedPos, target.localPosition) > dist)
                if (normDist < 0.5f)
                    transform.localPosition = orgPos;
                else
                    transform.localPosition = target.localPosition;
            else
                transform.localPosition = moddedPos;
            normDist = 1 - Vector3.Distance(transform.position, target.position) / dist;
            if (normDist < 0)
                normDist = 0;
            if (normDist > 0.97f)
            {
                owner.releaseSlider();
                owner = null;
                switch (DestinationSetting)
                {
                    case DestBehaviour.Destroy:
                        Destroy(target);
                        Destroy(gameObject);
                        break;
                    case DestBehaviour.ReturnToOrigin:
                        StartCoroutine(Return());
                        break;
                    default:
                        break;
                }
            }
        }
        else if(transform.localPosition != orgPos && !returning)
            StartCoroutine(Return());
        if (bh != null)
            bh.setTapeRip(normDist);

    }

    private IEnumerator Return()
    {
        returning = true;
        Vector3 dir = (transform.localPosition - orgPos).normalized;
        while (Vector3.Distance(transform.localPosition, orgPos)>0.01f && owner==null)
        {
            transform.localPosition += dir * returnSpeed * Time.deltaTime;
            yield return null;
        }
        returning = false;
        if(owner == null)
            yield break;
        transform.localPosition = orgPos;
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
