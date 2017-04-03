using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;
using System;

public class ListBudgets : MonoBehaviour{
    private Transform but;
    private GameObject overlay;

    private float moveTH = Screen.height / 20f;
    private float touchMove = 0;
    private Vector2 prevPoint, v2Null = Vector3.one*-10000;
    private int curTrackedID = -1;
    bool subbed = false;

    public void setOverlay(GameObject go)
    {
        overlay = go;
        if (overlay != null)
            overlay.SetActive(false);
        Sub();
    }
    private void Awake()
    {
        overlay = GameObject.Find("Overlay (Canvas)");
        GenerateBudgetList();
    }
    private void OnEnable()
    {
        if(overlay != null)
            overlay.SetActive(false);
        Sub();
    }
    private void Sub()
    {
        if (subbed)
            return;
        prevPoint = v2Null;
        TouchManager.Instance.TouchesBegan += TouchStart;
        TouchManager.Instance.TouchesMoved += TouchMoved;
        TouchManager.Instance.TouchesEnded += TouchEnd;
        subbed = true;
    }

    private void TouchEnd(object sender, TouchEventArgs e)
    {
        bool lostTrack = true;
        for(int x = 0; x<TouchManager.Instance.NumberOfTouches; ++x)
        {
            if (TouchManager.Instance.ActiveTouches[x].Id == curTrackedID)
                lostTrack = false;
        }
        if(lostTrack)
        {
            prevPoint = v2Null;
            touchMove = 0;
        }
    }

    private void TouchMoved(object sender, TouchEventArgs e)
    {
        if (prevPoint == v2Null)
        {
            prevPoint = TouchManager.Instance.ActiveTouches[0].Position;
            curTrackedID = TouchManager.Instance.ActiveTouches[0].Id;
        }
        for(int x = 0; x<TouchManager.Instance.NumberOfTouches; ++x)
        {
            if(TouchManager.Instance.ActiveTouches[x].Id==curTrackedID)
            {
                float frameDist = TouchManager.Instance.ActiveTouches[x].Position.y - prevPoint.y;
                touchMove += frameDist;
                prevPoint = TouchManager.Instance.ActiveTouches[x].Position;
                if (Math.Abs(touchMove) >= moveTH)
                {
                    GetComponent<RectTransform>().localPosition += Vector3.up * frameDist;
                }
            }
        }
    }

    private void TouchStart(object sender, TouchEventArgs e)
    {
        if (prevPoint == v2Null)
        {
            prevPoint = TouchManager.Instance.ActiveTouches[0].Position;
            curTrackedID = TouchManager.Instance.ActiveTouches[0].Id;
        }
    }

    private void Unsub()
    {
        try
        {
            TouchManager.Instance.TouchesBegan -= TouchStart;
            TouchManager.Instance.TouchesMoved -= TouchMoved;
            TouchManager.Instance.TouchesEnded -= TouchEnd;
        }
        catch { }
        subbed = false;
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDisable()
    {
        transform.parent.GetComponent<Image>().enabled = false;
        Unsub();
        if (overlay != null)
            overlay.SetActive(true);
    }
    public void GenerateBudgetList()
    {
        but = Resources.Load<Transform>("budgetListButton");
        string path = Application.streamingAssetsPath + "/ExcelFiles/Budget/";
        string[] budgetList = Directory.GetFiles(path);
        for (int x = budgetList.Length - 1; x > -1; --x)
        {
            if (budgetList[x].Contains(".xls") && !budgetList[x].Contains(".meta"))
            {
                Transform go = Instantiate(but, transform, false);
                go.GetChild(0).GetComponent<Text>().text = budgetList[x].Substring(path.Length, budgetList[x].Length - path.Length - 4);
            }
            else
                continue;
        }
        if (transform.childCount > 5)
        {
            GetComponent<RectTransform>().localPosition = Vector3.up * -(transform.childCount * 26 + 52);
        }
        else
        {
            GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        
        GetComponent<RectTransform>().sizeDelta = new Vector2(1920, Screen.height + transform.childCount * 108);
    }

    public void ScrollControl()
    {
        if (transform.childCount > 1)
        {
            if (transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().position.y >= 52)
            {
                GetComponent<RectTransform>().localPosition = Vector3.up * (transform.childCount * 26 + 52);
            }
            if (transform.GetChild(0).GetComponentInChildren<RectTransform>().position.y <= Screen.height - 52)
            {
                GetComponent<RectTransform>().localPosition = Vector3.up * -(transform.childCount * 26 + 52);
            }
        }
        else
            GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}
