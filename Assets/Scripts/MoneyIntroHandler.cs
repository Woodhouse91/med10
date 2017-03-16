using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyIntroHandler : MonoBehaviour {
    private int month = 0, category = 0;
    private int[,] data;
    PlaceAllCrates pac;
    List<GameObject[]> currencyFound;
    [SerializeField]
    Transform screen;
    [SerializeField]
    private float minTravelTime, maxTravelTime;
    DataHandler.billRef[] res;
    private void Awake()
    {
        pac = FindObjectOfType<PlaceAllCrates>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            MoveMoneyToScreen();
    }
    IEnumerator doMovement(bool coin, float t, Vector3 tarPos, Quaternion tarRot, Transform obj, bool side, bool destroy)
    {
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        int s = side ? 1 : -1;
        if (coin)
        {
            tarRot *= Quaternion.AngleAxis(90, s*Vector3.right);
            tarRot *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
        }
        else
        {
            if (side)
                tarRot *= Quaternion.AngleAxis(180, Vector3.up);
            tarRot *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
        }
        float tt = 0;
        while (tt <= t)
        {
            obj.position = Vector3.Lerp(orgPos, tarPos, tt / t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, tt / t);
            tt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        obj.position = orgPos;
        obj.rotation = tarRot;
        if (destroy)
            Destroy(obj);
        yield break;
    }
    public void MoveMoneyToScreen()
    {
        int sum1000 = 0, sum500 = 0, sum200 = 0, sum100 = 0, sum50 = 0, sum20 = 0, sum10 = 0, sum5 = 0, sum2 = 0, sum1 = 0;
        currencyFound = new List<GameObject[]>();
        res = DataHandler.BillsAtMonth_Category[month];
        bool side = true;
        for(int x = 0; x<res.Length; ++x)
        {
            sum1000 += res[x]._1000;
            sum500 += res[x]._500;
            sum200 += res[x]._200;
            sum100 += res[x]._100;
            sum50 += res[x]._20;
            sum20 += res[x]._20;
            sum10 += res[x]._10;
            sum5 += res[x]._5;
            sum2 += res[x]._2;
            sum1 += res[x]._1;
        }
        GameObject[] _1000 = new GameObject[sum1000], _500 = new GameObject[sum500], _200 = new GameObject[sum200], 
            _100 = new GameObject[sum100], _50 = new GameObject[sum50], _20 = new GameObject[sum20], 
            _10 = new GameObject[sum10], _5 = new GameObject[sum5], _2 = new GameObject[sum2], _1 = new GameObject[sum1];
        GameObject[] search = GameObject.FindGameObjectsWithTag("1000kr");
        Vector3 pos = screen.position;
        Quaternion rot = screen.rotation;
        for(int x = 0; x< _1000.Length; ++x)
        {
            _1000[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("500kr");
        for(int x = 0; x<_500.Length; ++x)
        {
            _500[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("200kr");
        for (int x = 0; x < _200.Length; ++x)
        {
            _200[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("100kr");
        for (int x = 0; x < _100.Length; ++x)
        {
            _100[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("50kr");
        for (int x = 0; x < _50.Length; ++x)
        {
            _50[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("20kr");
        for (int x = 0; x < _20.Length; ++x)
        {
            _20[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("10kr");
        for (int x = 0; x < _10.Length; ++x)
        {
            _10[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
        }
        search = GameObject.FindGameObjectsWithTag("5kr");
        for (int x = 0; x < _5.Length; ++x)
        {
            _5[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("2kr");
        for (int x = 0; x < _2.Length; ++x)
        {
            _2[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("1kr");
        for (int x = 0; x < _1.Length; ++x)
        {
            _1[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime, maxTravelTime), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        currencyFound.Add(_1000);
        currencyFound.Add(_500);
        currencyFound.Add(_200);
        currencyFound.Add(_100);
        currencyFound.Add(_50);
        currencyFound.Add(_20);
        currencyFound.Add(_10);
        currencyFound.Add(_5);
        currencyFound.Add(_2);
        currencyFound.Add(_1);
    }
    public void MoveMoneyToCrate()
    {
        for(int x = 0; x<res.Length; ++x)
        {
            for(int k = 0; k<10; ++k)
            {
                for(int l = 0; l < res[x]._1000; ++l)
                {

                }
            }
        }
    }
}
