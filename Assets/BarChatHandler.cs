using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChatHandler : MonoBehaviour {
    private Transform bar;
    private int maxAmount;
    private int[][] differences;
    private float[] prevScales = new float[12];
    private float scaleTime = 1.5f;
    private Material negMat;
	// Use this for initialization
	void Start () {
        bar = GameObject.Find("Barchartholder").transform;
        negMat = Resources.Load<Material>("negbarmat");
        print(negMat);
        EventManager.OnExcelDataLoaded += generateData;
        EventManager.OnBoxAtTable += updateTable;
        EventManager.OnCategoryDone += updatePrevious;
	}

    private void updatePrevious()
    {
        for (int x = 0; x < 12; ++x)
        {
            StartCoroutine(scale(bar.GetChild(1).GetChild(x + 12), prevScales[x], true));
        }
    }

    private void updateTable(BoxBehaviour s)
    {
        for(int x = 0; x<s.moneyAtCrate.Length; ++x)
        {
            prevScales[x] = getScale(s.moneyAtCrate[x]);
            StartCoroutine(scale(bar.GetChild(1).GetChild(x), prevScales[x], false));
        }
    }

    private void generateData()
    {
        maxAmount = 0;
        for(int x = 0; x<bar.GetChild(0).childCount; ++x)
        {
            if (x + DataHandler.startMonth > 11)
                bar.GetChild(0).GetChild(x).GetComponent<TextMesh>().text = getMonth(x + DataHandler.startMonth - 12);
            else
                bar.GetChild(0).GetChild(x).GetComponent<TextMesh>().text = getMonth(x + DataHandler.startMonth);
            if (maxAmount < Mathf.Abs(DataHandler.monthlyDifference[x]))
                maxAmount = Mathf.Abs(DataHandler.monthlyDifference[x]);
        }
        for(int x = 0; x<11; ++x)
        {
            if (maxAmount < DataHandler.incomeData[x])
                maxAmount = DataHandler.incomeData[x];
        }
        if(maxAmount%500!=0)
            maxAmount += 500 - maxAmount % 500;
        bar.GetChild(2).GetComponent<TextMesh>().text = FormatHandler.FormatCurrency(maxAmount);
        bar.GetChild(3).GetComponent<TextMesh>().text = FormatHandler.FormatCurrency(maxAmount / 2);
        bar.GetChild(4).GetComponent<TextMesh>().text = FormatHandler.FormatCurrency(-maxAmount);
        bar.GetChild(5).GetComponent<TextMesh>().text = FormatHandler.FormatCurrency(-(maxAmount / 2));
        for(int x = 0; x<12; ++x)
        {
            bar.GetChild(1).GetChild(x).localScale = new Vector3(1, getScale(DataHandler.incomeData[x]), 1);
            bar.GetChild(1).GetChild(x+12).localScale = new Vector3(1, getScale(DataHandler.incomeData[x]), 1);
        }
    }
    private float getScale(float val)
    {
        return val / maxAmount;
    }
    private IEnumerator scale(Transform obj, float s, bool prev)
    {
        if (s == 0)
            yield break;
        float t = 0f;
        float org = obj.localScale.y;
        while (t < 1)
        {
            t += Time.deltaTime / scaleTime;
            if (t > 1)
                t = 1;
            obj.localScale = new Vector3(1, org -s * t, 1);
            if (obj.localScale.y < 0 && !prev)
                obj.GetChild(0).GetComponent<MeshRenderer>().material = negMat;
            yield return null;
        }
        yield break;
    }
    private string getMonth(int m)
    {
        if (m == 0)
            return "Jan";
        else if (m == 1)
            return "Feb";
        else if (m == 2)
            return "Mar";
        else if (m == 3)
            return "Apr";
        else if (m == 4)
            return "Maj";
        else if (m == 5)
            return "Jun";
        else if (m == 6)
            return "Jul";
        else if (m == 7)
            return "Aug";
        else if (m == 8)
            return "Sep";
        else if (m == 9)
            return "Okt";
        else if (m == 10)
            return "Nov";
        else
            return "Dec";
    }
}
