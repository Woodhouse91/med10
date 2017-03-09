using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currency : MonoBehaviour {
    public float CurrencyValue;
    private CurrencyHandler ch;
    [SerializeField]
    private Material org, org_b, marked, marked_b;
    [HideInInspector]
    public Transform myListObj;
    MeshRenderer f, b;
    public bool isMarked
    {
        get
        {
            return _marked;
        }
    }
    private bool _marked;
    // Use this for initialization
    void Start()
    {
        f = GetComponent<MeshRenderer>();
        b = transform.GetChild(0).GetComponent<MeshRenderer>();
        ch = FindObjectOfType<CurrencyHandler>();

    }
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Y))
            ch.AddCurrency(GameObject.FindGameObjectWithTag("ColumnSection").transform, transform);
    }
    
    public void MarkBill(bool mark)
    {
        _marked = mark;
        f.material = mark ? marked : org;
        b.material = mark ? marked_b : org_b;
    }

}
