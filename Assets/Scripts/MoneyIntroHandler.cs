﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyIntroHandler : MonoBehaviour {
    private int month = 0;
    private int[,] data;
    PlaceAllCrates pac;
    List<List<GameObject>> currencyFound;
    [SerializeField]
    Transform screen;
    [SerializeField]
    private float minTravelTime_screen, maxTravelTime_screen, minTravelTime_crate, maxTravelTime_crate;
    DataHandler.billRef[] res;
    private void Awake()
    {
        pac = FindObjectOfType<PlaceAllCrates>();
    }
    IEnumerator doMovement(bool coin, float t, Vector3 tarPos, Quaternion tarRot, Transform obj, bool side, bool destroy)
    {
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        int s = side ? 1 : -1;
        if (!destroy)
        {
            if (coin)
            {
                tarRot *= Quaternion.AngleAxis(90, s * Vector3.right);
                tarRot *= Quaternion.AngleAxis(Random.Range(0f, 359f), Vector3.up);
            }
            else
            {
                if (side)
                    tarRot *= Quaternion.AngleAxis(180, Vector3.up);
                tarRot *= Quaternion.AngleAxis(Random.Range(0f, 359f), Vector3.forward);
            }
        }
        else
            tarRot = new Quaternion(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359), 1);
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
        {
            Destroy(obj);
        }
        yield break;
    }
    IEnumerator eventTrigger()
    {
        yield return new WaitForSeconds(maxTravelTime_crate);
        EventManager.ObjectsPlacedAtShelves();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            MoveMoneyToScreen();
        }
    }
    public void MoveMoneyToScreen()
    {
        int sum1000 = 0, sum500 = 0, sum200 = 0, sum100 = 0, sum50 = 0, sum20 = 0, sum10 = 0, sum5 = 0, sum2 = 0, sum1 = 0;
        currencyFound = new List<List<GameObject>>();
        res = DataHandler.BillsAtCategory_Month[EventManager.CurrentCategory];
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
            if(res[x]._1000 + res[x]._500 + res[x]._200 + res[x]._100 + res[x]._50 + res[x]._20 + res[x]._10 + res[x]._5 + res[x]._2 + res[x]._1 == 0)
            {
                pac.FlipCrate(EventManager.CurrentCategory, x);
                return;
            }
        }
        List<GameObject> _1000 = new List<GameObject>(sum1000), _500 = new List<GameObject>(sum500), _200 = new List<GameObject>(sum200), 
            _100 = new List<GameObject>(sum100), _50 = new List<GameObject>(sum50), _20 = new List<GameObject>(sum20), 
            _10 = new List<GameObject>(sum10), _5 = new List<GameObject>(sum5), _2 = new List<GameObject>(sum2), _1 = new List<GameObject>(sum1);
        GameObject[] search = GameObject.FindGameObjectsWithTag("1000kr");
        Vector3 pos = screen.position;
        Quaternion rot = screen.rotation;
        bool side = true;
        for(int x = 0; x< _1000.Count; ++x)
        {
            _1000[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("500kr");
        for(int x = 0; x<_500.Count; ++x)
        {
            _500[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("200kr");
        for (int x = 0; x < _200.Count; ++x)
        {
            _200[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("100kr");
        for (int x = 0; x < _100.Count; ++x)
        {
            _100[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("50kr");
        for (int x = 0; x < _50.Count; ++x)
        {
            _50[x] = search[x];
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("20kr");
        for (int x = 0; x < _20.Count; ++x)
        {
            _20[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("10kr");
        for (int x = 0; x < _10.Count; ++x)
        {
            _10[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
        }
        search = GameObject.FindGameObjectsWithTag("5kr");
        for (int x = 0; x < _5.Count; ++x)
        {
            _5[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("2kr");
        for (int x = 0; x < _2.Count; ++x)
        {
            _2[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("1kr");
        for (int x = 0; x < _1.Count; ++x)
        {
            _1[x] = search[x];
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
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
        int bill = 0;
        for (int x = 0; x<res.Length; ++x)
        {
            bill = 0;
            Transform t = pac.GetCrate(EventManager.CurrentCategory, x);
            for (int l = 0; l < res[x]._1000; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._500; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._200; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._100; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            for (int l = 0; l < res[x]._50; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._20; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._10; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._5; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._2; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._1; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t.position, t.rotation, currencyFound[bill][0].transform, true, true));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            StartCoroutine(eventTrigger());
        }
    }
}
