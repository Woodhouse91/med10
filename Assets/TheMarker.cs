using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMarker : MonoBehaviour {

    public TextMesh totalText;
    float totalTextVal;
    public Transform TargetColumn;
    Transform transferTar;
    CurrencyHandler CH;
    [SerializeField]
    private float pickUpDetectTime, swipeSpeed, maxPickUpSpeed;
    private float detectTimer, velocity;
    private Vector3 prevPos;
    private List<Transform> bill;

    private void Awake()
    {
        detectTimer = 0;
        bill = new List<Transform>();
        CH = FindObjectOfType<CurrencyHandler>();
    }
    private void OnEnable()
    {
        detectTimer = 0;
        totalText.gameObject.SetActive(false);
        totalTextVal = 0;
    }
    public void MyDisable()
    {
        detectTimer = 0;
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
        velocity = (transform.position - prevPos).magnitude/Time.deltaTime;
        prevPos = transform.position;
        // keyboard controls
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * 0.15f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.15f);

        if (Input.GetKeyDown(KeyCode.Space))
            MyDisable();
        if (bill.Count > 0)
        {
            detectTimer += Time.deltaTime;
            if (detectTimer >= pickUpDetectTime)
                performAccuratePickup();
        }
        else
            detectTimer = 0;
    }
    private void performAccuratePickup()
    {
        bill.Sort(delegate (Transform a, Transform b)
        {
            return ((a.position-transform.position).magnitude.CompareTo((b.position-transform.position).magnitude));
        });
        CH.PickUp(bill[0], transform);
        bill.Remove(bill[0]);
        detectTimer = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Currency" && other.GetComponent<currency>().canPickUp && velocity <= maxPickUpSpeed)
        {
            if (!bill.Contains(other.transform))
            {
                bill.Add(other.transform);
            }
        }
        else if (other.tag == "ColumnSection")
        {
            TargetColumn = other.transform;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Currency" && other.GetComponent<currency>().canPickUp && velocity <= maxPickUpSpeed)
        {
            if (!bill.Contains(other.transform))
            {
                bill.Add(other.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Currency")
        {
            bill.Remove(other.transform);
        }
            if (other.tag == "ColumnSection")
        {

        }
    }




}
