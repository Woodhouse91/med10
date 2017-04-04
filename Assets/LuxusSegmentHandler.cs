using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuxusSegmentHandler : MonoBehaviour {
    private static Vector3 offset = new Vector3(0, 0, .175f);
    private static Transform segmentPrefab;
    private static Transform holder, nHolder;
    private static List<Transform> segment;
    private static List<Transform> activeSegments;
    private static List<List<Transform>> moneyPerCategory;
    private static List<Transform> moneyInCategory;
	// Use this for initialization
	private void Start ()
    {
        moneyPerCategory = new List<List<Transform>>();
        moneyInCategory = new List<Transform>();
        activeSegments = new List<Transform>();
        StaticStart();
	}
    private static void StaticStart()
    {
        segmentPrefab = Resources.Load<Transform>("LuxusSegment");
        holder = GameObject.Find("Segmentholder").transform;
        EventManager.OnBoxAtTable += GenerateTable;
        EventManager.OnExcelDataLoaded += PoolSegments;
        EventManager.OnRipTapeSliderDone += MoveMoneyToTable;
    }

    private static void MoveMoneyToTable()
    {
        
    }

    private static void GenerateTable(BoxBehaviour s)
    {
        nHolder = Instantiate(holder);
        for (int x = 0; x < s.CategoryString.Count; ++x)
        {
            activeSegments.Add(segment[0]);
            segment.Remove(segment[0]);
            activeSegments[x].SetParent(holder);
            activeSegments[x].localPosition = offset * x;
            activeSegments[x].GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = s.CategoryString[x];
            buildMoneyList(s.CategoryInt[x]);
        }
        ExposeTable();
    }
    public static void ExposeTable()
    {

    }
    private static IEnumerator moveTable()
    {
        yield break;
    }
    public static void ReleaseTable()
    {
        activeSegments.Clear();
        moneyPerCategory.Clear();
        nHolder.SetParent(holder.parent);
        nHolder.localPosition = Vector3.zero;
        nHolder.localRotation = Quaternion.identity;


        holder = nHolder;
    }
    private static void buildMoneyList(int cat)
    {
        int val = DataHandler.expenseData[13 , cat];
        for(int x = 0; x<val/1000; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(1000));
        }
        val = val % 1000;
        for (int x = 0; x < val / 500; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(500));
        }
        val = val % 500;
        for (int x = 0; x < val / 200; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(200));
        }
        val = val % 200;
        for (int x = 0; x < val / 100; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(100));
        }
        val = val % 100;
        for (int x = 0; x < val / 50; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(50));
        }
        val = val % 50;
        for (int x = 0; x < val / 20; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(20));
        }
        val = val % 20;
        for (int x = 0; x < val / 10; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(10));
        }
        val = val % 10;
        for (int x = 0; x < val / 5; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(5));
        }
        val = val % 5;
        for (int x = 0; x < val / 2; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(2));
        }
        val = val % 2;
        for (int x = 0; x < val / 1; ++x)
        {
            moneyInCategory.Add(MoneyHolder.getCurrency(1));
        }
        moneyPerCategory.Add(moneyInCategory);
        moneyInCategory.Clear();
    }
    private static void PoolSegments()
    {
        if (segment == null)
            segment = new List<Transform>();
        for(int x = 0; x< DataHandler.BudgetCategories.Count; ++x)
        {
            segment.Add(Instantiate(segmentPrefab));
            segment[x].gameObject.SetActive(false);
        }
    }

}
