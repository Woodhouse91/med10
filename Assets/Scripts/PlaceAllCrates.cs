using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlaceAllCrates : MonoBehaviour {

    public GameObject[,] ListOfCrates;
    public Vector3 SizeOfCrate; //should be (1.5f,1.0f,1.0f)
    public int SizeOfBudget;
    public int TestCat;
    public int TestMonth;
    public float AngleRotate;
    public GameObject Crate, MonthCrate;
    Vector3 originCrateSize;
    

    // Use this for initialization
    void Start () {

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
        PlaceTheMonths();
        PlaceAllRows();
    }

    public Transform GetCrate(int category, int month)
    {
        return ListOfCrates[month+1, category].transform;
    }
    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(ExpandCrateAt_Cat_Month(TestCat,TestMonth, 0.5f));
        }
        if(Input.GetKeyDown(KeyCode.K))
            raiseAllCrate();


    }
    public void FlipCrate(int category, int month)
    {
        StartCoroutine(FlipCrateAni( ListOfCrates[month, category]));
    }
    IEnumerator FlipCrateAni(GameObject crate) // MANGLER ANIMATION
    {
        crate.transform.rotation = Quaternion.AngleAxis(180, crate.transform.up);

        yield return null;
    }
  
    public void raiseAllCrate()
    {
        for (int i = 1; i <13; i++)
        {
            for (int k = 0; k < SizeOfBudget; k++)
            {
                StartCoroutine(MoveWithRow(ListOfCrates[i, k].transform.position, ListOfCrates[i, k].transform.position + ListOfCrates[i, k].transform.up, ListOfCrates[i, k].transform));
            }
        }
        StartCoroutine(waitASec());
    }

    private IEnumerator waitASec()
    {
        yield return new WaitForSeconds(1f);
        EventManager.StartNextCategory();
    }

    public IEnumerator ExpandCrateAt_Cat_Month(int rowNumber, int Month, float percent)
    {
        int month = Month + 1; 
        Vector3 start = originCrateSize;
        Vector3 end = originCrateSize + Vector3.up * percent;
        Vector3[] startPos = new Vector3[rowNumber];
        Vector3[] endPos = new Vector3[rowNumber];
        for (int i = 0; i < rowNumber; i++)
        {
            startPos[i] = ListOfCrates[month, i].transform.position;
            endPos[i] = ListOfCrates[month, i].transform.position + Vector3.up * originCrateSize.y * percent / 2f; // originsize = SizeOfCrate.y
        }
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            for (int i = 0; i < rowNumber; i++)
            {
                ListOfCrates[month, i].transform.position = Vector3.Lerp(startPos[i], endPos[i], t);
            }
            ListOfCrates[month, rowNumber].transform.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
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


    public void PlaceTheMonths()
    {
        int startMonth = DataHandler.startMonth;
        print("startmonth er: "+ DataHandler.startMonth);
        for (int i = 0; i < 12; i++)
        {
            GameObject month = Instantiate(MonthCrate);
            month.transform.position = ListOfCrates[i + 1,0].transform.position + ListOfCrates[i + 1, 0].transform.forward  / 2f;
            month.transform.rotation = ListOfCrates[i + 1, 0].transform.rotation * Quaternion.AngleAxis(90f,Vector3.up);
            month.GetComponentInChildren<TextMesh>().text = monthToString((startMonth + i) % 12);
        }
    }

    string monthToString(int x)
    {
        switch (x)
        {
            case 0:
                return "Januar";
            case 1:
                return "Februar";
            case 2:
                return "Marts";
            case 3:
                return "April";
            case 4:
                return "Maj";
            case 5:
                return "Juni";
            case 6:
                return "Juli";
            case 7:
                return "August";
            case 8:
                return "September";
            case 9:
                return "Oktober";
            case 10:
                return "November";
            case 11:
                return "December";
            default:
                return "Null";
        }
    }
    public void PlaceAllRows() //place the rest of all the rows needed
    {
        for (int i = 1; i < 13 ; i++) //runs through all the crates
        {
            for (int k = 1; k < SizeOfBudget; k++)
            {
                ListOfCrates[i,k] = Instantiate(Crate, transform);
                ListOfCrates[i,k].transform.position = ListOfCrates[i, 0].transform.position + Vector3.down * SizeOfCrate.y * k;
                ListOfCrates[i, k].transform.rotation = ListOfCrates[i, 0].transform.rotation;
            }
        }
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
