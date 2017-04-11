using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxInterfaceScreen : MonoBehaviour {


    Transform tTitle, tFullTextField, tMask, tNextSlidePlz, tSlider;
    Transform[] tTextField;
    public List<int> FlaggedItem;
    Vector3 ReturnToPos;
    public Sprite Flagged, Unflagged;
    BoxBehaviour BB;
    PlaceAllCrates pac;
    bool categoryDoneBool = false;
    int enabledTextFields = 0;
    bool interactable = false;
    public bool isScrolling = false;
    int tTextfieldplaceInt;
    LuxusSegmentHandler LSH;
    private int baseFontSize =  100;
    private int stringLengthCutoff = 15;
    
	// Use this for initialization
	void Start () {
        pac = FindObjectOfType<PlaceAllCrates>();
        FlaggedItem = new List<int>();
        DisableHeleLortet();
        ReturnToPos = Vector3.zero; // START POSITION
        EventManager.OnBoxAtTable += boxAtTable;
        EventManager.OnBoxEmptied += boxEmptied;
        EventManager.OnMoneyInstantiated += MoneyInstantiated;
        EventManager.OnCategoryDone += categoryDone;
	}

    private void Unsub()
    {
        EventManager.OnBoxAtTable -= boxAtTable;
        EventManager.OnBoxEmptied -= boxEmptied;
        EventManager.OnCategoryDone -= categoryDone;
        EventManager.OnMoneyInstantiated -= MoneyInstantiated;
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

    private void MoneyInstantiated()
    {
        tSlider.gameObject.SetActive(true);
        BB = GameObject.Find("Table").GetComponentInChildren<BoxBehaviour>();
    }
    private void categoryDone()
    {
        StartCoroutine(FadeOutScreen());
        
    }
    public void FlagIt(Transform target)
    {
        int flagNum = FindTransform(target);
        LuxusSegmentHandler.FlagCategory(flagNum);
        if (!FlaggedItem.Contains(BB.CategoryInt[flagNum]))
            FlaggedItem.Add(BB.CategoryInt[flagNum]);
        else
            FlaggedItem.Remove(BB.CategoryInt[flagNum]);
        pac.FlagCategory(BB.CategoryInt[flagNum]);
        UpdateImages();
    }
    private void UpdateImages()
    {
        
        for (int i = 0; i < tTextField.Length; i++)
        {
            for (int j = 0; j < FlaggedItem.Count; j++)
            {
                if (tTextField[i].GetChild(1).GetComponent<Text>().text == FormatHandler.FormatCategory(DataHandler.BudgetCategories[FlaggedItem[j]]))
                {
                    tTextField[i].GetChild(0).GetComponent<Image>().sprite = Flagged;
                    break;
                }
                else
                    tTextField[i].GetChild(0).GetComponent<Image>().sprite = Unflagged;
            }
            if(FlaggedItem.Count == 0)
            {
                tTextField[i].GetChild(0).GetComponent<Image>().sprite = Unflagged;
            }
        }
    }
    public int FindTransform(Transform searching)
    {
        for (int i = 0; i < tTextField.Length; i++)
        {
            if (searching.GetInstanceID() == tTextField[i].GetInstanceID())
                return i;
        }
        Debug.LogError("Could not find the correct transform, best regards Kris");
        return -100;
    }
    private IEnumerator FadeOutScreen()
    {
        // FADE LORTET HER PLZ
        //Fis.FadeOutScreen();
        EventManager.DisableAllMarkers();
        
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
        StartCoroutine(waitForScaleOfModels());
    }

    private IEnumerator waitForScaleOfModels()
    {
        yield return new WaitForSeconds(EventManager.scaleTime); 
        interactable = true;
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].GetComponent<Selectable>().enabled = interactable;
        }
        tNextSlidePlz.gameObject.SetActive(true);


    }

    private void boxAtTable(BoxBehaviour BB)
    {
        this.BB = BB;
        interactable = false;
        tTitle.gameObject.SetActive(true);
        tSlider.gameObject.SetActive(true);
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
            stringSize(tTextField[i].GetComponentInChildren<Text>(), FormatHandler.FormatCategory(BB.CategoryString[i]));
            tTextField[i].GetComponent<Selectable>().enabled = interactable;
        }
        UpdateImages();
    }
    private void stringSize(Text t, String s)
    {
        t.text = FormatHandler.FormatCategory(s);
        if (s.Length > stringLengthCutoff)
        {
            t.fontSize = (int)(baseFontSize * ((float)stringLengthCutoff / s.Length));
        }
        else
            t.fontSize = baseFontSize;
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
            tTitle = transform.GetChild(0); //Title
            tMask = transform.GetChild(1); //Mask
            tFullTextField = tMask.GetChild(0); //FullTextField
            tNextSlidePlz = transform.GetChild(2); //SliderNextSlide
            tSlider = transform.GetChild(3); //SliderHorizontal
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
        tSlider.gameObject.SetActive(false);
    }
    /*
    public void ChangeOriginPos()
    {
        NowWeAreAtFlaggedCats = !NowWeAreAtFlaggedCats;
        if (NowWeAreAtFlaggedCats)
            ReturnToPos = Vector3.zero;
        else
            ReturnToPos = Vector3.right * 0.6f;

    }
    */
    public void Moving()
    {
        if (categoryDoneBool == false)
        {
            if (transform.localPosition.x < -0.50f)
            {
                categoryDoneBool = true;
                isScrolling = true;
                EventManager.CategoryDone();
            }
        }
    }
    
    public void TapeRipSlide(float f)
    {
        BB.setTapeRip(f);
        if (f > 0.95f)
        {
            EventManager.DisableAllMarkers();
            BB.setTapeRip(1.0f);

        }
    }

    // Update is called once per frame
    void Update () {
        
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

            if(transform.localPosition != ReturnToPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, ReturnToPos, Time.deltaTime);
            }

        }
	}
}
