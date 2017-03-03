using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColumnSection : MonoBehaviour {

    public string SectionName;
    Transform HighLight;
    TextMesh Name;
    public List<Transform>[] ListCurrency = new List<Transform>[11];

    // Use this for initialization

    void OnEnable() {
        for (int i = 0; i < ListCurrency.Length; i++)
            ListCurrency[i] = new List<Transform>();
        if (Name == null)
            Name = transform.FindChild("SectionName").GetComponent<TextMesh>();
        if(HighLight == null)
        {
            HighLight = transform.FindChild("Highlight");
        }
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
        
    }
}
