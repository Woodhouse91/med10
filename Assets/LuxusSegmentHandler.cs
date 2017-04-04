using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuxusSegmentHandler : MonoBehaviour {
    private static Vector3 offset = new Vector3(0, 0, .175f);
    private static Vector3 basePos = Vector3.forward * -.56f;
    private static Transform segmentPrefab;
    private static Transform holder, nHolder;
    private static List<Transform> segment;
    private static List<Transform> activeSegments;
    private static float RotAngle = 55f;
    private static float slideTime = 2f;
    private static float rotTime = 1.5f;
    private float moneyMoveTime = 2f;
    private float moneyH = 0.072f;
    private bool stillMoving;
    private static List<List<Transform>> data;

    // Use this for initialization
    private void Start ()
    {
        data = new List<List<Transform>>();
        activeSegments = new List<Transform>();
        StaticStart();
        EventManager.OnBoxAtTable += GenerateTable;
        EventManager.OnRipTapeSliderDone += MoveMoneyToTable;
	}
    private static void StaticStart()
    {
        segmentPrefab = Resources.Load<Transform>("LuxusSegment");
        holder = GameObject.Find("Segmentholder").transform;
        EventManager.OnExcelDataLoaded += PoolSegments;
    }
    void Unsub()
    {
        try
        {
            EventManager.OnBoxAtTable -= GenerateTable;
            EventManager.OnRipTapeSliderDone -= MoveMoneyToTable;
            EventManager.OnExcelDataLoaded -= PoolSegments;
        }
        catch { }
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void MoveMoneyToTable()
    {
        Transform tar;
        Vector3 down;
        int i = 0;

    }

    private IEnumerator DoMovement(Transform obj, Transform tar)
    {
        while (stillMoving)
            yield return null;
        float t = 0;
        Vector3 orgPos = obj.position;
        while (t <= moneyMoveTime)
        {
            t += Time.deltaTime;
            obj.position = Vector3.Lerp(orgPos, tar.position, t/moneyMoveTime);
            yield return null;
        }
        yield break;
    }

    private void GenerateTable(BoxBehaviour s)
    {
        nHolder = Instantiate(holder);
        for (int x = 0; x < s.CategoryString.Count; ++x)
        {
            activeSegments.Add(segment[0]);
            segment.Remove(segment[0]);
            activeSegments[x].SetParent(holder);
            activeSegments[x].localPosition = offset * x;
            activeSegments[x].localRotation = Quaternion.Euler(90, -180, 0);
            activeSegments[x].GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMesh>().text = s.CategoryString[x];
            activeSegments[x].gameObject.SetActive(true);
            buildMoneyList(s.CategoryInt[x]);
        }
        print(data.Count);
        for(int x = 0; x<data.Count; ++x)
        {
            print(data[x].Count);
        }
        ExposeTable();
    }
    private void ExposeTable()
    {
        StartCoroutine(moveTable());
    }
    private IEnumerator moveTable()
    {
        stillMoving = true;
        Vector3 orgPos = holder.localPosition;
        Vector3 tarPos = orgPos + basePos-offset * (activeSegments.Count-1);
        print(tarPos);
        print(orgPos);
        Quaternion orgRot = holder.rotation;
        Quaternion tarRot = holder.rotation * Quaternion.AngleAxis(RotAngle, -Vector3.forward);
        float t;
        #region Slideout
        t = 0;
        while (t <= slideTime)
        {
            t += Time.deltaTime;
            holder.transform.localPosition = Vector3.Lerp(orgPos, tarPos, t / slideTime);
            yield return null;
        }
        #endregion
        #region Rotate
        t = 0;
        while(t<= rotTime)
        {
            t += Time.deltaTime;
            holder.rotation = Quaternion.Slerp(orgRot, tarRot, t / rotTime);
            yield return null;
        }
        #endregion
        stillMoving = false;
        yield break;
    }
    public static void ReleaseTable()
    {
        activeSegments.Clear();
        nHolder.SetParent(holder.parent);
        nHolder.localPosition = Vector3.zero;
        nHolder.localRotation = Quaternion.identity;


        holder = nHolder;
    }
    private static void buildMoneyList(int cat)
    {
        List<Transform> res = new List<Transform>();
        DataHandler.billRef[] myRef = DataHandler.BillsAtCategory_Month[cat];
        for(int x = 0; x<myRef.Length; ++x)
        {
            for (int f1 = 0; f1 < myRef[x]._1; ++f1)
            {
                res.Add(MoneyHolder.getCurrency(1));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f2 = 0; f2 < myRef[x]._2; ++f2)
            {
                res.Add(MoneyHolder.getCurrency(2));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f5 = 0; f5 < myRef[x]._5; ++f5)
            {
                res.Add(MoneyHolder.getCurrency(5));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f10 = 0; f10 < myRef[x]._10; ++f10)
            {
                res.Add(MoneyHolder.getCurrency(10));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f20 = 0; f20 < myRef[x]._20; ++f20)
            {
                res.Add(MoneyHolder.getCurrency(20));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f50 = 0; f50 < myRef[x]._50; ++f50)
            {
                res.Add(MoneyHolder.getCurrency(50));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f100 = 0; f100 < myRef[x]._100; ++f100)
            {
                res.Add(MoneyHolder.getCurrency(100));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f200 = 0; f200 < myRef[x]._200; ++f200)
            {
                res.Add(MoneyHolder.getCurrency(200));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f500 = 0; f500 < myRef[x]._500; ++f500)
            {
                res.Add(MoneyHolder.getCurrency(500));
            }
        }
        data.Add(res);
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
