using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    [SerializeField]
    private int CurrencySpawnedPerFrame;
    public static List<Transform> _1000, _500, _200, _100, _50, _20, _10, _5, _2, _1;
    public static List<GameObject> allCurrency;
    DataHandler.TotalBills tBills;
    private static GameObject pref1000, pref500, pref200, pref100, pref50, pref20, pref10, pref5, pref2, pref1;
    private static bool spawning = false;
    public static float spawnProgress
    {
        get
        {
            if (totalToSpawn == 0)
                return 0;
            return (float)curSpawned / totalToSpawn;
        }
    }
    private static readonly Random rnd = new Random();
    private static int totalToSpawn, curSpawned;
    // Use this for initialization
    void Start()
    {
        pref1000 = Resources.Load<GameObject>("_1000");
        pref500 = Resources.Load<GameObject>("_500");
        pref200 = Resources.Load<GameObject>("_200");
        pref100 = Resources.Load<GameObject>("_100");
        pref50 = Resources.Load<GameObject>("_50");
        pref20 = Resources.Load<GameObject>("_20");
        pref10 = Resources.Load<GameObject>("_10");
        pref5 = Resources.Load<GameObject>("_5");
        pref2 = Resources.Load<GameObject>("_2");
        pref1 = Resources.Load<GameObject>("_1");
        allCurrency = new List<GameObject>();
        _1000 = new List<Transform>(); _500 = new List<Transform>(); _200 = new List<Transform>(); _100 = new List<Transform>();
        _50 = new List<Transform>(); _20 = new List<Transform>(); _10 = new List<Transform>(); _5 = new List<Transform>();
        _2 = new List<Transform>(); _1 = new List<Transform>();
        EventManager.OnExcelDataLoaded += PoolMoney;
    }
    private void PoolMoney()
    {
        curSpawned = 0;
        totalToSpawn = 0;
        tBills = DataHandler.tBills;
        totalToSpawn += tBills._1000;
        totalToSpawn += tBills._500;
        totalToSpawn += tBills._200;
        totalToSpawn += tBills._100;
        totalToSpawn += tBills._50;
        totalToSpawn += tBills._20;
        totalToSpawn += tBills._10;
        totalToSpawn += tBills._5;
        totalToSpawn += tBills._2;
        totalToSpawn += tBills._1;
        StartCoroutine(contolledSpawnToSingleList());
    }
    IEnumerator contolledSpawnToSingleList()
    {
        spawning = true;
        StartCoroutine(spawnMoney(pref1000, tBills._1000));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref500, tBills._500));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref200, tBills._200));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref100, tBills._100));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref50, tBills._50));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref20, tBills._20));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref10, tBills._10));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref5, tBills._5));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref2, tBills._2));
        while (spawning)
            yield return null;
        spawning = true;
        StartCoroutine(spawnMoney(pref1, tBills._1));
        while (spawning)
            yield return null;
        spawning = true;
        for (int i = 0; i < allCurrency.Count; i++)
        {
            GameObject temp = allCurrency[i];
            int randomIndex = Random.Range(i, allCurrency.Count-1);
            allCurrency[i] = allCurrency[randomIndex];
            allCurrency[randomIndex] = temp;
        }
        EventManager.MoneyInstantiated();
        yield break;
    }
    IEnumerator spawnMoney(GameObject go, int count)
    {
        int i = 0;
        if (CurrencySpawnedPerFrame == 0)
            CurrencySpawnedPerFrame = 1;
        while (i < count)
        {
            allCurrency.Add(Instantiate(go));
            if (go == pref1000)
                _1000.Add(allCurrency[i].transform);
            else if (go == pref500)
                _500.Add(allCurrency[i].transform);
            else if (go == pref200)
                _200.Add(allCurrency[i].transform);
            else if (go == pref100)
                _100.Add(allCurrency[i].transform);
            else if (go == pref50)
                _50.Add(allCurrency[i].transform);
            else if (go == pref20)
                _20.Add(allCurrency[i].transform);
            else if (go == pref10)
                _10.Add(allCurrency[i].transform);
            else if (go == pref5)
                _5.Add(allCurrency[i].transform);
            else if (go == pref2)
                _2.Add(allCurrency[i].transform);
            else if (go == pref1)
                _1.Add(allCurrency[i].transform);
            ++curSpawned;
            i++;
            if (i % CurrencySpawnedPerFrame == 0)
                yield return null;
        }
        spawning = false;
        yield break;
    }
    public static Transform getCurrency(int currency)
    {
        Transform res;
        switch (currency)
        {
            case 1000:
                res = _1000[0];
                _1000.Remove(_1000[0]);
                return res;
            case 500:
                res = _500[0];
                _500.Remove(_500[0]);
                return res;
            case 200:
                res = _200[0];
                _200.Remove(_200[0]);
                return res;
            case 100:
                res = _100[0];
                _100.Remove(_100[0]);
                return res;
            case 50:
                res = _50[0];
                _1000.Remove(_50[0]);
                return res;
            case 20:
                res = _20[0];
                _20.Remove(_20[0]);
                return res;
            case 10:
                res = _10[0];
                _10.Remove(_10[0]);
                return res;
            case 5:
                res = _5[0];
                _5.Remove(_5[0]);
                return res;
            case 2:
                res = _2[0];
                _2.Remove(_5[0]);
                return res;
            case 1:
                res = _1[0];
                _1.Remove(_1[0]);
                return res;
        }
        return null;
    }
}
