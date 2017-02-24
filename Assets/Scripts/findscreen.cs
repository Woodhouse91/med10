using UnityEngine;
using System.Collections;

public class findscreen : MonoBehaviour {
    public Transform screen;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = screen.position;
        GetComponent<RectTransform>().sizeDelta = new Vector2(screen.localScale.x, screen.localScale.y);
	}
}
