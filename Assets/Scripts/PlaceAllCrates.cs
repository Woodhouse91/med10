using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlaceAllCrates : MonoBehaviour {

    public GameObject[,] ListOfCrates;
    public Vector3 SizeOfCrate; //should be (1.5f,1.0f,1.0f)
    public int SizeOfBudget;
    public int EXPANDDONGAT;
    public float AngleRotate;
    public GameObject Crate;

    

    // Use this for initialization
    void Start () {
        EventManager.OnExcelDataLoaded += InitiateStart;
    }
    private void Unsub()
    {
        EventManager.OnExcelDataLoaded -= InitiateStart;
    }
    private void InitiateStart()
    {
        SizeOfBudget = DataHandler.BudgetCategories.Count;
        ListOfCrates = new GameObject[14, SizeOfBudget];
        PlaceThemAll(SizeOfBudget);
        PlaceAllRows();
    }

    public Transform GetCrate(int category, int month)
    {
        return ListOfCrates[month+1, category].transform;
    }
    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ExpandRow(EXPANDDONGAT, 0.5f));
            raiseAllCrate();
        }
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
                StartCoroutine(MoveWithRow(ListOfCrates[i, k].transform.position, ListOfCrates[i, k].transform.position + ListOfCrates[i, k].transform.up * SizeOfCrate.y, ListOfCrates[i, k].transform));
            }
        }
    }

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
