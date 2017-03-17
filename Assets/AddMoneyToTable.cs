using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoneyToTable : MonoBehaviour {

    Transform spawnArea;
    float Ax, Ay, Az;
    Vector3 centerpoint;
    public GameObject Thousand, FiveHundred, TwoHundred, OneHundred, Fifty;
    public GameObject Twenty, Ten, Five, Two, One;
    public bool spawnNextNow = true;
	// Use this for initialization
	void Start () {
        spawnArea = transform.GetChild(0);
        Ax = spawnArea.lossyScale.x;
        Ay = spawnArea.lossyScale.y;
        Az = spawnArea.lossyScale.z;
        centerpoint = spawnArea.localPosition;
        int[,] myArray = {
                  {12341, 1234,4123},
                  {4989, 4789, 4489},
                  {1794, 1701, 4489},
                  {37898, 3789, 3889},
                    {12341, 1234,4123},
                  {4989, 4789, 4489},
                  {8794, 1701, 44489},
              };
        StartCoroutine(spawnAllTheMoney(myArray));
    }
	
	// Update is called once per frame
	void Update () {
        //insideArea(Thousand);
	}
 
    public IEnumerator spawnAllTheMoney(int[,] arr)
    {

        int x = arr.GetLength(0);
        int y = arr.GetLength(1);
       
        for (int i = 0; i < y; i++)
        {
            for (int k = 0; k < x-1; k++)
            {
                yield return new WaitUntil(() => spawnNextNow == true);

                if (i == 0)
                {
                    // i GET ALL THE CATEGORIES
                }
                else if(arr[k, i] > 0)
                {
                    spawnNextNow = false;
                    spawnMoney(arr[k, i]);
                }

            }
        }
      
    }
    void spawnMoney(int money)
    {
        int restMoney = money;

        StartCoroutine(ModulusMoney(restMoney, 1000, Thousand));
        restMoney %= 1000;
        StartCoroutine(ModulusMoney(restMoney, 500, FiveHundred));
        restMoney %= 500;
        StartCoroutine(ModulusMoney(restMoney, 200, TwoHundred));
        restMoney %= 200;
        StartCoroutine(ModulusMoney(restMoney, 100, OneHundred));
        restMoney %= 100;
        StartCoroutine(ModulusMoney(restMoney, 50, Fifty));
        restMoney %= 50;
        StartCoroutine(ModulusMoney(restMoney, 20, Twenty));
        restMoney %= 20;
        StartCoroutine(ModulusMoney(restMoney, 10, Ten));
        restMoney %= 10;
        StartCoroutine(ModulusMoney(restMoney, 5, Five));
        restMoney %= 5;
        StartCoroutine(ModulusMoney(restMoney, 2, Two));
        restMoney %= 2;
        StartCoroutine(ModulusMoney(restMoney, 1, One));
    }
    IEnumerator ModulusMoney(int m, int bill, GameObject go)
    {
        for (int i = 0; i < m / bill; i++)
        {
            yield return new WaitForEndOfFrame();
            Vector3 pos = new Vector3(Random.Range(-Ax / 2f, Ax / 2f), Random.Range(-Ay / 2f, Ay / 2f), Random.Range(-Az / 2f, Az / 2f)) + centerpoint;
            pos = transform.TransformPoint(pos);
            Instantiate(go, pos, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right) * Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up)* Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward));
           
        }
        if (((float)m / bill)%1 == 0)
            spawnNextNow = true;

    }

    
}
