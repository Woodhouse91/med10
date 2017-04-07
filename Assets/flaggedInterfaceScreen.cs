using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flaggedInterfaceScreen : MonoBehaviour
{
    Transform tTitle, tFullTextField, tMask;
    Transform[] tTextField;
    List<String> flaggedCategories;

    int enabledTextFields = 0;
    float waitUntilFade = 0.1f;
    bool interactable = false;
    public bool isScrolling = false;
    public bool isShowing = true;
    int tTextfieldplaceInt;
    // Use this for initialization
    void Start()
    {
        flaggedCategories = new List<String>();
        //DisableHeleLortet();
    }

    public IEnumerator FadeOutFlaggedScreen()
    {
        // FADE LORTET HER PLZ
        if(isShowing)
        {
            isShowing = false;
            List<CanvasRenderer> fadeList = new List<CanvasRenderer>();
            fadeList.AddRange(GetComponentsInChildren<CanvasRenderer>());
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime;
                for (int i = 0; i < fadeList.Count; i++)
                {
                    fadeList[i].SetAlpha(1f - t);
                }
                yield return null;
            }
            DisableHeleLortet();
        }
        yield return null;
    }

    public void ShowTable()
    {
        if(!isShowing)
        {
            interactable = false;
            tTitle.gameObject.SetActive(true);
       
            tMask.gameObject.SetActive(true);
            tFullTextField.gameObject.SetActive(true);
            enabledTextFields = flaggedCategories.Count;
            tTextfieldplaceInt = enabledTextFields;
            if (tTextfieldplaceInt < 3)
                tTextfieldplaceInt = 3;
            for (int i = 0; i < enabledTextFields; i++)
            {
                tTextField[i].gameObject.SetActive(true);
                tTextField[i].GetComponentInChildren<Text>().text = flaggedCategories[i];
                tTextField[i].GetComponent<Selectable>().enabled = interactable;
            }
           
        }
        waitUntilFade = 3f;
        isShowing = true;
    }

    public void AddCategory(GameObject textfield)
    {
        if(!flaggedCategories.Contains(textfield.GetComponentInChildren<Text>().text))
        {
            flaggedCategories.Add(textfield.GetComponentInChildren<Text>().text);
            tTextField[flaggedCategories.Count - 1].GetComponentInChildren<Text>().text = flaggedCategories[flaggedCategories.Count - 1];
            //Update the textfields as ShowTable()
            enabledTextFields = flaggedCategories.Count;
            tTextfieldplaceInt = enabledTextFields;
            if (tTextfieldplaceInt < 3)
                tTextfieldplaceInt = 3;
            for (int i = 0; i < enabledTextFields; i++)
            {
                tTextField[i].gameObject.SetActive(true);
                tTextField[i].GetComponentInChildren<Text>().text = flaggedCategories[i];
                tTextField[i].GetComponent<Selectable>().enabled = interactable;
            }
        }

    }

    public void DisableHeleLortet() //CHILD DEPENDING
    {
        if (tTitle == null)
        {
            tTitle = transform.GetChild(0);
            tMask = transform.GetChild(1);
            tFullTextField = tMask.GetChild(0);
            tTextField = new Transform[tFullTextField.childCount];
        }
        for (int i = 0; i < tFullTextField.childCount; i++)
        {
            if (tTextField[i] == null)
                tTextField[i] = tFullTextField.GetChild(i);
            tTextField[i].gameObject.SetActive(false);
        }
        tMask.gameObject.SetActive(false);
        tTitle.gameObject.SetActive(false);
        tFullTextField.gameObject.SetActive(false);
        isShowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isScrolling)
        {
            if (tFullTextField.transform.localPosition.y < -0.0005f)
            {
                tFullTextField.transform.localPosition += Vector3.up * -tFullTextField.transform.localPosition.y * 10f * Time.deltaTime;
            }
            else if (tFullTextField.transform.localPosition.y > 0.2333333f * tTextfieldplaceInt - 0.7f)
            {
                tFullTextField.transform.localPosition += Vector3.up * (0.2333333f * tTextfieldplaceInt - 0.7f - tFullTextField.transform.localPosition.y) * 10f * Time.deltaTime;
            }
        }
        if(waitUntilFade>0)
        {
            waitUntilFade -= Time.deltaTime;
            if (waitUntilFade <= 0)
                StartCoroutine(FadeOutFlaggedScreen());
        }

    }
}
