using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceWeight : MonoBehaviour {

    GameObject måler;
    GameObject pil;
    public Material red, green;
    GameObject currentMoneyBag;
    public GameObject MoneyBagPrefab;
    float totalIncome;
    public Transform IncomePos, ExpencePos;
    public float SizeOfBag = 2.0f;
    public AnimationCurve AC; // AC Går fra 0.0f til 0.5f
    public float curExpence = 0;
    float totalAngle = 35f; //BGA AC
    int totalCategories = 0;
    float curAngle;
	// Use this for initialization
	void Start () {
       
        green = GetComponent<MeshRenderer>().material;
        red = GameObject.Find("CylinderRød").GetComponent<MeshRenderer>().material;
        måler = GameObject.Find("måler");
        pil = GameObject.Find("Pil");
       
       // EventManager.OnBoxEmptied += BoxEmptied;
        EventManager.OnBoxAtTable += BoxAtTable;
        EventManager.OnRipTapeSliderDone += BoxOpened;
        
    }
    private void Unsub()
    {
       //EventManager.OnBoxEmptied -= BoxEmptied;
        EventManager.OnBoxAtTable -= BoxAtTable;
        EventManager.OnRipTapeSliderDone -= BoxOpened;
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
    // Update is called once per frame
    void Update () {
		
	}
    void BoxAtTable(BoxBehaviour BB)                            // BOX AT TABLE
    {
        float categoryTotal = 0;
        for (int i = 0; i < BB.moneyAtCrate.Length; i++)
        {
            categoryTotal += BB.moneyAtCrate[i];
        }
        curExpence += categoryTotal;
        curAngle = ((curExpence / totalIncome) - 1f) * totalAngle;
        CreateBagOfMoney(categoryTotal);
    }
    void BoxOpened()                                            //BOX TAPE RIPPED
    {
        if (totalCategories == 0) //for Moneykassen
        {
            totalIncome = DataHandler.tIncome > DataHandler.tExpense ? DataHandler.tIncome : DataHandler.tExpense;
            green.SetColor("_EmissionColor", Color.green * 1.0f); // later
            red.SetColor("_EmissionColor", Color.red * 0.0f);
            GameObject.Find("venstreGruppeSkål").AddComponent<ChildTo>().Initiate(GameObject.Find("venstreVip").transform);
            GameObject.Find("højreGruppeSkål").AddComponent<ChildTo>().Initiate(GameObject.Find("højreVip").transform);
            totalCategories = DataHandler.tCombinedCategories;
            CreateBagOfMoney(-1337);
            
        }
        else    // for Modelkasserne
        {
            DropBagOfMoney();
        }
    }
   
    void BoxEmptied()                                          //BOX EMPTIED (SLÅET FRA)
    {
        //FOR inkomst pengene 
        curAngle = ((curExpence / totalIncome) - 1f) * totalAngle; 
        RefreshWeight();
    }
    void RefreshWeight()
    {
        StartCoroutine(RotateScaleSlow());
        if (curAngle > 0) // hitting minus
        {
            green.SetColor("_EmissionColor", Color.green * 0.0f); 
            red.SetColor("_EmissionColor", Color.red * 1.0f);
        }
    }

    private IEnumerator RotateScaleSlow()
    {
        Quaternion rot = måler.transform.localRotation;
        Quaternion pilRot = pil.transform.localRotation;
        Quaternion newPilRot = Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(-curAngle * 3f, Vector3.forward); // 3 gange mere rotation for pilen
        Quaternion newRot = Quaternion.AngleAxis(-curAngle, Vector3.up);
        float t = 0;
        while(t < 1) //BGA AC skal alting ganges med 2
        {
            t += Time.deltaTime; 
            pil.transform.localRotation = Quaternion.Slerp(pilRot, newPilRot, t) * Quaternion.AngleAxis(-Quaternion.Angle(pilRot, newPilRot) * AC.Evaluate(t),Vector3.forward);
            måler.transform.localRotation = Quaternion.Slerp(rot, newRot, t) * Quaternion.AngleAxis(-Quaternion.Angle(rot, newRot) * AC.Evaluate(t), Vector3.up);
            yield return null;
        }
        yield return null;
    }

    void CreateBagOfMoney(float size)
    {
        if(size == -1337) //Create bags of money for income
        {
            StartCoroutine(SpawnIncomeBags());
            curAngle = ((curExpence / totalIncome) - 1f) * totalAngle; // TOTAL INCOME SKAL VÆRE DET BARCHARTS FINDER I STEDET (nico pico giver)
            return;
        }
        currentMoneyBag = Instantiate(MoneyBagPrefab);
        currentMoneyBag.transform.position = ExpencePos.position;
        currentMoneyBag.transform.localScale = Vector3.one * CubicRoot(size / totalIncome) * SizeOfBag; // coroutine
        currentMoneyBag.GetComponent<Rigidbody>().isKinematic = true;
    }
    void DropBagOfMoney()
    {
        StartCoroutine(DelayChildTo(currentMoneyBag));
    }
    IEnumerator SpawnIncomeBags()
    {
        
        for (int i = 0; i < 12; i++)
        {
            currentMoneyBag = Instantiate(MoneyBagPrefab);
            currentMoneyBag.transform.position = IncomePos.position + Random.insideUnitSphere* 0.05f;
            currentMoneyBag.transform.localScale = Vector3.one * CubicRoot(DataHandler.incomeData[i] / totalIncome) * SizeOfBag; // coroutine
            currentMoneyBag.GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(1 / 12f);
            StartCoroutine(DelayFirstMoneyBags(currentMoneyBag));
        }
        yield return new WaitForSeconds(0.5f);
        RefreshWeight();
    }
    IEnumerator DelayFirstMoneyBags(GameObject cmb)
    {
        yield return new WaitForSeconds(0.5f);
        cmb.AddComponent<ChildTo>().Initiate(IncomePos);
        yield return null;
    }
    IEnumerator DelayChildTo(GameObject cmb)
    {
        currentMoneyBag.GetComponent<Rigidbody>().isKinematic = false;
        currentMoneyBag.GetComponent<Rigidbody>().velocity = Vector3.down * 0.05f;
        while (cmb.GetComponent<Rigidbody>().velocity.magnitude > 0.0f)
        {
            yield return null;
        }
        RefreshWeight();
        cmb.AddComponent<ChildTo>().Initiate(ExpencePos);
        yield return null;
    }
    float CubicRoot(float n)
    {
        float root = Mathf.Pow(n, (1.0f / 3.0f));
        return root;
    }
}
