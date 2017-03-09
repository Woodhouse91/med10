using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CurrencyHandler : MonoBehaviour {
    public delegate void CurrencyEvent();
    public event CurrencyEvent OnCurrencyChange;
    private Transform moneyBall;

    public void TransferCurrency(Transform to, Transform from)
    {
        for(int k = 0; k<from.GetComponent<CurrencyHolder>().myCurrency.Count; ++k)
        {
            from.GetComponent<CurrencyHolder>().myCurrency[k].GetComponent<currency>().MarkBill(false);
            if(from.GetComponent<CurrencyHolder>().myCurrency[k].GetComponent<currency>().myListObj == to)
            {
                to = moneyBall;
                break;
            }
        }
        for(int x = 0; x<from.GetComponent<CurrencyHolder>().myCurrency.Count; ++x)
        {
            from.GetComponent<CurrencyHolder>().myCurrency[x].GetComponent<currency>().myListObj = to;
            to.GetComponent<CurrencyHolder>().myCurrency.Add(from.GetComponent<CurrencyHolder>().myCurrency[x]);
        }
        from.GetComponent<CurrencyHolder>().myCurrency.Clear();
        CurrencyChange();
    }
    public void MarkCurrency(Transform obj, Transform marker)
    {
        if(marker.GetComponent<CurrencyHolder>().myCurrency.Contains(obj))
        {
            obj.GetComponent<currency>().myListObj.GetComponent<CurrencyHolder>().myCurrency.Add(obj);
            marker.GetComponent<CurrencyHolder>().myCurrency.Remove(obj);
            obj.GetComponent<currency>().MarkBill(false);
        }
        else
        {
            obj.GetComponent<currency>().myListObj.GetComponent<CurrencyHolder>().myCurrency.Remove(obj);
            marker.GetComponent<CurrencyHolder>().myCurrency.Add(obj);
            obj.GetComponent<currency>().MarkBill(true);
        }
    }
    public void AddCurrency(Transform to, Transform obj)
    {
        if (obj.GetComponent<currency>().myListObj!=null)
            obj.GetComponent<currency>().myListObj.GetComponent<CurrencyHolder>().myCurrency.Remove(obj);
        to.GetComponent<CurrencyHolder>().myCurrency.Add(obj);
        obj.GetComponent<currency>().myListObj = to;
        CurrencyChange();
    }
    public void PickUpCurrency(Transform picker)
    {

    }
    public void CurrencyChange()
    {
        if (OnCurrencyChange != null)
            OnCurrencyChange();
    }
}