using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchObjHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler/*, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler*/
{
    private List<GameObject> touchObj = new List<GameObject>(1);
    private GameObject pref;
    private byte activeGOs = 0;
    private List<int> touchID = new List<int>(1);
    private Transform VrUi;

    private void Start()
    {
        VrUi = GameObject.Find("VRInterface").transform;
        pref = Resources.Load<GameObject>("TouchObject");
    }
  /*  public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
       
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        ++activeGOs;
        if (activeGOs > touchObj.Count)
        {
            touchObj.Add(Instantiate(pref));
            touchObj[activeGOs-1].transform.parent = VrUi;
            touchObj[activeGOs-1].transform.localPosition = NormPos(eventData.position);
            touchObj[activeGOs - 1].transform.LookAt(touchObj[activeGOs-1].transform.position + VrUi.forward);
            touchID.Add(eventData.pointerId);
        }
        else
        {
            for(int x = 0; x<touchID.Count; ++x)
            {
                if(touchID[x] == -1)
                {
                    touchObj[x].SetActive(true);
                    touchID[x] = eventData.pointerId;
                    touchObj[x].transform.localPosition = NormPos(eventData.position);
                    break;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for(int x = 0; x<activeGOs; ++x)
        {
            if (touchID[x] == eventData.pointerId)
            {
                touchObj[x].SetActive(false);
                touchID[x] = -1;
            }
        }
        activeGOs--;
    }

    private Vector3 NormPos(Vector2 dat)
    {
        print(dat.x);
        print(dat.y);
        float x, y, h =Screen.height, w = Screen.width;
        if (dat.x < 0.0f)
            x = -.3f;
        else if (dat.x > w)
            x = .3f;
        else
            x = dat.x / w * .6f - .3f;
        if (dat.y < 0.0f)
            y = -.17f;
        else if (dat.y > h)
            y = .17f;
        else
            y = dat.y / h * .34f - .17f;
        return -VrUi.right * x + VrUi.up * y;
    }
}
