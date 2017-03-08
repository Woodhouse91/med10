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
       // if (Name == null)
       //     Name = transform.FindChild("SectionName").GetComponent<TextMesh>();
        if(HighLight == null)
        {
            HighLight = transform.FindChild("Highlight");
        }
        if (MoneySpace == null)
            MoneySpace = transform.FindChild("MoneySpace");
        HighLight.gameObject.SetActive(false);
        //Name.text = SectionName;

    }
	
	// Update is called once per frame
    public void Highlight(bool on)
    {
        HighLight.gameObject.SetActive(on);
    }
	void Update () {
       
    }
}
