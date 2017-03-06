using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheNewMarker : MonoBehaviour {

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
        {
            listCurrency[i] = new List<Transform>();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Currency")
        {
            if (other.GetComponent<currency>().hasOwner)
                return;
            other.GetComponent<currency>().hasOwner = true;
            other.transform.SetParent(transform);
            int curVal = 0;
            switch ((int)other.GetComponent<currency>().CurrencyValue)
            {
                case 1000:
                    curVal = 0;
                    break;
                case 500:
                    curVal = 1;
                    break;
                case 200:
                    curVal = 2;
                    break;
                case 100:
                    curVal = 3;
                    break;
                case 50:
                    curVal = 4;
                    break;
                case 20:
                    curVal = 5;
                    break;
                case 10:
                    curVal = 6;
                    break;
                case 5:
                    curVal = 7;
                    break;
                case 2:
                    curVal = 8;
                    break;
                case 1:
                    curVal = 9;
                    break;
                case 0:
                    curVal = 10;
                    break;
            }
            listCurrency[curVal].Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {

    }
}
