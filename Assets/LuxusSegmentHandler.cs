using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuxusSegmentHandler : MonoBehaviour {
    private static Vector3 offset = new Vector3(0, 0, .175f);
    private static float scaledOffset = 0;
    private static Vector3 basePos = Vector3.forward * -.56f;
    private static Transform segmentPrefab;
    private static Transform holder, nHolder;
    private static List<Transform> segment;
    private static List<Transform> activeSegments;
    private static Material NormalMat, MarkedMat, FlaggedMat;
    private static List<int> flaggedList;
    private static GameObject flagPref;
    private static GameObject[] flagObj;
    public static List<Transform> completedSegments
    {
        get
        {
            return _completedSegments;
        }
    }
    private static List<Transform> _completedSegments;
    private static float RotAngle = 55f;
    private static float slideTime = 2f;
    private static float rotTime = 1.5f;
    private int releases = 0;
    private float moneyMoveTime = 2f;
    private float moneyH = 0.072f;
    private float moneyW = 0.165f;
    private float coinS = 0.00285f;
    private int coinsPerCol = 4;
    private int billsPerColumn = 25;
    private float hangHeight = 0f;
    private float secHangHeight = -.6f;
    private float defaultLeft = 1.25f;
    private float hangTime = 2f;
    private bool stillMoving;
    private static int prevHighlight = -1;
    private static List<List<Transform>> bills;
    private static List<List<Transform>> coins;
    private Transform targetWall;
    private float luxusOffset = 0;
    private static int markedCount = 0;
    private static Vector3 flagScale = Vector3.one * 0.1f;
    private static float _xRot = 0, _yRot = 0, _zRot = 60;
    private static Quaternion xRot = Quaternion.AngleAxis(_xRot, Vector3.right);
    private static Quaternion yRot = Quaternion.AngleAxis(_yRot, Vector3.up);
    private static Quaternion zRot = Quaternion.AngleAxis(_zRot, Vector3.forward);


    // Use this for initialization
    private void Start ()
    {
        targetWall = GameObject.Find("Tavlefar").transform;
        _completedSegments = new List<Transform>();
        bills = new List<List<Transform>>();
        coins = new List<List<Transform>>();
        activeSegments = new List<Transform>();
        EventManager.OnBoxAtTable += GenerateTable;
        EventManager.OnRipTapeSliderDone += MoveMoneyToTable;
        segmentPrefab = Resources.Load<Transform>("LuxusSegment");
        holder = GameObject.Find("Segmentholder").transform;
        EventManager.OnExcelDataLoaded += PoolSegments;
        EventManager.OnCategoryDone += ReleaseTable;
        staticStart();
	}
    private static void staticStart()
    {
        flagPref = Resources.Load<GameObject>("flagObj");
        MarkedMat = Resources.Load<Material>("MarkedSectionMat");
        NormalMat = Resources.Load<Material>("SectionMat");
        FlaggedMat = Resources.Load<Material>("FlaggedSectionMat");
        flaggedList = new List<int>();
    }
    void Unsub()
    {
        try
        {
            EventManager.OnBoxAtTable -= GenerateTable;
            EventManager.OnRipTapeSliderDone -= MoveMoneyToTable;
            EventManager.OnExcelDataLoaded -= PoolSegments;
            EventManager.OnCategoryDone -= ReleaseTable;
        }
        catch { }
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void MoveMoneyToTable()
    {
        Transform tar;
        for(int x = 0; x<activeSegments.Count; ++x)
        {
            List<float> offset = new List<float>();
            tar = activeSegments[x].GetChild(1).GetChild(0);
            float down = -moneyH/5f;
            int cols =  (bills[x].Count / billsPerColumn)+1;
            
            float coinRight = coinS*3f;
            float coinDown = -coinS*6f;

            int coinColCount = 0;

            for (int k = 0; k<cols; ++k)
            {
                offset.Add((moneyW * (((-cols/2f) +k)+.5f)));
            }
            if (x < bills.Count)
            {
                for (int y = 0; y < bills[x].Count; ++y)
                {
                    StartCoroutine(doBillMovement(bills[x][y], tar, down * (y/cols), offset[y % offset.Count]));
                }
            }
            if (x < coins.Count)
            {
                for(int y = 0; y<coins[x].Count; ++y)
                {
                    StartCoroutine(doCoinMovement(coins[x][y], tar, coinDown*(y/coinsPerCol)-coinDown, coinRight*coinColCount));
                    ++coinColCount;
                    if ((y + 1) % (coinsPerCol) == 0)
                        coinColCount = 0;
                }
            }
        }
    }

    private IEnumerator doCoinMovement(Transform obj, Transform tar, float downOff, float rightOff)
    {
        while (stillMoving)
            yield return null;
        float t = 0;
        Vector3 orgPos = obj.position;
        Vector3 target = tar.position + tar.up * downOff + tar.right * rightOff-tar.forward*0.02f;
        Quaternion orgRot = obj.rotation;
        Quaternion tarRot = tar.rotation*Quaternion.AngleAxis(-105, Vector3.right);
        tarRot *= Quaternion.AngleAxis(-15, Vector3.forward);
        while (t <= 1)
        {
            t += Time.deltaTime / moneyMoveTime;
            obj.position = Vector3.Lerp(orgPos, target, t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            yield return null;
        }
        obj.SetParent(tar.parent.parent.parent);
        yield break;
    }

    private IEnumerator doBillMovement(Transform obj, Transform tar, float downOff, float colOff)
    {
        while (stillMoving)
            yield return null;
        float t = 0;
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        Quaternion tarRot = tar.rotation*Quaternion.AngleAxis(15, -Vector3.right);
        Vector3 target = tar.position + tar.up * downOff + tar.right * colOff;
        while (t <= 1)
        {
            t += Time.deltaTime/moneyMoveTime;
            obj.position = Vector3.Lerp(orgPos, target, t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            yield return null;
        }
        obj.SetParent(tar.parent.parent.parent);
        yield break;
    }
    private void GenerateTable(BoxBehaviour s)
    {
        nHolder = Instantiate(holder);
        int scaleFactor = 0;
        float prevScaled = 0;
        bool scaled = false;
        for (int x = 0; x < s.CategoryString.Count; ++x)
        {
            activeSegments.Add(segment[0]);
            segment.Remove(segment[0]);
            activeSegments[x].SetParent(holder);
            scaleFactor = buildMoneyList(s.CategoryInt[x])/billsPerColumn;
            activeSegments[x].localPosition = (offset * x) + offset * scaledOffset + offset * (scaleFactor/2f);
            scaledOffset += (scaleFactor);
            activeSegments[x].localScale += Vector3.up*scaleFactor;
            activeSegments[x].localRotation = Quaternion.Euler(90, -180, 0);
            activeSegments[x].GetChild(0).GetChild(0).GetComponent<TextMesh>().text = FormatHandler.FormatCategory(s.CategoryString[x]);
            activeSegments[x].GetChild(0).GetChild(1).GetComponent<TextMesh>().text = FormatHandler.FormatCurrency(DataHandler.expenseData[13, s.CategoryInt[x]]);
            Vector3 texScale = activeSegments[x].GetChild(0).GetChild(0).localScale;
            texScale.x /= (scaleFactor+1);
            activeSegments[x].GetChild(0).GetChild(0).localScale = texScale;
            activeSegments[x].GetChild(0).GetChild(1).localScale = texScale;
            activeSegments[x].gameObject.SetActive(true);
            prevScaled = scaleFactor;
        }
        scaledOffset += scaled ? .5f : 0;
        markObjGen(activeSegments.Count);
        ExposeTable();
    }
    private static void markObjGen(int c)
    {
        flagObj = new GameObject[c];
    }
    private string formatCurrency(int val)
    {
        decimal moneyvalue = val;
        string res = String.Format("{0:N}", moneyvalue);
        res = res.Remove(res.Length - 3);
        res = res.Replace(",", ".");
        res += ",-";
        return res;
    }
    private void ExposeTable()
    {
        StartCoroutine(moveTable());
    }
    private IEnumerator moveTable()
    {
        stillMoving = true;
        Vector3 orgPos = holder.localPosition;
        Vector3 tarPos = orgPos + basePos-offset * (activeSegments.Count-1)-offset*scaledOffset;
        Quaternion orgRot = holder.rotation;
        Quaternion tarRot = holder.rotation * Quaternion.AngleAxis(RotAngle, -Vector3.forward);
        float t;
        #region Slideout
        t = 0;
        while (t <= slideTime)
        {
            t += Time.deltaTime;
            holder.transform.localPosition = Vector3.Lerp(orgPos, tarPos, t / slideTime);
            yield return null;
        }
        #endregion
        #region Rotate
        t = 0;
        while(t<= rotTime)
        {
            t += Time.deltaTime;
            holder.rotation = Quaternion.Slerp(orgRot, tarRot, t / rotTime);
            yield return null;
        }
        #endregion
        stillMoving = false;
        yield break;
    }
    public void ReleaseTable()
    {
        releases++;
        prevHighlight = -1;
        for (int x =0; x<activeSegments.Count; ++x)
        {
            if (!flaggedList.Contains(x))
                setMat(x, NormalMat);
            else
                setMat(x, FlaggedMat);
        }
        flaggedList = new List<int>();
        int segs = activeSegments.Count;
        activeSegments = new List<Transform>();
        nHolder.SetParent(holder.parent);
        nHolder.localPosition = Vector3.zero;
        nHolder.localRotation = Quaternion.identity;
        StartCoroutine(doRelease(holder, segs));
        holder = nHolder;
    }

    private IEnumerator doRelease(Transform obj, int s)
    {
        bills = new List<List<Transform>>();
        coins = new List<List<Transform>>();
        Vector3 orgPos = obj.position;
        Quaternion orgRot = obj.rotation;
        Transform myRef = obj;
        Vector3 target = targetWall.position - targetWall.right * (defaultLeft-luxusOffset) + targetWall.forward*hangHeight + targetWall.up*0.02f;
        for (int x = 0; x < s; ++x)
        {
            luxusOffset += obj.GetChild(x).GetComponent<MeshCollider>().bounds.size.x;
        }
        if (luxusOffset > 2.9f)
        {
            luxusOffset = 0;
            hangHeight = secHangHeight;
            target = targetWall.position - targetWall.right * (defaultLeft - luxusOffset) + targetWall.forward * hangHeight + targetWall.up * 0.02f;
            for (int x = 0; x < s; ++x)
            {
                luxusOffset += obj.GetChild(x).GetComponent<MeshCollider>().bounds.size.x;
            }
        }
        Quaternion tarRot = targetWall.rotation*Quaternion.AngleAxis(90, Vector3.right)*Quaternion.AngleAxis(90, Vector3.up)*Quaternion.AngleAxis(-90, Vector3.forward);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / (hangTime / 2f);
            obj.position = Vector3.Lerp(orgPos, targetWall.position + targetWall.right * 1.5f, t);
            obj.rotation = Quaternion.Lerp(orgRot, tarRot, t);
            yield return null;
        }
        orgPos = obj.position;
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / (hangTime / 2f);
            obj.position = Vector3.Lerp(orgPos, target, t);
            yield return null;
        }
        scaledOffset = 0;
        yield break;
    }
    private static int buildMoneyList(int cat)
    {
        List<Transform> resBills = new List<Transform>();
        List<Transform> resCoins = new List<Transform>();
        DataHandler.billRef[] myRef = DataHandler.BillsAtCategory_Month[cat];
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f1000 = 0; f1000 < myRef[x]._1000; ++f1000)
            {
                resBills.Add(MoneyHolder.getCurrency(1000));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f500 = 0; f500 < myRef[x]._500; ++f500)
            {
                resBills.Add(MoneyHolder.getCurrency(500));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f200 = 0; f200 < myRef[x]._200; ++f200)
            {
                resBills.Add(MoneyHolder.getCurrency(200));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f100 = 0; f100 < myRef[x]._100; ++f100)
            {
                resBills.Add(MoneyHolder.getCurrency(100));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f50 = 0; f50 < myRef[x]._50; ++f50)
            {
                resBills.Add(MoneyHolder.getCurrency(50));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f20 = 0; f20 < myRef[x]._20; ++f20)
            {
                resCoins.Add(MoneyHolder.getCurrency(20));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f10 = 0; f10 < myRef[x]._10; ++f10)
            {
                resCoins.Add(MoneyHolder.getCurrency(10));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f5 = 0; f5 < myRef[x]._5; ++f5)
            {
                resCoins.Add(MoneyHolder.getCurrency(5));
            }
        }
        for (int x = 0; x < myRef.Length; ++x)
        {
            for (int f2 = 0; f2 < myRef[x]._2; ++f2)
            {
                resCoins.Add(MoneyHolder.getCurrency(2));
            }
        }
        for(int x = 0; x<myRef.Length; ++x)
        {
            for (int f1 = 0; f1 < myRef[x]._1; ++f1)
            {
                resCoins.Add(MoneyHolder.getCurrency(1));
            }
        }
       
       coins.Add(resCoins);
       bills.Add(resBills);
        return resBills.Count;
    }

    private static void PoolSegments()
    {
        if (segment == null)
            segment = new List<Transform>();
        for(int x = 0; x< DataHandler.BudgetCategories.Count; ++x)
        {
            segment.Add(Instantiate(segmentPrefab));
            segment[x].gameObject.SetActive(false);
        }
    }
    public static void HighlightCategory(int cat)
    {
        if (prevHighlight != -1)
        {
            if (!flaggedList.Contains(prevHighlight))
                setMat(prevHighlight, NormalMat);
            else
                setMat(prevHighlight, FlaggedMat);
        }
        if (prevHighlight == cat)
        {
            prevHighlight = -1;
            return;
        }
        prevHighlight = cat;
        setMat(cat, MarkedMat);
    }
    public static void FlagCategory(int cat)
    {
        if (flaggedList.Contains(cat))
        {
            setMat(cat, NormalMat);
            flaggedList.Remove(cat);
            Destroy(flagObj[cat]);
            return;
        }
        flagObj[cat] = Instantiate(flagPref, activeSegments[cat].position, activeSegments[0].parent.rotation*xRot*yRot*zRot);
        flagObj[cat].transform.SetParent(activeSegments[0].parent);
        flagObj[cat].transform.localScale = flagScale;
        flagObj[cat].transform.SetAsLastSibling();
        flaggedList.Add(cat);
        if(prevHighlight!=cat)
            setMat(cat, FlaggedMat);
    }
    private static void setMat(int cat, Material dst)
    {
        activeSegments[cat].GetComponent<MeshRenderer>().material = dst;
        for (int i = 0; i < 2; i++)
        {
            activeSegments[cat].GetChild(i).GetComponent<MeshRenderer>().material = dst;
        }
    }
}
