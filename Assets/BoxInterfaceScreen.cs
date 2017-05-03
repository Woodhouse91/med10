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
    float[] prevY = new float[2]; 
    float scrollSpeed;
    bool velocityScrolling = false;
    bool categoryDoneBool = false;
    int enabledTextFields = 0;
    public bool isScrolling = false;
    int tTextfieldplaceInt;
    LuxusSegmentHandler LSH;
    float SPRINGBOUYS = 50;
    private int baseFontSize =  100;
    private int stringLengthCutoff = 15;
    [Header("Hints")]
    [SerializeField] private Transform flagHint;
    [SerializeField] private Transform markHint;
    [SerializeField] private Transform sliderHint;
    [SerializeField] private Transform nextSlideHint;
    [SerializeField] private Transform activePos;
    [SerializeField] private Transform deactivePos;
    private Transform currentHint = null;
    private bool activating = false;
    private float hintTimerStay = 0f;
    private float activateTimer = 1f;
    private float autoDisableTime = 8f;
    private bool firstActivate = true;
    private bool interactionDone = true;
    private float interactionWaitTime  = 15f;
    private bool showingHint = false;
    private bool firstBox = true;

    // Use this for initialization
    void Start () {
        pac = FindObjectOfType<PlaceAllCrates>();
        FlaggedItem = new List<int>();
        DisableHeleLortet();
        ReturnToPos = Vector3.zero; // START POSITION
        EventManager.OnBoxAtTable += boxAtTable;
        EventManager.OnBoxEmptied += boxEmptied;
        EventManager.OnCategoryDone += categoryDone;
        EventManager.OnMoneyInstantiated += MoneyInstantiated;
        EventManager.OnCategoryFinished += CategoryFinished;
        EventManager.OnExcelDataLoaded += ShowSliderHint;
        
    }

  

    private void Unsub()
    {
        EventManager.OnBoxAtTable -= boxAtTable;
        EventManager.OnBoxEmptied -= boxEmptied;
        EventManager.OnCategoryDone -= categoryDone;
        EventManager.OnMoneyInstantiated -= MoneyInstantiated;
        EventManager.OnCategoryFinished -= CategoryFinished;
        EventManager.OnExcelDataLoaded -= ShowSliderHint;
    }

    private void ShowSliderHint()
    {
        StartCoroutine(firstDeactivateHint(flagHint));
        StartCoroutine(firstDeactivateHint(markHint));
        StartCoroutine(firstDeactivateHint(nextSlideHint));
        StartCoroutine(activateHint(sliderHint));
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
        firstActivate = false;
        StartCoroutine(deactivateHint(nextSlideHint));
        StartCoroutine(FadeOutScreen());
        
    }
    public void FlagIt(Transform target)
    {
        StartCoroutine(deactivateHint(flagHint));
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
                if (tTextField[i].GetChild(2).GetComponent<Text>().text == FormatHandler.FormatCategory(DataHandler.BudgetCategories[FlaggedItem[j]]))
                {
                    tTextField[i].GetChild(1).GetComponent<Image>().sprite = Flagged;
                    break;
                }
                else
                    tTextField[i].GetChild(1).GetComponent<Image>().sprite = Unflagged;
            }
            if(FlaggedItem.Count == 0)
            {
                tTextField[i].GetChild(1).GetComponent<Image>().sprite = Unflagged;
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
        isScrolling = true;
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
        //StartCoroutine(waitForScaleOfModels());
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].GetComponent<Selectable>().enabled = false;
        }
        tTitle.gameObject.SetActive(true);
        tNextSlidePlz.gameObject.SetActive(true);
    }

    private IEnumerator waitForScaleOfModels()
    {
        yield return new WaitForSeconds(EventManager.scaleTime); 
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].GetComponent<Selectable>().enabled = false;
        }
        tNextSlidePlz.gameObject.SetActive(true);


    }
    private IEnumerator firstDeactivateHint(Transform myHint)
    {
        yield return new WaitForSeconds(1.5f);
        activating = false;
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = deactivePos.position;
        Quaternion tarRot = deactivePos.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.zero;
        for(int x = 1; x<myHint.childCount; ++x)
        {
            myHint.GetChild(x).GetComponent<LineRenderer>().enabled = false;
        }
        float t = 0;
        while (!activating && t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
        }
        yield break;
    }
    private IEnumerator deactivateHint(Transform myHint)
    {
        activating = false;
        currentHint = null;
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = deactivePos.position;
        Quaternion tarRot = deactivePos.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.zero;
        float t = 0;
        while (t < hintTimerStay && !activating)
        {
            t += Time.deltaTime;
            yield return null;
        }
        if (activating)
            yield break;
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
        }
        for (int x = 1; x < myHint.childCount; ++x)
        {
            myHint.GetChild(x).GetComponent<LineRenderer>().enabled = false;
        }
        yield break;
    }
    private IEnumerator activateHint(Transform myHint)
    {
        if (currentHint != null)
        {
            StartCoroutine(deactivateHint(currentHint));
            yield return new WaitForSeconds(activateTimer);
        }
        currentHint = myHint;
        activating = true;
        float t = 0;
        Vector3 orgPos = myHint.position;
        Quaternion orgRot = myHint.rotation;
        Vector3 tarPos = activePos.position;
        Quaternion tarRot = activePos.rotation;
        Vector3 orgScale = myHint.localScale;
        Vector3 tarScale = Vector3.one;
        for (int x = 1; x < myHint.childCount; ++x)
        {
            myHint.GetChild(x).GetComponent<LineRenderer>().enabled = true;
        }
        while (activating && t < 1)
        {
            t += Time.deltaTime / activateTimer;
            myHint.position = Vector3.Lerp(orgPos, tarPos, t);
            myHint.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            myHint.localScale = Vector3.Lerp(orgScale, tarScale, t);
            yield return null;
        }
        yield break;
    }
    private IEnumerator automaticDeactivate(Transform hint)
    {
        yield return new WaitForSeconds(autoDisableTime);
        StartCoroutine(deactivateHint(hint));
        if (firstBox)
        {
            yield return new WaitForSeconds(activateTimer);
            StartCoroutine(activateHint(markHint));
            StartCoroutine(automaticDeactivate(markHint));
            firstBox = false;
        }
        yield break;
    }
    private void boxAtTable(BoxBehaviour BB)
    {
        isScrolling = false;
        if (firstBox)
        {
            StartCoroutine(activateHint(flagHint));
            StartCoroutine(automaticDeactivate(flagHint));
        }
        sliderHint.GetChild(3).GetComponent<hintDrawLineTo>().target = BB.transform;
        this.BB = BB;
        //tTitle.gameObject.SetActive(true);
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
            tTextField[i].GetComponent<Selectable>().enabled = false;
        }
        UpdateImages();
        StartCoroutine(waitforExpectedinteraction(sliderHint));
    }

    private IEnumerator waitforExpectedinteraction(Transform hint)
    {
        float t = 0;
        interactionDone = false;
        while (t < interactionWaitTime)
        {
            t += Time.deltaTime;
            yield return null;
            if (interactionDone)
                yield break;
        }
        StartCoroutine(activateHint(hint));
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
        StartCoroutine(deactivateHint(markHint));
        for (int i = 0; i < tTextField.Length; i++)
        {
            if(childIndex == i && tTextField[i].GetComponentInChildren<Selectable>().enabled == false)
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
        transform.localPosition = Vector3.zero;
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
                interactionDone = true;
            }
        }
    }
    
    public void TapeRipSlide(float f)
    {
        BB.setTapeRip(f);
        if (f > 0.95f)
        {
            interactionDone = true;
            EventManager.DisableAllMarkers();
            BB.setTapeRip(1.0f);
            StartCoroutine(deactivateHint(sliderHint));
            if(!firstBox)
            {
                print("nextslidewait");
                StartCoroutine(waitforExpectedinteraction(nextSlideHint));
            }
        }
    }

    private void CategoryFinished() // HER SKAL DEN VISE ALLE FLAGGED CATEGORIES
    {
        tTitle.gameObject.SetActive(true);
        tSlider.gameObject.SetActive(true);
        tTitle.GetComponent<Text>().text = "MARKEREDE UDGIFTER";
        tMask.gameObject.SetActive(true);
        tFullTextField.gameObject.SetActive(true);
        enabledTextFields = FlaggedItem.Count;
        tTextfieldplaceInt = enabledTextFields;
        if (tTextfieldplaceInt < 3)
            tTextfieldplaceInt = 3;
        for (int i = 0; i < enabledTextFields; i++)
        {
            tTextField[i].gameObject.SetActive(true);
            stringSize(tTextField[i].GetComponentInChildren<Text>(), FormatHandler.FormatCategory(DataHandler.BudgetCategories[FlaggedItem[i]]));
            tTextField[i].GetComponent<Selectable>().enabled = false;
        }
        UpdateImages();
    }
    /*
    public void InheritVelocity(Vector3 velocity) // TODO VIRKER SLET IKKE MEN GIVER HELLER IKKE FEJL LOL (HERFRA)
    {
        print(velocity);
        if (velocity == Vector3.zero)
            velocityScrolling = false;
        else
        {
            velocityScrolling = true;
            //StartCoroutine(VelocityScrolling(velocity));
        }
    }
    IEnumerator VelocityScrolling(Vector3 velocity)
    {
        while(velocityScrolling || velocity.magnitude < 0.1f)
        {
            tFullTextField.transform.localPosition += new Vector3(0, velocity.y, 0) * Time.deltaTime;
            velocity -= velocity * Time.deltaTime;
            yield return null;
        }
        velocity = Vector3.zero; // lige meget
        yield return null;
    }                                                                                                   // (HERTIL) */
    // Update is called once per frame
    void Update () {
        if (!isScrolling && tFullTextField.gameObject.activeSelf)
        {
            scrollSpeed = tFullTextField.localPosition.y - prevY[1];
            tFullTextField.localPosition += ((Vector3.up / SPRINGBOUYS)  * scrollSpeed) / Time.deltaTime;
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
        prevY[1] = prevY[0];
        prevY[0] = tFullTextField.localPosition.y;
	}
}
