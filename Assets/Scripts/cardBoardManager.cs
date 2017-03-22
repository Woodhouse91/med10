﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardBoardManager : MonoBehaviour {

    public List<GameObject> CardBoxList;
    Vector3 initialPos, nextRight,nextUp;
    public GameObject CardBoxPrefab;
    public Vector3 OffsetBoxToTable;
    public float TimeForBoxToTable;
    // Use this for initialization
	void Start () {
        EventManager.OnExcelDataLoaded += StartAfterLoad;
        EventManager.OnStartNextCategory += MoveBoxToTable;
	}

    private void MoveBoxToTable()
    {
        Transform NewBox = CardBoxList[EventManager.CurrentCategory].transform;
        NewBox.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(MoveBoxAnimation(NewBox));
    }

    private IEnumerator MoveBoxAnimation(Transform newBox)
    {
        Vector3 startPos = newBox.position;
        Quaternion startRot = newBox.rotation;
        float t = 1;
        while (t>0)
        {
            t += Time.deltaTime / TimeForBoxToTable;
            newBox.position = Vector3.Lerp(EventManager.Table.position+OffsetBoxToTable, startPos, t);
            newBox.rotation = Quaternion.Lerp(EventManager.Table.rotation * Quaternion.AngleAxis(180f,Vector3.up), startRot, t);
            yield return null;
        }
        newBox.GetComponent<Rigidbody>().isKinematic = false;
        EventManager.BoxAtTable();
    }

    void StartAfterLoad()
    {
        initialPos = new Vector3(1.5f, 0.28f, 0f); // starten af pyramiden i bunden til venstre
        nextRight = new Vector3(-0.55f, 0, 0); // den næste der skal stå til højre for den forrige kasse
        nextUp = new Vector3(-0.275f, 0.51f, 0f); // når man hopper en tak op til næste række
        CardBoxList = new List<GameObject>();
        CreateAllBoxes();
        
    }
	// Update is called once per frame
	void Update () {
		
	}
    void CreateAllBoxes()
    {
        int budgetCatCount = DataHandler.BudgetCategories.Count;
        //int budgetCatCount = 1; //test
        int i = 0;
        while(budgetCatCount > i)
        {
            budgetCatCount -= i;
            ++i;
        } // finds the correct number of rows

        budgetCatCount = DataHandler.BudgetCategories.Count;
        //budgetCatCount = 1; //test
        for (int h = 0; h < i; h++)
        {
            for (int w = 0; w < i - h; w++)
            {
                if (budgetCatCount <= 0) 
                    return;
                else
                {
                    GameObject box = Instantiate(CardBoxPrefab, transform);
                    box.transform.localPosition = initialPos + nextRight * w + nextUp * h;
                    box.transform.rotation = transform.rotation;
                    CardBoxList.Add(box);
                    PaintBox(box,budgetCatCount);
                    budgetCatCount--;
                }
            }
        }
    }
    void PaintBox(GameObject box, int category)
    {
        box.GetComponentInChildren<TextMesh>().text = DataHandler.BudgetCategories[category-1];
        int categoryInt = DataHandler.expenseData[0, category - 1];
        if (categoryInt == -1)
        {
            box.GetComponentInChildren<SpriteRenderer>().sprite = CategorySpriteHandler.GetSprite(-1);
        }
        else
        {
            box.GetComponentInChildren<SpriteRenderer>().sprite = CategorySpriteHandler.GetSprite(categoryInt);
        }
        //box.GetComponent<BoxBehaviour>().setCategory(DataHandler.expenseData[category, 0]);
    }
}
