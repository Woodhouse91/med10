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
    private int _total;
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
        _total = 0;
        myCurrency.Sort(delegate (Transform a, Transform b)
        {
            return (b.GetComponent<currency>().CurrencyValue.CompareTo(a.GetComponent<currency>().CurrencyValue));
        });
        offset = _offset;
        for(int x = 0; x<myCurrency.Count; ++x)
        {
            StartCoroutine(PlaceMoneyInSpace(myCurrency[x], transform.GetChild(0).position - transform.up * offset));
            _total += (int)myCurrency[x].GetComponent<currency>().CurrencyValue;
            offset += _offset;
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
