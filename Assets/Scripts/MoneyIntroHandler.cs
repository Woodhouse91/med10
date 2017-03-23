using System.Collections;
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
    public static List<Transform> model;
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

    }
    private void Unsub()
    {
        EventManager.OnBoxEmptied -= MoveMoneyToScreen;
        EventManager.OnCategorySliderDone -= MoveMoneyToCrate;
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
    IEnumerator doMovement(bool coin, float t, Vector3 tarPos, Quaternion tarRot, Transform obj, bool side, bool destroy)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        if (destroy)
        {
            tarPos += this.t.up*.5f;
            if (obj.tag == "ModelOnTable")
                tarPos += this.t.right * .3f;
            else
                tarPos += this.t.right * Random.Range(.1f, .4f) + this.t.up * Random.Range(-.05f, .05f);
        }
        else
            tarPos += screen.right * Random.Range(-0.22f, 0.22f) + screen.up * Random.Range(-0.09f, 0.09f);
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
            tarRot = new Quaternion(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359), 1);
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
            obj.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(obj.gameObject);
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
            tarScale = defaultModelScale * (1f + DataHandler.getScale(EventManager.CurrentCategory, month));
            while (ttt <= EventManager.scaleTime)
            {
                ttt += Time.deltaTime;
                obj.localScale = Vector3.Lerp(defaultModelScale, tarScale, ttt / EventManager.scaleTime);
                yield return null;
            }
            obj.SetParent(this.t, true);
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.tag = "ModelOnShelf";
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
        yield return new WaitForSeconds(EventManager.scaleTime);
        EventManager.CategoryDone();
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
                pac.FlipCrate(EventManager.CurrentCategory, x); //flipper det lidt for tidligt måske?!
                return;
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
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("500kr");
        for(int x = 0; x<sum500; ++x)
        {
            _500.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("200kr");
        for (int x = 0; x < sum200; ++x)
        {
            _200.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("100kr");
        for (int x = 0; x < sum100; ++x)
        {
            _100.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("50kr");
        for (int x = 0; x < sum50; ++x)
        {
            _50.Add(search[x]);
            StartCoroutine(doMovement(false, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("20kr");
        for (int x = 0; x < sum20; ++x)
        {
            _20.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("10kr");
        for (int x = 0; x < sum10; ++x)
        {
            _10.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
        }
        search = GameObject.FindGameObjectsWithTag("5kr");
        for (int x = 0; x < sum5; ++x)
        {
            _5.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("2kr");
        for (int x = 0; x < sum2; ++x)
        {
            _2.Add(search[x]);
            StartCoroutine(doMovement(true, Random.Range(minTravelTime_screen, maxTravelTime_screen), pos, rot, search[x].transform, side, false));
            side = !side;
        }
        search = GameObject.FindGameObjectsWithTag("1kr");
        for (int x = 0; x < sum1; ++x)
        {
            _1.Add(search[x]);
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
    Transform t;
    public void MoveMoneyToCrate()
    {
        int bill = 0;
        GameObject[] model = GameObject.FindGameObjectsWithTag("ModelOnTable");
        for (int x = 0; x<res.Length; ++x)
        {
            month = x;
            bill = 0;
            t = pac.GetCrate(EventManager.CurrentCategory, x);
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
            StartCoroutine(doMovement(false, minTravelTime_crate, t.position, t.rotation, model[x].transform, false, true));
        }
        StartCoroutine(eventTrigger());
    }
}
