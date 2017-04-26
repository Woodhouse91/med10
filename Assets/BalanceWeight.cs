using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceWeight : MonoBehaviour {

    GameObject måler;
    GameObject pil;
    public Material red, green;

    float totalIncome;
    float curExpence = 0;
    float totalAngle = 24;
    int totalCategories = 0;
    float curAngle;
	// Use this for initialization
	void Start () {
       
        green = GetComponent<MeshRenderer>().material;
        red = GameObject.Find("CylinderRød").GetComponent<MeshRenderer>().material;
        måler = GameObject.Find("måler");
        pil = GameObject.Find("Pil");
       
        EventManager.OnBoxEmptied += BoxEmptied;
        EventManager.OnBoxAtTable += BoxAtTable;
        EventManager.OnRipTapeSliderDone += FirstBoxOpened;
        
    }
    private void Unsub()
    {
        EventManager.OnBoxEmptied -= BoxEmptied;
        EventManager.OnBoxAtTable -= BoxAtTable;
        EventManager.OnRipTapeSliderDone -= FirstBoxOpened;
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }
    // Update is called once per frame
    void Update () {
		
	}
    void FirstBoxOpened()
    {
        if (totalCategories == 0)
        {
            totalIncome = DataHandler.tIncome > DataHandler.tExpense ? DataHandler.tIncome : DataHandler.tExpense;
            green.SetColor("_EmissionColor", Color.green * 1.0f); // later
            red.SetColor("_EmissionColor", Color.red * 0.0f);
            GameObject.Find("venstreGruppeSkål").AddComponent<ChildTo>().Initiate(GameObject.Find("venstreVip").transform);
            GameObject.Find("højreGruppeSkål").AddComponent<ChildTo>().Initiate(GameObject.Find("højreVip").transform);
            totalCategories = FindObjectOfType<cardBoardManager>().CardBoxList.Count;
            CreateBagOfMoney(-1337);
            curAngle = ((curExpence / totalIncome) - 1f) * totalAngle; // TOTAL INCOME SKAL VÆRE DET BARCHARTS FINDER I STEDET (nico pico giver)
            RefreshWeight();
        }
    }
    void BoxAtTable(BoxBehaviour BB)
    {
        float categoryTotal = 0;
        for (int i = 0; i < BB.moneyAtCrate.Length; i++)
        {
            categoryTotal += BB.moneyAtCrate[i];
        }
        curExpence += categoryTotal;
        CreateBagOfMoney(categoryTotal);
    }
    void BoxEmptied()
    {
        //FOR inkomst pengene 
        curAngle = ((curExpence / totalIncome) - 1f) * totalAngle; 
        DropBagOfMoney();
        RefreshWeight();
    }
    void RefreshWeight()
    {
        StartCoroutine(RotateScaleSlow());
        if (curAngle > 0)
        {
            green.SetColor("_EmissionColor", Color.green * 0.0f); 
            red.SetColor("_EmissionColor", Color.red * 1.0f);
        }
    }

    private IEnumerator RotateScaleSlow()
    {
        Quaternion rot = måler.transform.localRotation;
        Quaternion pilRot = pil.transform.localRotation;
        Quaternion newPilRot = Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(-curAngle, Vector3.up);
        Quaternion newRot = Quaternion.AngleAxis(-curAngle, Vector3.up);
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            pil.transform.localRotation = Quaternion.Slerp(pilRot, newPilRot, t);
            måler.transform.localRotation = Quaternion.Slerp(rot, newRot, t);
            yield return null;
        }
        yield return null;
    }

    void CreateBagOfMoney(float size)
    {
        if(size == -1337) //Create bags of money for income
        {

            return;
        }
        print("size: " + size);
        print("size of bag for balance: " + (size / totalIncome) / totalCategories);
    }
    void DropBagOfMoney()
    {
        print("drop it, BZBZBZBZBZZ");
    }
}
