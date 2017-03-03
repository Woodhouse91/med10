using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMarker : MonoBehaviour {

    public TextMesh totalText;
    float totalTextVal;
    public Transform TargetColumn;
    List<Transform>[] listCurrency = new List<Transform>[11];

    private void OnApplicationQuit()
    {
        for (int i = 0; i < listCurrency.Length; i++)
        {
            listCurrency[i].Clear();
        }
    }
    // Use this for initialization
    private void OnEnable()
    {
        for (int i = 0; i < listCurrency.Length; i++)
            listCurrency[i] = new List<Transform>();
        totalText.gameObject.SetActive(false);
        totalTextVal = 0;

    }
    public void MyDisable()
    {
        if (TargetColumn == null)
        {
            gameObject.SetActive(false);
            return; // HER SKAL DEN I MONEYBALL
        }
        for (int k = 0; k < 11; ++k)
        {
            for (int i = 0; i < listCurrency[k].Count; i++)
            {
                //listCurrency[k][i].SetParent(TargetColumn.GetComponent<ColumnSection>().MoneySpace);
                listCurrency[k][i].SetParent(null);
                TargetColumn.GetComponent<ColumnSection>().ListCurrency[k].Add(listCurrency[k][i]);
            }
            //listCurrency[k].Clear();
        }
        
        TargetColumn.GetComponent<ColumnSection>().PlaceMoney(); // make the columnSection set the money
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {

        // keyboard controls
        transform.Translate(Vector3.up * Input.GetAxis("Vertical")* Time.deltaTime * 0.15f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.15f);

        if (Input.GetKeyDown(KeyCode.Space))
            MyDisable();        
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Currency")
        {
            if (totalText.gameObject.activeSelf == false)
                totalText.gameObject.SetActive(true);
            if (other.transform.parent == transform)
                return;
            other.transform.SetParent(transform);

            float newCurrency = other.transform.GetComponent<currency>().CurrencyValue;
            totalTextVal += newCurrency;
            totalText.text = totalTextVal.ToString();
            switch((int)newCurrency)
            {
                case 1000:
                    listCurrency[0].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[0].Remove(other.transform);
                    break;
                case 500:
                    listCurrency[1].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[1].Remove(other.transform);
                    break;
                case 200:
                    listCurrency[2].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[2].Remove(other.transform);
                    break;
                case 100:
                    listCurrency[3].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[3].Remove(other.transform);
                    break;
                case 50:
                    listCurrency[4].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[4].Remove(other.transform);
                    break;
                case 20:
                    listCurrency[5].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[5].Remove(other.transform);
                    break;
                case 10:
                    listCurrency[6].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[6].Remove(other.transform);
                    break;
                case 5:
                    listCurrency[7].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[7].Remove(other.transform);
                    break;
                case 2:
                    listCurrency[8].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[8].Remove(other.transform);
                    break;
                case 1:
                    listCurrency[9].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[9].Remove(other.transform);
                    break;
                case 0:
                    listCurrency[10].Add(other.transform);
                    TargetColumn.GetComponent<ColumnSection>().ListCurrency[10].Remove(other.transform);
                    break;
            }
            
        }
        if (other.tag == "ColumnSection")
        {
            TargetColumn = other.transform;
            other.GetComponent<ColumnSection>().Highlight(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ColumnSection")
        {
            other.GetComponent<ColumnSection>().Highlight(false);
        }
    }




}
