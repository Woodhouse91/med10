﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;
public class ListBudgets : MonoBehaviour {
    private Transform but;
    [SerializeField]
    private GameObject overlay, eventSystem;

    [SerializeField]
    private TouchManager tm;
    // Use this for initialization
    private void Start()
    {
        GenerateBudgetList();
    }
    public void GenerateBudgetList()
    {
        but = Resources.Load<Transform>("budgetListButton");
        string path = Application.streamingAssetsPath + "/ExcelFiles/Budget/";
        string[] budgetList = Directory.GetFiles(path);
        for (int x = budgetList.Length - 1; x > -1; --x)
        {
            if (budgetList[x].Contains(".xlsx") && !budgetList[x].Contains(".meta"))
            {
                Transform go = Instantiate(but, transform, false);
                go.GetChild(0).GetComponent<Text>().text = budgetList[x].Substring(path.Length, budgetList[x].Length - path.Length - 5);
            }
            else
                continue;
        }
        GetComponent<RectTransform>().localPosition = Vector3.up * -(transform.childCount * 26 + 52);
        GetComponent<RectTransform>().sizeDelta = new Vector2(1920, Screen.height + transform.childCount * 108);
    }
    private void OnDisable()
    {
        if(overlay!=null)
            overlay.SetActive(true);
        if(eventSystem!=null)
            eventSystem.SetActive(false);
        if (tm != null)
            tm.enabled = true;
    }
    private void OnEnable()
    {
        overlay.SetActive(false);
        eventSystem.SetActive(true);
        tm.enabled = false;
    }

    public void ScrollControl()
    {
        if(transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().position.y >= 52){
            GetComponent<RectTransform>().localPosition = Vector3.up*(transform.childCount*26+52);
        }
        if (transform.GetChild(0).GetComponentInChildren<RectTransform>().position.y <= Screen.height - 52)
        {
            GetComponent<RectTransform>().localPosition = Vector3.up * -(transform.childCount * 26 + 52);
        }
    }
}
