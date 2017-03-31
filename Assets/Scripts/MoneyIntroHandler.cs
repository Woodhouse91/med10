//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoneyIntroHandler : MonoBehaviour {
    private int month = 0;
    private float BagValue;
    private int maxValueOfAMonth;
    float bagSize;
    public float spawnBagDelay = 1f;
    public Transform BagOfMoney;
    private int[,] data;
    PlaceAllCrates pac;
    List<List<GameObject>> currencyFound;
    [SerializeField]
    Transform screen;
    [SerializeField]
    private float minTravelTime_screen, maxTravelTime_screen, minTravelTime_crate, maxTravelTime_crate;
    DataHandler.billRef[] res;
    public static List<Transform> model;
    private List<int> EmptyCrate;
    cardBoardManager cbm;
    public static List<Transform> focusModel;
    [SerializeField]
    Vector3 defaultModelScale;
    [SerializeField]
    private float defaultScaleTime = 1.5f;

    private void Awake()
    {
        focusModel = new List<Transform>();
        pac = FindObjectOfType<PlaceAllCrates>();
        cbm = FindObjectOfType<cardBoardManager>();
        EventManager.OnBoxEmptied += MoveMoneyToScreen;
        EventManager.OnCategorySliderDone += MoveMoneyToCrate;
        EventManager.OnExcelDataLoaded += CalculateBagOfMoney;
        EventManager.OnCategoryFinished += CreateMoneyBagsOnTable;
        EventManager.OnCategoryDone += CreateMoneyBagsOnTable2;
    }

    private void CreateMoneyBagsOnTable2()
    {
        print("vi kører videre");
    }

    private void CreateMoneyBagsOnTable()
    {
        print("nu stopper vi");
    }
    private void CalculateBagOfMoney()
    {
        BagValue = DataHandler.incomeData[12]/100f;
    }

    private void Unsub()
    {
        EventManager.OnBoxEmptied -= MoveMoneyToScreen;
        EventManager.OnCategorySliderDone -= MoveMoneyToCrate;
        EventManager.OnExcelDataLoaded -= CalculateBagOfMoney;
        EventManager.OnCategoryFinished -= CreateMoneyBagsOnTable;
        EventManager.OnCategoryDone -= CreateMoneyBagsOnTable2;
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }
    void CreateBagsOfMoney(int _month,Transform target)
    {
        float bags = DataHandler.expenseData[_month+1, EventManager.CurrentCategory] / BagValue;
        
        for (int i = 0; i < bags; i++)
        {
            Transform BoM  = Instantiate(BagOfMoney);
            if (bagSize == 0)
            {
                bagSize = BoM.GetComponent<Collider>().bounds.size.y;
                print("bagsize er : " +bagSize);
            }
            BoM.localScale = Vector3.zero;
            BoM.position = target.position + Vector3.up * (target.GetComponent<Collider>().bounds.size.y*0.8f-bagSize/2f) + Vector3.right * (target.GetComponent<Collider>().bounds.size.x * 0.8f * Random.Range(-0.4f, 0.4f));
            BoM.rotation = target.rotation;
            if (bags - i >= 1f)
                StartCoroutine(ExpandBag(1f, i,BoM, target));
            else
                StartCoroutine(ExpandBag(bags % 1, i,BoM,target));
        }

    }

    private IEnumerator FallAndChildTo(Transform obj,Transform target)
    {
        obj.GetComponent<Rigidbody>().isKinematic = false;
        float t = 0;
        while (t < EventManager.scaleTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        obj.gameObject.AddComponent<ChildTo>();
        obj.GetComponent<ChildTo>().Initiate(target);
        yield return null;
    }

    private IEnumerator ExpandBag(float expand, int delay, Transform BoM, Transform target)
    {
        float cDelay = 0;
        while (cDelay<delay)
        {
            cDelay += Time.deltaTime * spawnBagDelay;
            yield return null;
        }
        float t = 0;
        while (t < expand)
        {
            t += Time.deltaTime;
            if (t > 1)
                t = 1.0f;
            BoM.localScale = Vector3.one * t;
            yield return null;
        }
        StartCoroutine(FallAndChildTo(BoM, target));
        yield return null;
    }

    IEnumerator doMovement(bool coin, float t, Transform target, Transform obj, bool side, bool destroy, int _month)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        Vector3 tarPos = target.position;
        Quaternion tarRot = target.rotation;
        if (destroy)
        {
            tarPos += target.up*.5f;
            if (obj.tag == "ModelOnTable") { }
            //tarPos += target.right * .3f; DEN SKAL SGU VÆRE I MIDTEN BROSKI
            else
                tarPos += target.right * Random.Range(.1f, .4f) + target.up * Random.Range(-.05f, .05f);
        }
        else
            tarPos += screen.right * Random.Range(-0.22f, 0.22f) + screen.up * Random.Range(-0.09f, 0.09f) - screen.forward * Random.Range(0.0f,0.001f);
        int s = side ? 1 : -1;
        if (!destroy && obj.tag != "ModelOnTable")
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
        else if (obj.tag != "ModelOnTable")
            tarRot = Random.rotation;
        float tt = 0;
        while (tt <= t)
        {
            tt += Time.deltaTime;
            obj.position = Vector3.Lerp(orgPos, tarPos, tt / t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, tt / t);
            yield return new WaitForEndOfFrame();
        }
   
        //obj.position = orgPos;
        //obj.rotation = tarRot;
        if (destroy && obj.tag!="ModelOnTable")
        {
            Destroy(obj.gameObject);
            //obj.GetComponent<Rigidbody>().isKinematic = false;
            //obj.tag = "Untagged";
        }
        else if(obj.tag == "ModelOnTable")
        {
            Vector3 tarScale = defaultModelScale;
            float ttt = 0;
            while (ttt <= defaultScaleTime)
            {
                ttt += Time.deltaTime;
                obj.localScale = Vector3.Lerp(Vector3.one, tarScale, ttt/EventManager.scaleTime);
                yield return null;
            }
            ttt = 0;
            tarScale = defaultModelScale * (1f + DataHandler.getScale(EventManager.CurrentCategory, _month));
            StartCoroutine(pac.ExpandCrateAt_Cat_Month(EventManager.CurrentCategory, _month, DataHandler.getScale(EventManager.CurrentCategory, _month)));
            while (ttt <= EventManager.scaleTime)
            {
                ttt += Time.deltaTime;
                if(obj.GetComponent<Collider>() == null)
                {
                    if (obj.GetComponentInChildren<Collider>().bounds.size.z < 1f && obj.GetComponentInChildren<Collider>().bounds.size.x < 1f)
                        obj.localScale = Vector3.Lerp(defaultModelScale, tarScale, ttt / EventManager.scaleTime);
                }
                else if(obj.GetComponent<Collider>().bounds.size.z < 1f && obj.GetComponent<Collider>().bounds.size.x < 1f)
                    obj.localScale = Vector3.Lerp(defaultModelScale, tarScale, ttt / EventManager.scaleTime);
                yield return null;
            }
            StartCoroutine(FallAndChildTo(obj, target));
            obj.tag = "ModelOnShelf";
            CreateBagsOfMoney(_month,target);
        }
        yield break;
    }
    IEnumerator doModelMovement()
    {
        yield break;
    }
    IEnumerator eventTrigger()
    {
        yield return new WaitForSeconds(maxTravelTime_crate+defaultScaleTime);
        EventManager.ObjectsPlacedAtShelves();
        yield return new WaitForSeconds(EventManager.scaleTime + (maxValueOfAMonth / BagValue)/spawnBagDelay );
        EventManager.CategoryDone();
    }
    public void MoveMoneyToScreen()
    {
        maxValueOfAMonth = 0;
        EmptyCrate = new List<int>();
        int sum1000 = 0, sum500 = 0, sum200 = 0, sum100 = 0, sum50 = 0, sum20 = 0, sum10 = 0, sum5 = 0, sum2 = 0, sum1 = 0;
        currencyFound = new List<List<GameObject>>();
        res = DataHandler.BillsAtCategory_Month[EventManager.CurrentCategory];
        for(int x = 0; x<res.Length; ++x)
        {
            sum1000 += res[x]._1000;
            sum500 += res[x]._500;
            sum200 += res[x]._200;
            sum100 += res[x]._100;
            sum50 += res[x]._50;
            sum20 += res[x]._20;
            sum10 += res[x]._10;
            sum5 += res[x]._5;
            sum2 += res[x]._2;
            sum1 += res[x]._1;
            if(res[x]._1000 + res[x]._500 + res[x]._200 + res[x]._100 + res[x]._50 + res[x]._20 + res[x]._10 + res[x]._5 + res[x]._2 + res[x]._1 == 0)
            {
                EmptyCrate.Add(x);
                pac.FlipCrate(EventManager.CurrentCategory, x); //flipper det lidt for tidligt måske?!
            }
            else 
            {
                if (res[x]._1000 * 1000 + res[x]._500 * 500 + res[x]._200 * 200 + res[x]._100 * 100 + res[x]._50 * 50 + res[x]._20 * 20 + res[x]._10 * 10 + res[x]._5 * 5 + res[x]._2 * 2 + res[x]._1 > maxValueOfAMonth)
                    maxValueOfAMonth = res[x]._1000 * 1000 + res[x]._500 * 500 + res[x]._200 * 200 + res[x]._100 * 100 + res[x]._50 * 50 + res[x]._20 * 20 + res[x]._10 * 10 + res[x]._5 * 5 + res[x]._2 * 2 + res[x]._1;
            }
        }
        List<GameObject> _1000 = new List<GameObject>(), _500 = new List<GameObject>(), _200 = new List<GameObject>(), 
            _100 = new List<GameObject>(), _50 = new List<GameObject>(), _20 = new List<GameObject>(), 
            _10 = new List<GameObject>(), _5 = new List<GameObject>(), _2 = new List<GameObject>(), _1 = new List<GameObject>();
        GameObject[] search = GameObject.FindGameObjectsWithTag("1000kr");
        Vector3 pos = screen.position;
        Quaternion rot = screen.rotation;
        bool side = true;
        for(int x = 0; x< sum1000; ++x)
        {
            _1000.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false,month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("500kr");
        for(int x = 0; x<sum500; ++x)
        {
            _500.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("200kr");
        for (int x = 0; x < sum200; ++x)
        {
            _200.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("100kr");
        for (int x = 0; x < sum100; ++x)
        {
            _100.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("50kr");
        for (int x = 0; x < sum50; ++x)
        {
            _50.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("20kr");
        for (int x = 0; x < sum20; ++x)
        {
            _20.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("10kr");
        for (int x = 0; x < sum10; ++x)
        {
            _10.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
        }
        search = GameObject.FindGameObjectsWithTag("5kr");
        for (int x = 0; x < sum5; ++x)
        {
            _5.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("2kr");
        for (int x = 0; x < sum2; ++x)
        {
            _2.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("1kr");
        for (int x = 0; x < sum1; ++x)
        {
            _1.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), screen, search[x].transform, side, false, month));
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
    Transform t;
    public void MoveMoneyToCrate()
    {
        int bill = 0;
        GameObject[] model = GameObject.FindGameObjectsWithTag("ModelOnTable");
        int models = model.Length;
        
        for (int x = 0; x<res.Length; ++x)
        {
            if (EmptyCrate.Contains(x))
                continue;
            
            month = x;
            bill = 0;
            //print(DataHandler.expenseData[EventManager.CurrentCategory, month]);
            t = pac.GetCrate(EventManager.CurrentCategory, x);
                for (int l = 0; l < res[x]._1000; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._500; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._200; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._100; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._50; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._20; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._10; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._5; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._2; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
            bill++;
            for (int l = 0; l < res[x]._1; ++l)
            {
                StartCoroutine(doMovement(false, Random.Range(minTravelTime_crate, maxTravelTime_crate), t, currencyFound[bill][0].transform, true, true, month));
                currencyFound[bill].Remove(currencyFound[bill][0]);
            }
           
            
            StartCoroutine(doMovement(false, minTravelTime_crate, t, model[model.Length-models].transform, false, true, month));
            --models;
        }
        StartCoroutine(eventTrigger());
    }
}
