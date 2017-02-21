using UnityEngine;
using System.Collections;

public class placeME : MonoBehaviour
{

	// Use this for initialization

	public SteamVR_TrackedController _controllerL, _controllerR;
    public Vector2 ScreenSizeCm;

    public Vector3[] placement = new Vector3[4]; //upLeft, downLeft, upRight, downRight
    public int xPlace = 0;
    public void Start()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        transform.localScale = new Vector3(ScreenSizeCm.x, ScreenSizeCm.y, 0);
    }

    private void OnEnable ()
	{
		//_controllerL = GetComponent<SteamVR_TrackedController> ();
		_controllerL.TriggerClicked += LeftHandleTriggerClicked;
		

		//_controllerR = GetComponent<SteamVR_TrackedController> ();
		_controllerR.TriggerClicked += RightHandleTriggerClicked;

        _controllerL.Gripped += ResetPlacement;
        _controllerR.Gripped += ResetPlacement;
	}
    private void ResetPlacement(object controller, ClickedEventArgs e)
    {
        xPlace = 0;
        GameObject[] s = GameObject.FindGameObjectsWithTag("s");
        for(int ss = 0; ss<s.Length; ++ss)
        {
            Destroy(s[ss]);
        }
    }
	private void OnDisable ()
	{
		_controllerL.TriggerClicked -= LeftHandleTriggerClicked;
	

		_controllerR.TriggerClicked -= RightHandleTriggerClicked;
		
	}


    private void LeftHandleTriggerClicked(object controller, ClickedEventArgs e)
    {
        if(xPlace < 4)
        {
            placement[xPlace] = _controllerL.transform.position + _controllerL.transform.forward * 0.05f + _controllerL.transform.up * -0.1f;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.tag = "s";
            sphere.transform.position = _controllerL.transform.position + _controllerL.transform.forward * 0.05f + _controllerL.transform.up * -0.1f;
            sphere.transform.localScale = Vector3.one * 0.1f;
            xPlace++;
            if (xPlace == 4)
                Placed();
        }
    }
    private void RightHandleTriggerClicked(object controller, ClickedEventArgs e)
    {
        if(xPlace < 4)
        {
            placement[xPlace] = _controllerR.transform.position + _controllerR.transform.forward * 0.05f + _controllerR.transform.up * -0.1f;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.tag = "s";
            sphere.transform.position = _controllerR.transform.position + _controllerR.transform.forward * 0.05f + _controllerR.transform.up * -0.1f;
            sphere.transform.localScale = Vector3.one * 0.1f;
            xPlace++;
            if (xPlace == 4)
                Placed();
        }
    }
    private void Placed()
    {
        GameObject[] s = GameObject.FindGameObjectsWithTag("s");
        for (int ss = 0; ss < s.Length; ++ss)
        {
            Destroy(s[ss]);
        }
        float z = (placement[0].z + placement[1].z + placement[2].z + placement[3].z) / 4.0f;
        float x = (placement[0].x + placement[1].x + placement[2].x + placement[3].x) / 4.0f;
        float y = (placement[0].y + placement[1].y + placement[2].y + placement[3].y) / 4.0f;
        print(placement[0] + ((placement[0] - placement[3]).normalized * ((placement[0] - placement[3]).magnitude / 2.0f)));
        transform.position = placement[0] + ((placement[0] - placement[3]).normalized * ((placement[0] - placement[3]).magnitude / 2.0f));
        GameObject fuck = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(fuck, 2f);
        fuck.transform.position = transform.position;
        fuck.transform.localScale = Vector3.one;
        Vector3 normT = (Vector3.Cross(placement[1], placement[0]) + Vector3.Cross(placement[3], placement[2])) / 2f;
        Vector3 normP = (Vector3.Cross(placement[0], placement[2]) + Vector3.Cross(placement[1], placement[3])) / 2f;
        transform.LookAt(transform.position + ((normP + normT) / 2f));
        // transform.localRotation = new Quaternion(90, 0, 0, 1);
        //transform.localScale = new Vector3(Mathf.Abs(placement[2].x - placement[3].x), Mathf.Abs(placement[0].z - placement[1].z),0 );
        PlayerPrefs.SetFloat("PosZ", z);
        PlayerPrefs.SetFloat("PosX", x);
        PlayerPrefs.SetFloat("PosY", y);
    }
    // Update is called once per frame
    void Update ()
	{
	
	}
}
