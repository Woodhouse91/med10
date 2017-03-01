using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColumnSection : MonoBehaviour {

    public string SectionName;
    Transform HighLight;
    TextMesh Name;
    // Use this for initialization

	void OnEnable() {
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
}
