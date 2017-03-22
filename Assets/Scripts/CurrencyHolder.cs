using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CurrencyHolder : MonoBehaviour {
    [HideInInspector]
    public List<Transform> myCurrency;
    [SerializeField]
    private float offset;
    private float _offset;
    private CurrencyHandler ch;
    public int Total
    {
        get
        {
            return _total;
        }
    }
    private int _total, _50, _100, _200, _500, _1000;
    private void Awake()
    {
        ch = FindObjectOfType<CurrencyHandler>();
        _offset = offset;
        myCurrency = new List<Transform>();
        if (tag == "ColumnSection")
            ch.OnCurrencyChange += updateHolding;
    }

    public void updateHolding()
    {
        _total = 0; _50 = 0; _100 = 0; _200 = 0; _500 = 0; _1000 = 0;
        myCurrency.Sort(delegate (Transform a, Transform b)
        {
            return (b.GetComponent<currency>().CurrencyValue.CompareTo(a.GetComponent<currency>().CurrencyValue));
        });
        offset = _offset;
        for (int x = 0; x < myCurrency.Count; ++x)
        {
            StartCoroutine(PlaceMoneyInSpace(myCurrency[x], transform.GetChild(0).position - transform.up * offset));
            switch ((int)myCurrency[x].GetComponent<currency>().CurrencyValue)
            {
                case 50:
                    _50 ++;
                    break;
                case 100:
                    _100 ++;
                    break;
                case 200:
                    _200++;
                    break;
                case 500:
                    _500++;
                    break;
                case 1000:
                    _1000++;
                    break;
            }
            _total += (int)myCurrency[x].GetComponent<currency>().CurrencyValue;
            offset += _offset;
        }
    }
    public List<Transform> extractExactValue(int val)
    {
        if (val > _total)
        {
            throw new System.Exception("Tried to extract a value higher than total");
        }
        _total -= val;
        int lacking = lackVal(val);
        if (lacking > 0)
        {
            exchange(lacking);
        }
        List<Transform> res = new List<Transform>();
        for(int x = myCurrency.Count-1; x>-1; x--)
        {
            if (myCurrency[x].GetComponent<currency>().CurrencyValue <= val)
            {
                res.Add(myCurrency[x]);

            }
        }
        return res;
    }
    public List<Transform> extractPercentageValue(float perc)
    {
        if (perc < 0f || perc > 1f)
        {
            throw new System.Exception("Tried to extract value with invalid percentage parameter");
        }
        int val = (int)(_total * (1 - perc));
        List<Transform> res = extractExactValue(val);
        return res;
    }
    int lackVal(int val)
    {
        int res = val;
        int split = res / 1000;
        if (split > 1)
        {
            if(split>=_1000)
            {
                res -= _1000;
                _1000 = 0;
            }
            else
            {
                res -= 1000 * split;
                _1000 -= split;
            }
        }
        split = res / 500;
        if (split > 1)
        {
            if (split >= _500)
            {
                res -= _500;
                _500 = 0;
            }
            else
            {
                res -= 500 * split;
                _500 -= split;
            }
        }
        split = res / 200;
        if (split > 1)
        {
            if (split >= _200)
            {
                res -= _200;
                _200 = 0;
            }
            else
            {
                res -= 200 * split;
                _200 -= split;
            }
        }
        split = res / 100;
        if (split > 1)
        {
            if (split >= _100)
            {
                res -= _100;
                _100 = 0;
            }
            else
            {
                res -= 100 * split;
                _100 -= split;
            }
        }
        split = res / 50;
        if (split > 1)
        {
            if (split >= _50)
            {
                res -= _50;
                _50 = 0;
            }
            else
            {
                res -= 50 * split;
                _50 -= split;
            }
        }
        return res;
    }
    public void exchange(int targetValue)
    {
        int split = 0;
        //switch (targetValue)
        //{
        //    case 50:
        //        if(_100>1)
        //        {
        //            split = 1;
        //        }
        //        else if(_200>)
        //        break;
        //}
        for (int i = 0; i < myCurrency.Count; i++)
        {
            split = (int)myCurrency[i].GetComponent<currency>().CurrencyValue / targetValue;
            if (split >= 2)
            {
                
            }
        }
    }
    IEnumerator PlaceMoneyInSpace(Transform obj, Vector3 pos)
    {
        float t = 0;
        Transform id = obj;
        while (t < 1)
        {
            id.position = Vector3.Lerp(id.position, pos, t);
            id.LookAt(id.transform.position + transform.forward);
            id.Rotate(Vector3.right * -5f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    private void OnApplicationQuit()
    {
        ch.OnCurrencyChange -= updateHolding;
    }
    private void OnDisable()
    {
        ch.OnCurrencyChange -= updateHolding;
    }
}
