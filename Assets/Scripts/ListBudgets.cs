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
    private bool scrolling;
    [SerializeField]
    private float scrollSpeed = 1;
    float tarY;
    Transform sibling;

    private void Awake()
    {
        overlay = GameObject.Find("Overlay (Canvas)");
        overlay.SetActive(false);
        sibling = transform.parent.GetChild(0);
        sibling.gameObject.SetActive(false);
        Sub();
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
        scrolling = false;
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
        scrolling = true;
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
        sibling.GetComponent<introtouch>().setOverlay(overlay);
        Unsub();
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
        tarY = Screen.height / 2f - ((transform.childCount) * 54);
        GetComponent<RectTransform>().localPosition = Vector3.up * tarY;
        GetComponent<RectTransform>().sizeDelta = new Vector2(1920, Screen.height * 2 + transform.childCount*108);
    }

    private void Update()
    {
        if (scrolling)
        {

        }
        else
        {
            if (GetComponent<RectTransform>().localPosition.y <= tarY)
                GetComponent<RectTransform>().localPosition += ((Vector3.up * tarY)-GetComponent<RectTransform>().localPosition).normalized*scrollSpeed*Time.deltaTime;
            if (GetComponent<RectTransform>().localPosition.y >= -tarY)
                GetComponent<RectTransform>().localPosition += ((Vector3.up * -tarY) - GetComponent<RectTransform>().localPosition).normalized * scrollSpeed * Time.deltaTime;
        }
        GetComponent<RectTransform>().localPosition = new Vector3(0, GetComponent<RectTransform>().localPosition.y, 0);
    }
}
