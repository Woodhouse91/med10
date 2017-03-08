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
	
	// Update is called once per frame
    public void Highlight(bool on)
    {
        HighLight.gameObject.SetActive(on);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Marker")
            Highlight(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Marker")
            Highlight(false);
    }

}
