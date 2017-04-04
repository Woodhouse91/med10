using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxInterfaceScreen : MonoBehaviour {


    Transform tTitle, tFullTextField;
    Transform[] tTextField;

    int enabledTextFields = 0;
    bool interactable = false;

	// Use this for initialization
	void Start () {
        DisableHeleLortet();
        EventManager.OnBoxAtTable += boxAtTable;
        EventManager.OnBoxEmptied += boxEmptied;
        EventManager.OnCategoryDone += categoryDone;
	}

    private void Unsub()
    {
        EventManager.OnBoxAtTable -= boxAtTable;
        EventManager.OnBoxEmptied -= boxEmptied;
        EventManager.OnCategoryDone -= categoryDone;
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


    private void categoryDone()
    {
        DisableHeleLortet();
    }

    private void boxEmptied()
    {
        interactable = true;
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].GetComponent<Selectable>().enabled = interactable;
        }
    }

    private void boxAtTable(BoxBehaviour BB)
    {
        interactable = false;
        tTitle.gameObject.SetActive(true);
        tTitle.GetComponent<Text>().text = BB.GetComponentInChildren<TextMesh>().text;
        tFullTextField.gameObject.SetActive(true);
        enabledTextFields = BB.CategoryString.Count;
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].gameObject.SetActive(true);
            tTextField[i].GetComponentInChildren<Text>().text = BB.CategoryString[i];
            tTextField[i].GetComponent<Selectable>().enabled = interactable;
        }
        

    }

    private void DisableHeleLortet()
    {
        if(tTitle == null)
        {
            tTitle = transform.GetChild(0);
            tFullTextField = transform.GetChild(1);
            tTextField = new Transform[tFullTextField.childCount];
        }
        for (int i = 0; i < tFullTextField.childCount; i++)
        {
            if(tTextField[i] == null)
                tTextField[i] = tFullTextField.GetChild(i);
            tTextField[i].gameObject.SetActive(false);
        }
        tTitle.gameObject.SetActive(false);
        tFullTextField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
