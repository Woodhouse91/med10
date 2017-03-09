using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColumnSection : MonoBehaviour {

    public string SectionName;
    Transform HighLight;
    public Transform MoneySpace;
    TextMesh Name;
    // Use this for initialization
  
    void OnEnable() {
      
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
	

}
