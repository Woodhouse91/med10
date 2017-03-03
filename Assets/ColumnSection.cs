using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColumnSection : MonoBehaviour {

    public string SectionName;
    Transform HighLight;
    public Transform MoneySpace;
    TextMesh Name;
    public List<Transform>[] ListCurrency = new List<Transform>[11];

    // Use this for initialization
    private void OnApplicationQuit()
    {
        for (int i = 0; i < ListCurrency.Length; i++)
        {
            ListCurrency[i].Clear();
        }
    }
    void OnEnable() {
        for (int i = 0; i < ListCurrency.Length; i++)
            ListCurrency[i] = new List<Transform>();
        if (Name == null)
            Name = transform.FindChild("SectionName").GetComponent<TextMesh>();
        if(HighLight == null)
        {
            HighLight = transform.FindChild("Highlight");
        }
        if (MoneySpace == null)
            MoneySpace = transform.FindChild("MoneySpace");
        HighLight.gameObject.SetActive(false);
        Name.text = SectionName;

    }
	
	// Update is called once per frame
    public void Highlight(bool on)
    {
        HighLight.gameObject.SetActive(on);
    }
	void Update () {
       
    }
    public void PlaceMoney()
    {
        float SectionValue= 0;
        //if(TOO MANY MONEYS)
        float moneyOffsetPlacement = 0.03f;
        for (int k = 0; k < 11; ++k) // 0-4 BILLS  // 5-10 Coins
        {
            for (int i = 0; i < ListCurrency[k].Count; i++)
            {
                SectionValue += ListCurrency[k][i].GetComponent<currency>().CurrencyValue;
                StartCoroutine(PlaceMoneyInSpace(k, i, MoneySpace.position + -MoneySpace.transform.up * moneyOffsetPlacement));
                moneyOffsetPlacement += 0.025f;
            }
        }
    }
    IEnumerator PlaceMoneyInSpace(int k, int i, Vector3 pos)
    {
        float t = 0;    
        while(t<1)
        {
            ListCurrency[k][i].position = Vector3.Lerp(ListCurrency[k][i].position, pos, t);
            ListCurrency[k][i].transform.LookAt(ListCurrency[k][i].transform.position + transform.forward);
            t += Time.deltaTime;
            yield return null;
        }
        ListCurrency[k][i].Rotate(Vector3.right * -5f);
        yield return null;
    }
}
