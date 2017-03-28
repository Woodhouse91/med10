using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBudgetName : MonoBehaviour {
    public void setName()
    {
        DataAppLauncher.LaunchApplication(transform.GetChild(0).GetComponent<Text>().text);
        transform.parent.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            setName();
    }
}
