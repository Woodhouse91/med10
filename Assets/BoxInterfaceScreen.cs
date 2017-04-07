using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxInterfaceScreen : MonoBehaviour {


    Transform tTitle, tFullTextField, tMask, tNextSlidePlz;
    Transform[] tTextField;
    flaggedInterfaceScreen Fis;
    bool categoryDoneBool = false;
    int enabledTextFields = 0;
    bool NowWeAreAtFlaggedCats = false;
    bool interactable = false;
    public bool isScrolling = false;
    int tTextfieldplaceInt;
	// Use this for initialization
	void Start () {
        Fis = transform.parent.GetComponentInChildren<flaggedInterfaceScreen>();
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
        StartCoroutine(FadeOutScreen());
    }

    private IEnumerator FadeOutScreen()
    {
        // FADE LORTET HER PLZ
        //Fis.FadeOutScreen();
        List<GameObject> markers = new List<GameObject>();
        markers.AddRange(GameObject.FindGameObjectsWithTag("Marker"));
        for (int i = 0; i < markers.Count; i++)
        {
            markers[i].SetActive(false);
        }
        
        List<CanvasRenderer> fadeList = new List<CanvasRenderer>();
        fadeList.AddRange(transform.parent.GetComponentsInChildren<CanvasRenderer>());
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
        tNextSlidePlz.transform.localPosition = Vector3.zero;
        categoryDoneBool = false;
       
        yield return null;
    }

    private void boxEmptied()
    {
        interactable = true;
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].GetComponent<Selectable>().enabled = interactable;
        }
        tNextSlidePlz.gameObject.SetActive(true);
    }

    private void boxAtTable(BoxBehaviour BB)
    {
        interactable = false;
        tTitle.gameObject.SetActive(true);
        tTitle.GetComponent<Text>().text = BB.GetComponentInChildren<TextMesh>().text;
        tMask.gameObject.SetActive(true);
        tFullTextField.gameObject.SetActive(true);
        enabledTextFields = BB.CategoryString.Count;
        tTextfieldplaceInt = enabledTextFields;
        if (tTextfieldplaceInt < 3)
            tTextfieldplaceInt = 3;
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].gameObject.SetActive(true);
            tTextField[i].GetComponentInChildren<Text>().text = BB.CategoryString[i];
            tTextField[i].GetComponent<Selectable>().enabled = interactable;
        }
        

    }
    public void SlideTextfieldLeft(Transform textfield, bool clicked)
    {
        float x = textfield.localPosition.x;
        if(x<-0.15f) // is slided left
        {
            Fis.ShowTable();
            if(clicked)
            {
                Fis.AddCategory(textfield.gameObject);
            }
        }
    }
    public void ClickTextField(int childIndex)
    {
        for (int i = 0; i < tTextField.Length; i++)
        {
            if(childIndex == i)
                tTextField[i].GetComponentInChildren<Selectable>().enabled = true;
            else
                tTextField[i].GetComponentInChildren<Selectable>().enabled = false;
        }
    }
    private void DisableHeleLortet() //CHILD DEPENDING
    {
        if(tTitle == null)
        {
            tTitle = transform.GetChild(0);
            tMask = transform.GetChild(1);
            tFullTextField = tMask.GetChild(0);
            tNextSlidePlz = transform.parent.GetChild(3); //SliderNextSlide
            tTextField = new Transform[tFullTextField.childCount];
        }
        for (int i = 0; i < tFullTextField.childCount; i++)
        {
            if(tTextField[i] == null)
                tTextField[i] = tFullTextField.GetChild(i);
            tTextField[i].gameObject.SetActive(false);
        }
        tMask.gameObject.SetActive(false);
        tTitle.gameObject.SetActive(false);
        tFullTextField.gameObject.SetActive(false);
        tNextSlidePlz.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
       
        if(tNextSlidePlz.transform.localPosition != transform.localPosition && categoryDoneBool == false)
        {
            if(tNextSlidePlz.localPosition.x < -0.55f)
            {
                categoryDoneBool = true;
                isScrolling = true;
                EventManager.CategoryDone();
            }
            if(tNextSlidePlz.localPosition.x > 0.55f)
            {
                NowWeAreAtFlaggedCats = true;
                isScrolling = true;
            }
            else
                transform.localPosition = tNextSlidePlz.localPosition;
        }
        
        if (!isScrolling)
        {
            for (int i = 0; i < enabledTextFields; i++)
            {
                tTextField[i].transform.localPosition -= Vector3.right * tTextField[i].transform.localPosition.x * 5f * Time.deltaTime;
                if (tTextField[i].transform.localPosition.x < 0.001f)
                {
                    tTextField[i].transform.localPosition = new Vector3(0f, tTextField[i].transform.localPosition.y,0f);
                }
            }
            if (tFullTextField.transform.localPosition.y < -0.0005f)
            {
                tFullTextField.transform.localPosition += Vector3.up * -tFullTextField.transform.localPosition.y * 10f * Time.deltaTime;
            }
            else if (tFullTextField.transform.localPosition.y > 0.2333333f*tTextfieldplaceInt-0.7f)
            {
                tFullTextField.transform.localPosition += Vector3.up * (0.2333333f * tTextfieldplaceInt - 0.7f - tFullTextField.transform.localPosition.y) * 10f * Time.deltaTime;
            }
            if(!NowWeAreAtFlaggedCats)
            {
                if (tNextSlidePlz.transform.localPosition.x + NowWeAreAtFlaggedCats.GetHashCode()*0.6f < -0.01f + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f)
                {
                    if (tNextSlidePlz.localPosition.x + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f > -0.55f + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f)
                    {
                        tNextSlidePlz.transform.localPosition -= Vector3.right * tNextSlidePlz.transform.localPosition.x * 10f * Time.deltaTime;
                    }
                    if (tNextSlidePlz.transform.localPosition.x + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f > -0.015f + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f)
                        tNextSlidePlz.transform.localPosition = Vector3.zero + Vector3.right * NowWeAreAtFlaggedCats.GetHashCode() * 0.6f;
                }
                if (tNextSlidePlz.transform.localPosition.x + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f > 0.01f + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f)
                {
                    if (tNextSlidePlz.localPosition.x + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f < 0.55f + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f)
                    {
                        tNextSlidePlz.transform.localPosition += Vector3.right * tNextSlidePlz.transform.localPosition.x * 10f * Time.deltaTime;
                    }
                    if (tNextSlidePlz.transform.localPosition.x + NowWeAreAtFlaggedCats.GetHashCode()*0.6f < 0.015f + NowWeAreAtFlaggedCats.GetHashCode() * 0.6f)
                        tNextSlidePlz.transform.localPosition = Vector3.zero + Vector3.right * NowWeAreAtFlaggedCats.GetHashCode() * 0.6f;
                }
            }
            else
            {

            }
        }
	}
}
