using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAbleObject : MonoBehaviour
{
    private Vector3 orgPos;
    private enum DirBehaviour { Horizontal, Vertical, TowardsTarget, Free }
    private enum DestBehaviour { Stay, Destroy, ReturnToOrigin }
    private enum ReleaseBehaviour { Stay, Return, CarryMomentum}
    [SerializeField]
    private ReleaseBehaviour ReleaseSetting;
    [SerializeField]
    private DirBehaviour DirectionSetting;
    [SerializeField]
    private DestBehaviour DestinationSetting;
    [HideInInspector]
    public Transform target, slider, area;
    private float dist, normDist;
    [SerializeField]
    private float returnSpeed;
    TheNewestMarker owner;
    private bool returning = false;
    private BoxBehaviour bh;
    private cardBoardManager cbm;

    private void Start()
    {
        target = transform.GetChild(1);
        area = transform.GetChild(2);
        slider = transform.GetChild(0);
        bh = FindObjectOfType<BoxBehaviour>();
        cbm = FindObjectOfType<cardBoardManager>();
        EventManager.OnUIPlaced += SetVariables;
        EventManager.OnBoxAtTable += NextCategory;
    }

    private void NextCategory()
    {
        bh = cbm.CardBoxList[EventManager.CurrentCategory].GetComponent<BoxBehaviour>();
        for (int x = 0; x < transform.childCount; ++x)
        {
            transform.GetChild(x).gameObject.SetActive(true);
        }
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
        switch (ReleaseSetting)
        {
            case ReleaseBehaviour.Return:
                if (!returning)
                    StartCoroutine(Return());
                break;
            case ReleaseBehaviour.Stay:
                break;
            case ReleaseBehaviour.CarryMomentum:
                StartCoroutine(Glide());
                break;
        }
        StartCoroutine(Return());
    }

    private IEnumerator Glide()
    {

        yield break;
    }

    private void SetVariables()
    {
        orgPos = slider.position;
        switch (DirectionSetting)
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
        switch (DirectionSetting)
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
            DestinationInvoke();
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
        if(bh!=null)
            bh.setTapeRip(normDist);
        slider.position = moddedPos;
    }

    private void DestinationInvoke()
    {
        bh = null;
        if (DirectionSetting == DirBehaviour.Horizontal)
            EventManager.RipTapeSliderDone();
        else
            EventManager.CategorySliderDone();
        for(int x = 0; x<transform.childCount; ++x)
        {
            transform.GetChild(x).gameObject.SetActive(false);
        }
    }
    private void instantReturn()
    {
        slider.position = orgPos;
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
        slider.position = orgPos;
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
