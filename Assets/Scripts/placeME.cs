using UnityEngine;
using System.Collections;
public class placeME : MonoBehaviour
{

	// Use this for initialization

	public SteamVR_TrackedController _controllerL, _controllerR;
    public Vector2 ScreenSizeCm;
    public Vector3[] placement = new Vector3[4]; //upLeft, downLeft, upRight, downRight
    public int xPlace = 0;
    Virtualscreen vc;
    public void Start()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        transform.LookAt(new Vector3(PlayerPrefs.GetFloat("normX"), PlayerPrefs.GetFloat("normY"), PlayerPrefs.GetFloat("normZ")));
        transform.localScale = new Vector3(ScreenSizeCm.x, ScreenSizeCm.y, 0);
        vc = FindObjectOfType<Virtualscreen>();
        vc.Setpos();
        EventManager.UIPlaced();
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
        //GameObject[] s = GameObject.FindGameObjectsWithTag("s");
        //for (int ss = 0; ss < s.Length; ++ss)
        //{
        //    Destroy(s[ss]);
        //}
        Vector3 mid = placement[0] + (placement[3] - placement[0]) / 2.0f;
        transform.position = mid;
        Vector3 norm = transform.position + Vector3.Cross(placement[1] - placement[0], placement[3] - placement[0]);
        transform.LookAt(norm);
        // transform.localRotation = new Quaternion(90, 0, 0, 1);
        //transform.localScale = new Vector3(Mathf.Abs(placement[2].x - placement[3].x), Mathf.Abs(placement[0].z - placement[1].z),0 );
        PlayerPrefs.SetFloat("PosZ", mid.z);
        PlayerPrefs.SetFloat("PosX", mid.x);
        PlayerPrefs.SetFloat("PosY", mid.y);
        PlayerPrefs.SetFloat("normZ", norm.z);
        PlayerPrefs.SetFloat("normX", norm.x);
        PlayerPrefs.SetFloat("normY", norm.y);
        vc.Setpos();
        EventManager.UIPlaced();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("PosZ", transform.position.z);
        PlayerPrefs.SetFloat("PosX", transform.position.x);
        PlayerPrefs.SetFloat("PosY", transform.position.y);
        PlayerPrefs.SetFloat("normZ", (transform.position + transform.forward).z);
        PlayerPrefs.SetFloat("normX", (transform.position + transform.forward).x);
        PlayerPrefs.SetFloat("normY", (transform.position + transform.forward).y);
    }
}
