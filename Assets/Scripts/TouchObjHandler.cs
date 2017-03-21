using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TouchScript;
public class TouchObjHandler : MonoBehaviour/*,IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler*/
{
    private GameObject pref;
    private int prevTouch;
    private Transform VrUi;
    //public PointerEventData[] dat = new PointerEventData[10];

    public struct TouchRef
    {
        public GameObject GO;
        public int id;
    }
    TouchRef[] touch = new TouchRef[10];
    void Start()
    {
        VrUi = GameObject.Find("VRInterface").transform;
        pref = Resources.Load<GameObject>("TouchObject");
        for(int x = 0; x<10; ++x)
        {
            touch[x].GO = Instantiate(pref);
            touch[x].GO.transform.parent = VrUi;
            touch[x].GO.transform.position = Vector3.one * 1000;
            touch[x].GO.SetActive(false); 
            touch[x].id = -10;
        }
        Subscribe();
    }



    private void Subscribe()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan += touchesBeganHandler;
            TouchManager.Instance.TouchesMoved += touchesMovedHandler;
            TouchManager.Instance.TouchesEnded += touchesEndedHandler;
        }
    }


    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
            TouchManager.Instance.TouchesMoved -= touchesMovedHandler;
            TouchManager.Instance.TouchesEnded -= touchesEndedHandler;
        }
    }
    private void OnApplicationQuit()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
            TouchManager.Instance.TouchesMoved -= touchesMovedHandler;
            TouchManager.Instance.TouchesEnded -= touchesEndedHandler;
        }
    }


    private void touchesEndedHandler(object sender, TouchEventArgs e)
    {
        bool removeItem;
        for (int k = 0; k<touch.Length; ++k)
        {
            removeItem = true;
            for(int x = 0; x<TouchManager.Instance.ActiveTouches.Count; ++x)
            {
                if (touch[k].id == TouchManager.Instance.ActiveTouches[x].Id)
                    removeItem = false;
            }
            if (removeItem)
            {
                touch[k].id = -10;
                touch[k].GO.transform.position = Vector3.one * 1000;
                touch[k].GO.SetActive(false);
            }
        }
    }
    private void touchesBeganHandler(object sender, TouchEventArgs e)
    {
        for(int x = 0; x< TouchManager.Instance.ActiveTouches.Count; ++x)
        {
            touch[x].GO.SetActive(true);
            touch[x].GO.transform.localPosition = NormPos(TouchManager.Instance.ActiveTouches[x].Position);
            touch[x].GO.transform.LookAt(touch[x].GO.transform.position + VrUi.forward);
            touch[x].id = TouchManager.Instance.ActiveTouches[x].Id;
        }
    }
    private void touchesMovedHandler(object sender, TouchEventArgs e)
    {
        for (int x = 0; x < TouchManager.Instance.ActiveTouches.Count; ++x)
        {
            if (touch[x].id == TouchManager.Instance.ActiveTouches[x].Id)
            {
                touch[x].GO.transform.localPosition = NormPos(TouchManager.Instance.ActiveTouches[x].Position);
            }
        }
    }

    private Vector3 NormPos(Vector2 dat)
    {
        float x, y, h = Screen.height, w = Screen.width;
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
        return new Vector3(x, y);
    }
}
