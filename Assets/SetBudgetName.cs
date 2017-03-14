using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBudgetName : MonoBehaviour {
    ExcelReader er;
    private void Start()
    {
        er = FindObjectOfType<ExcelReader>();
    }
    public void setName()
    {
        er.setExcelFile(transform.GetChild(0).GetComponent<Text>().text);
        transform.parent.gameObject.SetActive(false);
    }
}
