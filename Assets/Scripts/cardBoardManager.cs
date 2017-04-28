using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardBoardManager : MonoBehaviour {

    public List<GameObject> CardBoxList;
    Vector3 initialPos, nextRight,nextUp;
    public GameObject CardBoxPrefab;
    public AnimationCurve AC;
    public Vector3 OffsetBoxToTable;
    public float TimeForBoxToTable;
    // Use this for initialization
	void Start () {
        EventManager.OnExcelDataLoaded += StartAfterLoad;
        EventManager.OnStartNextCategory += MoveBoxToTable;
	}

    private void Unsub()
    {
        EventManager.OnExcelDataLoaded -= StartAfterLoad;
        EventManager.OnStartNextCategory -= MoveBoxToTable;
    }
    private void MoveBoxToTable()
    {
        StartCoroutine(MoveBoxAnimation(CardBoxList[EventManager.CurrentCategory].transform));
    }

    private IEnumerator MoveBoxAnimation(Transform newBox)
    {
        newBox.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 startPos = newBox.position;
        Quaternion startRot = newBox.rotation;
        float t = 1;
        while (t>0)
        {
            t -= Time.deltaTime / TimeForBoxToTable;
            newBox.position = Vector3.Lerp(EventManager.Table.position+OffsetBoxToTable, startPos, t);
            newBox.position += Vector3.up * AC.Evaluate(t);
            newBox.rotation = Quaternion.Lerp(EventManager.Table.rotation * Quaternion.AngleAxis(180f,Vector3.up), startRot, t);
            yield return null;
        }
        newBox.GetComponent<Rigidbody>().isKinematic = false;
        EventManager.BoxAtTable(newBox.GetComponent<BoxBehaviour>());
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
        List<int> differentCats = new List<int>();
        for (int k = 0; k < budgetCatCount; k++)
        {
            if(!differentCats.Contains(DataHandler.expenseData[0, k]))
                differentCats.Add(DataHandler.expenseData[0, k]);
        }
        //int budgetCatCount = 1; //test
        int i = 0;
        budgetCatCount = differentCats.Count;
        DataHandler.tCombinedCategories = budgetCatCount;
        StartCoroutine(FindObjectOfType<PlaceAllCrates>().PlaceAllRows(budgetCatCount));
        while (budgetCatCount > i)
        {
            budgetCatCount -= i;
            ++i;
        } // finds the correct number of rows

        budgetCatCount = differentCats.Count;
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
                    CardBoxList.Insert(0, box);
                    PaintBox(box,differentCats[differentCats.Count-budgetCatCount]);
                    budgetCatCount--;
                }
            }
        }
    }
    void PaintBox(GameObject box, int category)
    {
        box.GetComponentInChildren<TextMesh>().text = FormatHandler.FormatCategory(CategoryDatabase.GetName(category)); // her skal være text
        box.GetComponentInChildren<SpriteRenderer>().sprite = CategorySpriteHandler.GetAt(category);

        //her fylder vi dens liste
        box.GetComponent<BoxBehaviour>().modelsForShelves(category);
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
}
