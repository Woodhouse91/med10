using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlaceAllCrates : MonoBehaviour {

    public GameObject[,] ListOfCrates;
    public Vector3 SizeOfCrate; //should be (1.3f,1.0f,1.0f)
    public int SizeOfBudget;
    public int TestCat;
    public int TestMonth;
    public float AngleRotate;
    public GameObject Crate, MonthCrate;
    BoxInterfaceScreen bis;
    Vector3 originCrateSize;
    GameObject flagObj;
    bool firstCratesPlaced = false;
    float MoveDownFloor = 0;

    // Use this for initialization
    void Start () {
        bis = FindObjectOfType<BoxInterfaceScreen>();
        originCrateSize = Crate.transform.lossyScale;
        EventManager.OnExcelDataLoaded += InitiateStart;
        EventManager.OnCategoryDone += raiseAllCrate;
    }
    private void Unsub()
    {
        EventManager.OnExcelDataLoaded -= InitiateStart;
        EventManager.OnCategoryDone -= raiseAllCrate;
    }
    private void InitiateStart()
    {
        SizeOfBudget = DataHandler.BudgetCategories.Count;
        ListOfCrates = new GameObject[14, SizeOfBudget];
        PlaceThemAll(SizeOfBudget);
        
        //PlaceAllRows(); CardboardManager kalder det
    }

    public Transform GetCrate(int category, int month)
    {
        return ListOfCrates[month+1, category].transform;
    }
    
    public int GetExpenseDataFromCrate(Transform crate) //Expensive method for getting the expenseData for the specific crate
    {
        for (int i = 1; i < 12; i++)
        {
            for (int k = 0; k < SizeOfBudget; k++)
            {
                if (crate.gameObject.GetInstanceID() == ListOfCrates[i, k].GetInstanceID())
                    return DataHandler.expenseData[i, k];
            }
        }
        return 0;
    }
    public void FlagCategory(int category)
    {
        if (flagObj == null)
            flagObj = Resources.Load("flagObj",typeof(GameObject)) as GameObject;
        List<Transform> TempFlagList = new List<Transform>();
        for (int i = 1; i < 13; i++)
        {
            if (DataHandler.expenseData[i, category] > 0)
            {
                TempFlagList.Add(GetCrate(EventManager.CurrentCategory, i-1)); // eventmanager.currentcategory = category
            }
        }

        if (bis.FlaggedItem.Contains(category))
        {
            //Flag it
            for (int i = 0; i < TempFlagList.Count; i++)
            {
                GameObject flag = Instantiate(flagObj, TempFlagList[i]).gameObject;
                flag.transform.localScale = new Vector3(0.2f/originCrateSize.x,0.2f / originCrateSize.y, 0.2f / originCrateSize.z); //gives the flag a size of vector3.one * 0.1 in world space
                flag.transform.localPosition = new Vector3(0.5f-TempFlagList[i].childCount*0.1f,0.075f,0.37f);
                flag.transform.rotation = TempFlagList[i].rotation * Quaternion.AngleAxis(-70.0f,Vector3.up);
            }
        }
        else
        {
            for (int i = 0; i < TempFlagList.Count; i++)
            {
                Destroy(TempFlagList[i].GetChild(TempFlagList[i].childCount-1).gameObject);
            }
        }
    }
    public void HighlightCategory(int category)
    {

    }
    // Update is called once per frame
    void Update () {
	
    }
    public void FlipCrate(int category, int month)
    {
        StartCoroutine(FlipCrateAni( ListOfCrates[month+1, category]));
    }
    IEnumerator FlipCrateAni(GameObject crate) // MANGLER ANIMATION
    {
        float t = 0;
        Vector3 startS = crate.transform.localScale;
        Vector3 endS = crate.transform.localScale - Vector3.forward * crate.transform.localScale.z * 2f;
        while(t<1)
        {
            t += Time.deltaTime;
            crate.transform.localScale = Vector3.Lerp(startS, endS, t);
            yield return null;
        }
        yield return null;
    }
  
    public void raiseAllCrate()
    {
        /*
        for (int i = 1; i <13; i++)
        {
            for (int k = 0; k < SizeOfBudget; k++)
            {
                StartCoroutine(MoveWithRow(ListOfCrates[i, k].transform.position, ListOfCrates[i, k].transform.position + ListOfCrates[i, k].transform.up, ListOfCrates[i, k].transform));
            }
        }*/

        StartCoroutine(MoveEverythingUp());
    }

    private IEnumerator MoveEverythingUp()
    {
        /*
        float t = 0;
        Vector3 start = transform.parent.position;
        Vector3 end = transform.parent.position + transform.up;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.parent.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        */
        //yield return new WaitForSeconds(1f);
        EventManager.StartNextCategory();
        yield return null;
    }

    public IEnumerator ExpandCrateAt_Cat_Month(int rowNumber, int Month, float percent)
    {
        /*
        percent = percent / 4f; // EFTER AT HYLDERNE ER BLEVET SMÅ
        int month = Month + 1; 
        Vector3 start = originCrateSize;
        Vector3 end = originCrateSize + Vector3.up * percent;
        Vector3[] startPos = new Vector3[rowNumber];
        Vector3[] endPos = new Vector3[rowNumber];
        for (int i = 0; i < rowNumber; i++)
        {
            startPos[i] = ListOfCrates[month, i].transform.localPosition;
            endPos[i] = ListOfCrates[month, i].transform.localPosition + (Vector3.up * originCrateSize.y * percent)/(2f/originCrateSize.y); // originsize = SizeOfCrate.y
        }
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            for (int i = 0; i < rowNumber; i++)
            {
                ListOfCrates[month, i].transform.localPosition = Vector3.Lerp(startPos[i], endPos[i], t);
            }
            ListOfCrates[month, rowNumber].transform.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
        */
        yield return null;
    }

    #region OutdatedExpand
    IEnumerator ExpandRow(int rowNumber, float percent)
    {
        for (int i = 1; i < 13; i++) // only going through crates
        {
            StartCoroutine(ExpandWithRow(ListOfCrates[i, rowNumber].transform.localScale, ListOfCrates[i, rowNumber].transform.localScale + Vector3.up * percent, ListOfCrates[i, rowNumber].transform));
            for (int k = rowNumber-1; k >= 0; k--)
            {
                StartCoroutine(MoveWithRow(ListOfCrates[i, k].transform.position, ListOfCrates[i, k].transform.position + Vector3.up * SizeOfCrate.y * percent / 2f, ListOfCrates[i, k].transform));
            }
        }
        yield return null;
    }
    IEnumerator MoveWithRow(Vector3 start, Vector3 end,Transform crate)
    {
        float t = 0;
        while(t<1f)
        {
            crate.position = Vector3.Lerp(start, end, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    IEnumerator ExpandWithRow(Vector3 start, Vector3 end, Transform crate)
    {
        float t = 0;
        while (t < 1f)
        {
            crate.localScale = Vector3.Lerp(start, end, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion


    public void PlaceThemAll(int sizeOfBudget)
    {
        //kasse 7 er i 0,0,0 
        //kasse 6 er i 1.5f,0,0
        GameObject crate = Instantiate(Crate, transform);
        ListOfCrates[7, 0] = crate;
        crate.transform.position = transform.position;
        crate.transform.rotation = transform.rotation;
        PlaceThemRepeat(5, crate.transform.position, crate.transform.rotation, false);
        GameObject crate2 = Instantiate(Crate, transform);
        ListOfCrates[6, 0] = crate2;
        crate2.transform.position = crate.transform.position + crate.transform.right * SizeOfCrate.x; 
        crate2.transform.rotation = transform.rotation;
        PlaceThemRepeat(5, crate2.transform.position, crate2.transform.rotation, true);
        firstCratesPlaced = true;
    }
     void PlaceThemRepeat(int x, Vector3 prevPos, Quaternion prevRot, bool clockwise)
    {
        if(x<1)
        {
            return;
        }
        else if(clockwise)
        {
            GameObject crate = Instantiate(Crate,prevPos,prevRot, transform);
            ListOfCrates[x, 0] = crate;
            crate.transform.position += crate.transform.forward * SizeOfCrate.z / 2f + crate.transform.right * SizeOfCrate.x/2f; //first edge of crate
            crate.transform.rotation *= Quaternion.AngleAxis(-AngleRotate, crate.transform.up); //rotated to new angle
            crate.transform.position += crate.transform.forward * -SizeOfCrate.z / 2f + crate.transform.right * SizeOfCrate.x / 2f; //back to right position
            PlaceThemRepeat(x - 1, crate.transform.position, crate.transform.rotation, clockwise);
        }
        else
        {
            GameObject crate = Instantiate(Crate, prevPos, prevRot, transform);
            ListOfCrates[13 - x, 0] = crate;
            crate.transform.position += crate.transform.forward * SizeOfCrate.z / 2f - crate.transform.right * SizeOfCrate.x / 2f; //first edge of crate
            crate.transform.rotation *= Quaternion.AngleAxis(AngleRotate, crate.transform.up); //rotated to new angle
            crate.transform.position += crate.transform.forward * -SizeOfCrate.z / 2f - crate.transform.right * SizeOfCrate.x / 2f; //back to right position
            PlaceThemRepeat(x - 1, crate.transform.position, crate.transform.rotation, clockwise);
        }
    }


    public void PlaceTheMonths(int _sizeOfBudget)
    {
        int startMonth = DataHandler.startMonth;
        
        for (int i = 0; i < 12; i++)
        {
            GameObject month = Instantiate(MonthCrate);
            month.transform.position = ListOfCrates[i + 1,_sizeOfBudget-1].transform.position + ListOfCrates[i + 1, _sizeOfBudget-1].transform.forward  / 8f; //8 i stedet for 2 fordi cratesne er 4 gange mindre
            month.transform.rotation = ListOfCrates[i + 1, _sizeOfBudget-1].transform.rotation * Quaternion.AngleAxis(90f,Vector3.up);
            month.GetComponentInChildren<TextMesh>().text = monthToString((startMonth + i) % 12);
        }
    }

    string monthToString(int x)
    {
        switch (x)
        {
            case 0:
                return "Jan";
            case 1:
                return "Feb";
            case 2:
                return "Mar";
            case 3:
                return "Apr";
            case 4:
                return "Maj";
            case 5:
                return "Jun";
            case 6:
                return "Jul";
            case 7:
                return "Aug";
            case 8:
                return "Sep";
            case 9:
                return "Okt";
            case 10:
                return "Nov";
            case 11:
                return "Dec";
            default:
                return "Null";
        }
    }
    public IEnumerator PlaceAllRows(int _sizeOfBudget) //place the rest of all the rows needed
    {
        while (!firstCratesPlaced) //wait for the crates to be placed
            yield return new WaitForEndOfFrame();
       MoveDownFloor = 0;

        for (int i = 1; i < 13 ; i++) //runs through all the crates
        {
            for (int k = 1; k < _sizeOfBudget; k++)
            {
                ListOfCrates[i,k] = Instantiate(Crate, transform);
                ListOfCrates[i,k].transform.position = ListOfCrates[i, 0].transform.position + Vector3.down * SizeOfCrate.y * k;
                ListOfCrates[i, k].transform.rotation = ListOfCrates[i, 0].transform.rotation;
                if(i==1)
                {
                    transform.localPosition += Vector3.up * SizeOfCrate.y;
                    if (k > 8)
                        MoveDownFloor += SizeOfCrate.y;
                }
            }
        }
        if (MoveDownFloor > 1)
            MoveDownFloor = 1;
        transform.parent.position -= Vector3.up * MoveDownFloor;
        PlaceTheMonths(_sizeOfBudget);
    }
    int findRowNumber ()
    {
        for (int i = 0; i < SizeOfBudget; i++)
        {
            if(ListOfCrates[1, i] == null)
                return i;
        }
        return SizeOfBudget;
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
}
