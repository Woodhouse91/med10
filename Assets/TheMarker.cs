using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMarker : MonoBehaviour {

    TextMesh totalText;
    float totalTextVal;
    public Transform TargetColumn;
    List<Transform>[] listCurrency = new List<Transform>[11];

    // Use this for initialization
	private void OnEnable()
    {
        for (int i = 0; i < listCurrency.Length; i++)
            listCurrency[i] = new List<Transform>();
        totalText = transform.GetComponentInChildren<TextMesh>();
        totalText.gameObject.SetActive(false);
    }
    private void MyDisable()
    {
        print(listCurrency[0].Count);
        for (int k = 0; k < 11; ++k)
        {
            for (int i = 0; i < listCurrency[k].Count; i++)
            {
                listCurrency[k][i].SetParent(TargetColumn);
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
        transform.Translate(Vector3.up * Input.GetAxis("Vertical")* Time.deltaTime * 0.05f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.05f);

        if (Input.GetKeyDown(KeyCode.Space))
            MyDisable();        
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Currency")
        {
            if (totalText.gameObject.activeSelf == false)
                totalText.gameObject.SetActive(true);
            other.transform.SetParent(transform);

            float newCurrency = other.transform.GetComponent<currency>().CurrencyValue;
            totalTextVal += newCurrency;
            totalText.text = totalTextVal.ToString();
            switch((int)newCurrency)
            {
                case 1000:
                    listCurrency[0].Add(other.transform);
                    break;
                case 500:
                    listCurrency[1].Add(other.transform);
                    break;
                case 200:
                    listCurrency[2].Add(other.transform);
                    break;
                case 100:
                    listCurrency[3].Add(other.transform);
                    break;
                case 50:
                    listCurrency[4].Add(other.transform);
                    break;
                case 20:
                    listCurrency[5].Add(other.transform);
                    break;
                case 10:
                    listCurrency[6].Add(other.transform);
                    break;
                case 5:
                    listCurrency[7].Add(other.transform);
                    break;
                case 2:
                    listCurrency[8].Add(other.transform);
                    break;
                case 1:
                    listCurrency[9].Add(other.transform);
                    break;
                case 0:
                    listCurrency[10].Add(other.transform);
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
