using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMarker : MonoBehaviour {


    float followSpeed = 0.50000f;
    List<Transform> pickedUp;
    TextMesh totalText;
    float totalTextVal;
    Transform TargetColumn;

    // Use this for initialization
	private void OnEnable()
    {
        pickedUp = new List<Transform>();
        totalText = transform.GetComponentInChildren<TextMesh>();
        totalText.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        // FLYT PIKKENE( PENGENE) PÅ BORDET
    }
    // Update is called once per frame
    void Update () {

        // keyboard controls
        transform.Translate(Vector3.up * Input.GetAxis("Vertical")* Time.deltaTime * 0.05f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.05f);

        for (int i = 0; i < pickedUp.Count; i++)
        {
            pickedUp[i].position = Vector3.Slerp(pickedUp[i].position, transform.position - Vector3.forward* 0.01f, followSpeed * Time.deltaTime);

        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Currency")
        {
            if (totalText.gameObject.activeSelf == false)
                totalText.gameObject.SetActive(true);
        
            other.transform.SetParent(transform);

            totalTextVal += other.transform.GetComponent<currency>().CurrencyValue;
            totalText.text = totalTextVal.ToString();
            pickedUp.Add(other.transform);
        }
        if (other.tag == "ColumnSection")
        {
            TargetColumn = other.transform;
            other.GetComponent<ColumnSection>().Highlight(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ColumnSection")
        {
            other.GetComponent<ColumnSection>().Highlight(false);
        }
    }




}
