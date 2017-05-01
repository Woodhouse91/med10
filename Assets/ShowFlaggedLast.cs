using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlaggedLast : MonoBehaviour {

    Canvas canvas;
    List<int> flaggedList;
    GameObject TextField;
	// Use this for initialization
	void Start () {
        canvas = GetComponentInParent<Canvas>();
        canvas.enabled = false;
        TextField = transform.GetChild(0).gameObject;
        transform.parent.GetChild(0).GetComponent<Image>().enabled = false;
        transform.parent.GetChild(1).GetComponent<Text>().enabled = false;
        EventManager.OnCategoryFinished += LastStart;	
	}	
    void LastStart()
    {
        canvas.enabled = true;
        flaggedList = FindObjectOfType<BoxInterfaceScreen>().FlaggedItem;
        if (flaggedList.Count == 0)
            return;
        transform.parent.GetChild(0).GetComponent<Image>().enabled = true;
        transform.parent.GetChild(1).GetComponent<Text>().enabled = true;
        TextField.GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[flaggedList[0]]);
        for (int i = 1; i < flaggedList.Count; i++)
        {
            GameObject tf = Instantiate(TextField, transform);
            tf.GetComponentInChildren<Text>().text = FormatHandler.FormatCategory(DataHandler.BudgetCategories[flaggedList[i]]);
            if(i>7)
                canvas.transform.localPosition += Vector3.up * 0.12f * 0.34f;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}

}
