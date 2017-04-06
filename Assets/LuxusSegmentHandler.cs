﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuxusSegmentHandler : MonoBehaviour {
    private static Vector3 offset = new Vector3(0, 0, .175f);
    private static float scaledOffset = 0;
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
    private float moneyW = 0.165f;
    private float coinS = 0.00285f;
    private float coinsPerCol = 5;
    private int billsPerColumn = 25;
    private float hangHeight = 0.2f;
    private float hangTime = 2f;
    private bool stillMoving;
    private static List<List<Transform>> bills;
    private static List<List<Transform>> coins;
    private Transform targetWall;
    private float luxusOffset;


    // Use this for initialization
    private void Start ()
    {
        targetWall = GameObject.Find("Tavlefar").transform;
        bills = new List<List<Transform>>();
        coins = new List<List<Transform>>();
        activeSegments = new List<Transform>();
        EventManager.OnBoxAtTable += GenerateTable;
        EventManager.OnRipTapeSliderDone += MoveMoneyToTable;
        segmentPrefab = Resources.Load<Transform>("LuxusSegment");
        holder = GameObject.Find("Segmentholder").transform;
        EventManager.OnExcelDataLoaded += PoolSegments;
        EventManager.OnCategoryDone += ReleaseTable;
	}
    void Unsub()
    {
        try
        {
            EventManager.OnBoxAtTable -= GenerateTable;
            EventManager.OnRipTapeSliderDone -= MoveMoneyToTable;
            EventManager.OnExcelDataLoaded -= PoolSegments;
            EventManager.OnCategoryDone -= ReleaseTable;
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
        for(int x = 0; x<activeSegments.Count; ++x)
        {
            List<float> offset = new List<float>();
            tar = activeSegments[x].GetChild(1).GetChild(0);
            float down = -moneyH/5f;
            int cols = bills[x].Count / billsPerColumn+1;
            int downOffset = 0;

            float coinRight = coinS;
            float coinDown = down;

            int coinDownCount = 0;
            int coinColCount = 0;

            for (int k = 0; k<cols; ++k)
            {
                offset.Add((moneyW * (((-cols/2f) +k)+.5f)));
            }
            if (x < bills.Count)
            {
                for (int y = 0; y < bills[x].Count; ++y)
                {
                    if ((y+1) % (cols+1) == 0)
                        ++downOffset;
                    StartCoroutine(DoMovement(bills[x][y], tar, down * downOffset, offset[y % offset.Count]));
                }
            }
            if (x < coins.Count)
            {
                for(int y = 0; y<coins[x].Count; ++y)
                {
                    StartCoroutine(doCoinMovement(coins[x][y], tar, coinDown*coinDownCount, coinRight*coinColCount));
                    coinColCount++;
                    if ((y+1) % (coinsPerCol+1) == 0)
                    {
                        coinColCount = 0;
                        coinDownCount++;
                    }
                }
            }
        }
    }

    private IEnumerator doCoinMovement(Transform obj, Transform tar, float downOff, float rightOff)
    {
        while (stillMoving)
            yield return null;
        float t = 0;
        Vector3 orgPos = obj.position;
        Vector3 target = tar.position + tar.up * downOff + tar.right * rightOff;
        Quaternion orgRot = obj.rotation;
        Quaternion tarRot = tar.rotation * Quaternion.AngleAxis(15, -Vector3.forward);
        while (t <= 1)
        {
            t += Time.deltaTime / moneyMoveTime;
            obj.position = Vector3.Lerp(orgPos, target, t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            yield return null;
        }
         obj.gameObject.AddComponent<ChildTo>();
        obj.GetComponent<ChildTo>().Initiate(tar);
        yield break;
    }

    private IEnumerator DoMovement(Transform obj, Transform tar, float downOff, float colOff)
    {
        while (stillMoving)
            yield return null;
        float t = 0;
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        Quaternion tarRot = tar.rotation*Quaternion.AngleAxis(15, -Vector3.right);
        Vector3 target = tar.position + tar.up * downOff + tar.right * colOff;
        while (t <= 1)
        {
            t += Time.deltaTime/moneyMoveTime;
            obj.position = Vector3.Lerp(orgPos, target, t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            yield return null;
        }
        obj.gameObject.AddComponent<ChildTo>();
        obj.GetComponent<ChildTo>().Initiate(tar);
        yield break;
    }
    private void GenerateTable(BoxBehaviour s)
    {
        nHolder = Instantiate(holder);
        int scaleFactor = 0;
        float prevScaled = 0;
        bool scaled = false;
        for (int x = 0; x < s.CategoryString.Count; ++x)
        {
            activeSegments.Add(segment[0]);
            segment.Remove(segment[0]);
            activeSegments[x].SetParent(holder);
            scaleFactor = buildMoneyList(s.CategoryInt[x])/billsPerColumn;
            activeSegments[x].localPosition = (offset * x) + offset * scaledOffset + offset * (scaleFactor/2f);
            scaledOffset += (scaleFactor);
            activeSegments[x].localScale += Vector3.up*scaleFactor;
            activeSegments[x].localRotation = Quaternion.Euler(90, -180, 0);
            activeSegments[x].GetChild(0).GetChild(0).GetComponent<TextMesh>().text = s.CategoryString[x];
            activeSegments[x].GetChild(0).GetChild(1).GetComponent<TextMesh>().text = formatCurrency(DataHandler.expenseData[13, s.CategoryInt[x]]);
            Vector3 texScale = activeSegments[x].GetChild(0).GetChild(0).localScale;
            texScale.x /= (scaleFactor+1);
            activeSegments[x].GetChild(0).GetChild(0).localScale = texScale;
            activeSegments[x].GetChild(0).GetChild(1).localScale = texScale;
            activeSegments[x].gameObject.SetActive(true);
            prevScaled = scaleFactor;
        }
        scaledOffset += scaled ? .5f : 0;
        ExposeTable();
    }
    private string formatCurrency(int val)
    {
        decimal moneyvalue = val;
        string res = String.Format("{0:N}", moneyvalue);
        res = res.Remove(res.Length - 3);
        res = res.Replace(",", ".");
        res += ",-";
        return res;
    }
    private void ExposeTable()
    {
        StartCoroutine(moveTable());
    }
    private IEnumerator moveTable()
    {
        stillMoving = true;
        Vector3 orgPos = holder.localPosition;
        Vector3 tarPos = orgPos + basePos-offset * (activeSegments.Count-1)-offset*scaledOffset;
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
    public void ReleaseTable()
    {
        activeSegments.Clear();
        nHolder.SetParent(holder.parent);
        nHolder.localPosition = Vector3.zero;
        nHolder.localRotation = Quaternion.identity;
        StartCoroutine(doRelease(holder));
        holder = nHolder;
    }

    private IEnumerator doRelease(Transform obj)
    {
        Transform myRef = obj;
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        Vector3 target = targetWall.position - targetWall.right * (4.5f-luxusOffset) + targetWall.forward*0.02f + targetWall.up*hangHeight;
        luxusOffset = scaledOffset;
        if (luxusOffset > 10f)
        {
            luxusOffset = 0;
            hangHeight = -.95f;
            target = targetWall.position - targetWall.right * (4.5f - luxusOffset) + targetWall.forward * 0.02f + targetWall.up * hangHeight;
        }
        Quaternion tarRot = targetWall.rotation;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / hangTime;
            obj.position = Vector3.Lerp(orgPos, target, t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            yield return null;
        }
        for(int x = 0; x<coins.Count; ++x)
        {
            for(int y = 0; y<coins[x].Count; ++y)
            {
                Destroy(coins[x][y].GetComponent<ChildTo>());
            }
        }
        for(int x = 0; x<bills.Count; ++x)
        {
            for(int y = 0; y<bills[x].Count; ++y)
            {
                Destroy(bills[x][y].GetComponent<ChildTo>());
            }
        }
        yield break;
    }
    private static int buildMoneyList(int cat)
    {
        List<Transform> resBills = new List<Transform>();
        List<Transform> resCoins = new List<Transform>();
        DataHandler.billRef[] myRef = DataHandler.BillsAtCategory_Month[cat];
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f1000 = 0; f1000 < myRef[x]._1000; ++f1000)
            {
                resBills.Add(MoneyHolder.getCurrency(1000));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f500 = 0; f500 < myRef[x]._500; ++f500)
            {
                resBills.Add(MoneyHolder.getCurrency(500));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f200 = 0; f200 < myRef[x]._200; ++f200)
            {
                resBills.Add(MoneyHolder.getCurrency(200));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f100 = 0; f100 < myRef[x]._100; ++f100)
            {
                resBills.Add(MoneyHolder.getCurrency(100));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f50 = 0; f50 < myRef[x]._50; ++f50)
            {
                resBills.Add(MoneyHolder.getCurrency(50));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f20 = 0; f20 < myRef[x]._20; ++f20)
            {
                resCoins.Add(MoneyHolder.getCurrency(20));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f10 = 0; f10 < myRef[x]._10; ++f10)
            {
                resCoins.Add(MoneyHolder.getCurrency(10));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f5 = 0; f5 < myRef[x]._5; ++f5)
            {
                resCoins.Add(MoneyHolder.getCurrency(5));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f2 = 0; f2 < myRef[x]._2; ++f2)
            {
                resCoins.Add(MoneyHolder.getCurrency(2));
            }
        }
        for(int x = 0; x<myRef.Length; ++x)
        {
            for (int f1 = 0; f1 < myRef[x]._1; ++f1)
            {
                resCoins.Add(MoneyHolder.getCurrency(1));
            }
        }
        if(resCoins.Count>0)
            coins.Add(resCoins);
        if(resBills.Count>0)
            bills.Add(resBills);
        return resBills.Count;
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
