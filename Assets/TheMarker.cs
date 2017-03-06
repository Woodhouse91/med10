using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMarker : MonoBehaviour {

    public TextMesh totalText;
    float totalTextVal;
    public Transform TargetColumn;
    Transform transferTar;
    CurrencyHandler CH;


    private void Awake()
    {
        CH = FindObjectOfType<CurrencyHandler>();
    }
    private void OnEnable()
    {
        totalText.gameObject.SetActive(false);
        totalTextVal = 0;
    }
    public void MyDisable()
    {
        if (TargetColumn == null)
        {
            gameObject.SetActive(false);
            return; // HER SKAL DEN I MONEYBALL
        }
        CH.TransferCurrency(TargetColumn, transform);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {

        // keyboard controls
        transform.Translate(Vector3.up * Input.GetAxis("Vertical")* Time.deltaTime * 0.15f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.15f);

        if (Input.GetKeyDown(KeyCode.Space))
            MyDisable();        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Currency")
        {
            CH.PickUp(other.transform, transform);
        }
        else if (other.tag == "ColumnSection")
        {
            TargetColumn = other.transform;
            other.GetComponent<ColumnSection>().Highlight(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ColumnSection")
        {
            other.GetComponent<ColumnSection>().PlaceMoney();
            other.GetComponent<ColumnSection>().Highlight(false);
        }
    }




}
