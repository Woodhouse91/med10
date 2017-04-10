using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TouchScript;

public class SetBudgetName : MonoBehaviour{
    private bool isAClick = true;
    Button mySet;
    Image myCol;
    [SerializeField]
    private Color normal, highlighted, pressed;
    private float minX = 0, maxX, minY, maxY;
    RectTransform myRect;
    List<TouchPoint> t;
    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
        float sW = Screen.width / 2f, sH = Screen.height / 2f;
        mySet = GetComponent<Button>();
        myCol = GetComponent<Image>();
        myCol.color = normal;
        t = new List<TouchPoint>();
        TouchManager.Instance.TouchesBegan += touchStart;
        TouchManager.Instance.TouchesEnded += touchEnd;
        TouchManager.Instance.TouchesMoved += touchMove;
    }

    private void touchMove(object sender, TouchEventArgs e)
    {
        for(int x = 0; x<e.Touches.Count; ++x)
        {
            if (!hit(e.Touches[x].Position))
                t.Remove(e.Touches[x]);
            else if (!t.Contains(e.Touches[x]))
                t.Add(e.Touches[x]);
        }
    }

    private void touchEnd(object sender, TouchEventArgs e)
    {
        for(int x = 0; x<e.Touches.Count; ++x)
        {
            if (hit(e.Touches[x].Position) && t.Contains(e.Touches[x]))
            {
                setName();
                t.Clear();
            }
        }
    }

    private void touchStart(object sender, TouchEventArgs e)
    {
        for(int x = 0; x<e.Touches.Count; ++x)
        {
            if (hit(e.Touches[x].Position))
            {
                t.Add(e.Touches[x]);
            }
        }
    }

    private void Unsub()
    {
        try
        {
            TouchManager.Instance.TouchesBegan -= touchStart;
            TouchManager.Instance.TouchesEnded -= touchEnd;
        }
        catch { }
    }
    private void OnDisable()
    {
        Unsub();
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    public void setName()
    {
        DataAppLauncher.LaunchApplication(transform.GetChild(0).GetComponent<Text>().text);
        transform.parent.gameObject.SetActive(false);
        transform.parent.parent.GetChild(0).gameObject.SetActive(true);
    }
    private void Update()
    {
        if (t.Count > 0)
            myCol.color = pressed;
        else
            myCol.color = normal;
    }
    private bool hit(Vector2 p)
    {
        minX = myRect.position.x - (myRect.sizeDelta.x / 2f);
        maxX = myRect.position.x + (myRect.sizeDelta.x / 2f);
        minY = myRect.position.y - (myRect.sizeDelta.y / 2f);
        maxY = myRect.position.y + (myRect.sizeDelta.y / 2f);
        return ((p.x > minX && p.x < maxX) && (p.y > minY && p.y < maxY));
    }
}
