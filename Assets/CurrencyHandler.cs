using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CurrencyHandler : MonoBehaviour {
    private Transform prevColumn;
    public delegate void CurrencyEvent();
    public event CurrencyEvent OnCurrencyChange;
    private Transform moneyBall;

    public void TransferCurrency(Transform to, Transform from)
    {
        for(int k = 0; k<from.GetComponent<CurrencyHolder>().myCurrency.Count; ++k)
        {
            if(from.GetComponent<CurrencyHolder>().myCurrency[k].GetComponent<currency>().myListObj == to)
            {
                to = moneyBall;
                break;
            }
        }
        for(int x = 0; x<from.GetComponent<CurrencyHolder>().myCurrency.Count; ++x)
        {
            from.GetComponent<CurrencyHolder>().myCurrency[x].GetComponent<currency>().tar = null;
            from.GetComponent<CurrencyHolder>().myCurrency[x].GetComponent<currency>().myListObj = to;
            to.GetComponent<CurrencyHolder>().myCurrency.Add(from.GetComponent<CurrencyHolder>().myCurrency[x]);
        }
        from.GetComponent<CurrencyHolder>().myCurrency.Clear();
        CurrencyChange();
    }
    public void PickUp(Transform obj, Transform to)
    {
        if(obj.GetComponent<currency>().tar == null)
        {
            Transform from = obj.GetComponent<currency>().myListObj;
            from.GetComponent<CurrencyHolder>().myCurrency.Remove(obj);
            to.GetComponent<CurrencyHolder>().myCurrency.Add(obj);
            obj.GetComponent<currency>().tar = to;
            obj.GetComponent<currency>().myListObj = to;
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
    public void CurrencyChange()
    {
        if (OnCurrencyChange != null)
            OnCurrencyChange();
    }
}