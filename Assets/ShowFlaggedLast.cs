using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlaggedLast : MonoBehaviour {

    Canvas canvas;
    GameObject TextField;
    Vector3 StartPos;
    // Use this for initialization
    void Start () {
        canvas = GetComponentInParent<Canvas>();
        canvas.enabled = false;
        StartPos = canvas.transform.localPosition;
        TextField = transform.GetChild(0).gameObject;
        EventManager.OnCategoryFinished += LastStart;	
	}	
    void LastStart()
    {
        StartCoroutine(DeactivateActivate());
        List<int> flaggedList = FindObjectOfType<BoxInterfaceScreen>().FlaggedItem;
        if (flaggedList.Count == 0)
        {
            TextField.GetComponentInChildren<Text>().text = "Ingen markerede";
            return;
        }
        for (int i = 0; i < flaggedList.Count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[flaggedList[i]]);
            if(i>7)
                canvas.transform.localPosition += Vector3.up * 0.12f * 0.34f;
        }
    }
    public void RefreshFlaggedList(List<int> newFlaggedList)
    {
        int difference = newFlaggedList.Count - transform.childCount;
        if(newFlaggedList.Count == 0)
        {
            transform.GetChild(0).GetComponentInChildren<Text>().text = "Ingen markerede";
            return;
        }
       
        canvas.transform.localPosition = StartPos;
       
        for (int i = 0; i < newFlaggedList.Count; i++)
        {
            print(newFlaggedList[i]);
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[newFlaggedList[i]]);
            if (i > 7)
                canvas.transform.localPosition += Vector3.up * 0.12f * 0.34f;
        }
        for (int i = newFlaggedList.Count; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }


    IEnumerator DeactivateActivate()
    {
        yield return new WaitForEndOfFrame();
        canvas.enabled = true;
        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        canvas.enabled = false;
        transform.parent.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        canvas.enabled = true;
        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update () {
		
	}

}
