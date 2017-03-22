using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoneyToTable : MonoBehaviour {

    Transform spawnArea;
    float Ax, Ay, Az;
    Vector3 centerpoint;
    bool spawnNextNow = true;
    // Use this for initialization
    void Start () {
        spawnArea = transform.GetChild(2); //the lids are 0 and 1
        Ax = spawnArea.lossyScale.x/2f;
        Ay = spawnArea.lossyScale.y/2f;
        Az = spawnArea.lossyScale.z/2f;
        centerpoint = spawnArea.localPosition;
       // StartCoroutine(spawnAllTheMoney(myArray));
    }
	
	// Update is called once per frame
	void Update () {
        //insideArea(Thousand);
	}
    public void ShowMeTheMoney()
    {
        StartCoroutine(spawnAllTheMoney());
    }

    public IEnumerator spawnAllTheMoney()
    {
        int y = 0;
        while (y < MoneyHolder.allCurrency.Count)
        {
            for(float x = 0; x < 1/90f; x+=Time.deltaTime)
            {
                Vector3 pos = new Vector3(Random.Range(-Ax, Ax), Random.Range(-Ay, Ay), Random.Range(-Az, Az)) + centerpoint;
                pos = transform.TransformPoint(pos);
                MoneyHolder.allCurrency[y].SetActive(true);
                MoneyHolder.allCurrency[y].transform.position = pos;
                MoneyHolder.allCurrency[y].transform.rotation = Quaternion.AngleAxis(Random.Range(1, 360), Vector3.right) * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.up) * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.forward);
                y++;
                if (y < MoneyHolder.allCurrency.Count)
                    break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        EventManager.StartNextCategory();
        GetComponent<BoxBehaviour>().Throw();
        yield break;
    }
}
