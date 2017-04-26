using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {

    RipTheTape tape;
    Transform leftLid, rightLid;
    Rigidbody rig;
    public Vector3 FORCEIT;
    public float SpawnRate;
    private Transform spawnArea;
    private float Ax,Ay,Az;
    public AnimationCurve AC;
    private int categoryModel;
    public List<string> CategoryString;
    public List<int> CategoryInt;
    public int[] moneyAtCrate;
    Transform MovingUp;
    BoxInterfaceScreen bis;

    private float BagValue;
    private int maxValueOfAMonth;
    float bagSize;
    public float spawnBagDelay = 1f;
    public Transform BagOfMoney;

    //fra money intro handler
    Vector3 defaultModelScale;
   
    PlaceAllCrates pac;

    private float defaultScaleTime = 1.5f;

    // Use this for initialization
    void Start () {
        MovingUp = GameObject.Find("MovingUp").transform;
        defaultModelScale = Vector3.one * 3;
        pac = FindObjectOfType<PlaceAllCrates>();
        bis = FindObjectOfType<BoxInterfaceScreen>();
        rig = GetComponent<Rigidbody>();
        leftLid = transform.GetChild(0);
        rightLid = transform.GetChild(1);
        tape = GetComponentInChildren<RipTheTape>();
        spawnArea = transform.GetChild(2); //the lids are 0 and 1
        Ax = spawnArea.lossyScale.x / 2.5f;
        Ay = spawnArea.lossyScale.y / 2.5f;
        Az = spawnArea.lossyScale.z / 2.5f;
        if(GetComponent<AddMoneyToTable>()==null) //KUN FOR MONEYKASSEN
            BagValue = DataHandler.incomeData[12] / 100f;
    }

    public void modelsForShelves(int category)
    {
        //string modelInt = GetComponentInChildren<Sprite>().name;
        categoryModel = category;
        CategoryInt = new List<int>();
        CategoryString = new List<string>();
        moneyAtCrate = new int[12];
        for (int k = 0; k < DataHandler.BudgetCategories.Count; k++)
        {
            if (DataHandler.expenseData[0, k] == category)
            {
                CategoryString.Add(DataHandler.BudgetCategories[k]);
                CategoryInt.Add(k);
            }
        }
        for (int i = 1; i < 13; i++)
        {
            for (int j = 0; j < CategoryInt.Count; j++)
            {
                moneyAtCrate[i - 1] += DataHandler.expenseData[i, CategoryInt[j]];
            }
        }
    }
    // Update is called once per frame
    public void tapeRipped()
    {
        EventManager.RipTapeSliderDone();
        if (GetComponent<AddMoneyToTable>() == null)
            StartCoroutine(FlipUp());
        else
            StartCoroutine(MoneyToTable());
        
    }
    public void setTapeRip(float dist)
    {
        tape.SetTapeDist(dist);
    }

    IEnumerator makeKinematic(float t, Transform obj)
    {
        yield return new WaitForSeconds(t);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        yield break;
    }
    IEnumerator LayerChange(Transform other, bool layer, float t)
    {
        yield return new WaitForSeconds(t);
        if(layer)
            other.gameObject.layer = LayerMask.NameToLayer("ignoreCur");
        else
            other.gameObject.layer = LayerMask.NameToLayer("Currency");
        yield return null;
    }


    IEnumerator MoneyToTable()
    {
        rig.isKinematic = true;
        //first we raise it.
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = EventManager.Table.position + EventManager.Table.up * 2f - EventManager.Table.right / 2f;
           
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime;
            yield return null;
        }
        //then we flip it.
        t = 0;  
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transform.rotation * Quaternion.AngleAxis(90f, Vector3.up) * Quaternion.AngleAxis(170f, Vector3.forward);  // ROTATE 180
        while (t < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
        //then we open the lids
        Quaternion startRotL = leftLid.localRotation;
        Quaternion startRotR = rightLid.localRotation;
        Quaternion endRotL = leftLid.localRotation * Quaternion.AngleAxis(-135f, Vector3.forward); // ROTATE LIDS 135
        Quaternion endRotR = rightLid.localRotation * Quaternion.AngleAxis(135f, Vector3.forward);
        t = 0;
        while (t < 1)
        {
            leftLid.localRotation = Quaternion.Slerp(startRotL, endRotL, AC.Evaluate(t));
            rightLid.localRotation = Quaternion.Slerp(startRotR, endRotR, AC.Evaluate(t));
            t += Time.deltaTime;
            yield return null;
        }
        GetComponent<AddMoneyToTable>().ShowMeTheMoney(); // SPAWN ALL THE MONIES PLZ
       
        yield break;
        
    }
    IEnumerator FlipUp()
    {
        rig.isKinematic = true;
        Vector3 shakeVec;
        Vector3 startpos = transform.position;
        float t = 0;
        
        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            shakeVec = new Vector3(Random.Range(-AC.Evaluate(t), AC.Evaluate(t)), Random.Range(-AC.Evaluate(t), AC.Evaluate(t)), Random.Range(-AC.Evaluate(t), AC.Evaluate(t)));
            transform.position = startpos + shakeVec / 100f;
            yield return null;
        }
        
        //then we open the lids
        Quaternion startRotL = leftLid.localRotation;
        Quaternion startRotR = rightLid.localRotation;
        Quaternion endRotL = leftLid.localRotation * Quaternion.AngleAxis(-160f, Vector3.forward); // ROTATE LIDS 70% of 160 
        Quaternion endRotR = rightLid.localRotation * Quaternion.AngleAxis(160f, Vector3.forward);
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime *2;
            leftLid.localRotation = Quaternion.Slerp(startRotL, endRotL, AC.Evaluate(t));
            rightLid.localRotation = Quaternion.Slerp(startRotR, endRotR, AC.Evaluate(t));
            yield return null;
        }
        //SPAWN MODELS
       

        for (int i = 0; i < 12; i++)
        {
            if(moneyAtCrate[i] > 0)
            {
                GameObject model = CategoryModelHandler.GetAt(categoryModel).gameObject; // MODELS HERE THO
                Vector3 pos = transform.position + new Vector3(Random.Range(-Ax, Ax), Random.Range(-Ay, Ay), Random.Range(-Az, Az));
                Vector3 forceUp = new Vector3(0.1f, 5.5f, 0.1f);
                //pos = transform.TransformPoint(pos);
                model = Instantiate(model, pos, Quaternion.AngleAxis(Random.Range(1, 360), Vector3.right) * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.up) * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.forward));
                model.GetComponent<Rigidbody>().isKinematic = false;
                model.GetComponent<Rigidbody>().AddForce(forceUp, ForceMode.VelocityChange);
                StartCoroutine(FlyModelsToShelves(model.transform,i));
                yield return new WaitForSeconds(1f / SpawnRate);
            }
            else
            {
                pac.FlipCrate(EventManager.CurrentCategory, i);
            }
        }
        yield return new WaitForSeconds(0.5f);
        EventManager.BoxEmptied();
        Throw();
        yield return null;
    }

    private IEnumerator FlyModelsToShelves(Transform model,int CrateMonth)
    {
        float t = 0;
        while (model.GetComponent<Rigidbody>().velocity.y > -1.0f && t< 1f)
        {
            t += Time.deltaTime;
            yield return null;
        }
        model.GetComponent<Rigidbody>().isKinematic = true;
        Transform target = pac.GetCrate(EventManager.CurrentCategory, CrateMonth);
        Vector3 endPos = pac.GetCrate(EventManager.CurrentCategory, CrateMonth).position + Vector3.up * .5f;
        Vector3 startPos = model.position;
        Quaternion startRot = model.rotation;
        Quaternion endRot = pac.GetCrate(EventManager.CurrentCategory, CrateMonth).rotation;
        t = 0;
        while (t<1f)
        {
            t += Time.deltaTime / 2f;
            model.position = Vector3.Lerp(startPos,endPos,t);
            model.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }
        model.SetParent(MovingUp, true);
        Vector3 tarScale = defaultModelScale;
        float ttt = 0;
        while (ttt <= defaultScaleTime)
        {
            ttt += Time.deltaTime;
            model.localScale = Vector3.Lerp(Vector3.one, tarScale, ttt / EventManager.scaleTime);
            yield return null;
        }
        ttt = 0;
        tarScale = defaultModelScale * (1f + moneyAtCrate[CrateMonth]*12f / DataHandler.tExpense); // times 12 because of single crate expanding
        StartCoroutine(pac.ExpandCrateAt_Cat_Month(EventManager.CurrentCategory, CrateMonth, moneyAtCrate[CrateMonth] * 12f / DataHandler.tExpense));
        while (ttt <= EventManager.scaleTime)
        {
            ttt += Time.deltaTime;
            if (model.GetComponent<Collider>() == null)
            {
                if (model.GetComponentInChildren<Collider>().bounds.size.z < 1f && model.GetComponentInChildren<Collider>().bounds.size.x < 1f)
                    model.localScale = Vector3.Lerp(defaultModelScale, tarScale, ttt / EventManager.scaleTime);
            }
            else if (model.GetComponent<Collider>().bounds.size.z < 1f && model.GetComponent<Collider>().bounds.size.x < 1f)
                model.localScale = Vector3.Lerp(defaultModelScale, tarScale, ttt / EventManager.scaleTime);
            yield return null;
        }
       StartCoroutine(FallAndChildTo(model, target));
        model.tag = "ModelOnShelf";
       CreateBagsOfMoney(CrateMonth, target);
        yield return null;
    }

    public void Throw()
    {
        rig.isKinematic = false;
        Vector3 forceDir = EventManager.Table.right * FORCEIT.x + EventManager.Table.forward * FORCEIT.z + EventManager.Table.up * FORCEIT.y;
        rig.AddForce(forceDir);
        rig.AddTorque(forceDir);
    }
    void CreateBagsOfMoney(int _month, Transform target)
    {
        float bags = moneyAtCrate[_month] / BagValue;

        for (int i = 0; i < bags; i++)
        {
            Transform BoM = Instantiate(BagOfMoney);
            if (bagSize == 0)
            {
                bagSize = BoM.GetComponent<Collider>().bounds.size.y;
                print("bagsize er : " + bagSize);
            }
            BoM.localScale = Vector3.zero;
            BoM.position = target.position + Vector3.up * (target.GetComponent<Collider>().bounds.size.y * 0.8f - bagSize / 2f) + Vector3.right * (target.GetComponent<Collider>().bounds.size.x * 0.8f * Random.Range(-0.4f, 0.4f));
            BoM.rotation = target.rotation;
            BoM.gameObject.AddComponent<ChildTo>().Initiate(target);
            if (bags - i >= 1f)
                StartCoroutine(ExpandBag(1f, i, BoM, target));
            else
                StartCoroutine(ExpandBag(bags % 1, i, BoM, target));
        }

    }

    private IEnumerator FallAndChildTo(Transform obj, Transform target)
    {
        if (obj.GetComponent<ChildTo>())
            Destroy(obj.GetComponent<ChildTo>());
        if (obj.GetComponent<Rigidbody>() != null)
            obj.GetComponent<Rigidbody>().isKinematic = false;
        else
            obj.gameObject.AddComponent<Rigidbody>();
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
        while (cDelay < delay)
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
}