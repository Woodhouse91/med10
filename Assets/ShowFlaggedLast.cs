using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlaggedLast : MonoBehaviour {

    Canvas canvas;
    List<int> flaggedList;
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
        flaggedList = FindObjectOfType<BoxInterfaceScreen>().FlaggedItem;
        if (flaggedList.Count == 0)
            return;
        TextField.GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[flaggedList[0]]);
        for (int i = 1; i < flaggedList.Count; i++)
        {
            GameObject tf = Instantiate(TextField, transform);
            tf.GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[flaggedList[i]]);
            if(i>7)
                canvas.transform.localPosition += Vector3.up * 0.12f * 0.34f;
        }
    }
    public void RefreshFlaggedList()
    {
        List<int> newFlaggedList = FindObjectOfType<BoxInterfaceScreen>().FlaggedItem;
        int difference = newFlaggedList.Count - flaggedList.Count;
        //difference = +1 hvis der kommer en ny på listen eller -1 hvis der bliver fjernet en.
        if (difference > 0)
        {
            for (int i = transform.childCount; i < newFlaggedList.Count; i++)
            {
                GameObject tf = Instantiate(TextField, transform);
            }
        }
        else if (difference < 0)
        {
            for (int i = transform.childCount; i > newFlaggedList.Count; i--)
            {
                Destroy(transform.GetChild(transform.childCount - 1));
            }
        }
        else
            return;
        canvas.transform.localPosition = StartPos;
        flaggedList = newFlaggedList;
        for (int i = 0; i < flaggedList.Count; i++)
        {
            GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[flaggedList[i]]);
            if (i > 7)
                canvas.transform.localPosition += Vector3.up * 0.12f * 0.34f;
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
