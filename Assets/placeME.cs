using UnityEngine;
using System.Collections;

public class placeME : MonoBehaviour
{

	// Use this for initialization

	public SteamVR_TrackedController _controllerL, _controllerR;

    public Vector3[] placement = new Vector3[4]; //up, down, left, right
    public int xPlace = 0;
    public void Start()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        transform.localScale = new Vector3(PlayerPrefs.GetFloat("ScaleX"), PlayerPrefs.GetFloat("ScaleY"), 0);
    }

    private void OnEnable ()
	{
		//_controllerL = GetComponent<SteamVR_TrackedController> ();
		_controllerL.TriggerClicked += LeftHandleTriggerClicked;
		

		//_controllerR = GetComponent<SteamVR_TrackedController> ();
		_controllerR.TriggerClicked += RightHandleTriggerClicked;
	

	}

	private void OnDisable ()
	{
		_controllerL.TriggerClicked -= LeftHandleTriggerClicked;
	

		_controllerR.TriggerClicked -= RightHandleTriggerClicked;
		
	}


    private void LeftHandleTriggerClicked(object controller, ClickedEventArgs e)
    {
        placement[xPlace] = _controllerL.transform.position + _controllerL.transform.forward * 0.05f + _controllerL.transform.up * -0.1f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = _controllerL.transform.position + _controllerL.transform.forward * 0.05f + _controllerL.transform.up * -0.1f;
        sphere.transform.localScale = Vector3.one * 0.1f;
        xPlace++;
        if (xPlace == 4)
            Placed();
    }
    private void RightHandleTriggerClicked(object controller, ClickedEventArgs e)
    {
        placement[xPlace] = _controllerR.transform.position + _controllerR.transform.forward * 0.05f + _controllerR.transform.up * -0.1f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = _controllerR.transform.position + _controllerR.transform.forward * 0.05f + _controllerR.transform.up * -0.1f;
        sphere.transform.localScale = Vector3.one * 0.1f;
        xPlace++;
        if (xPlace == 4)
            Placed();
    }
    private void Placed()
    {
        float z = placement[0].z + ( (placement[1].z - placement[0].z) / 2 );
        float x = placement[2].x + ((placement[3].x - placement[2].x) / 2);
        float y = (placement[0].y + placement[1].y+ placement[2].y+ placement[3].y) / 4;
        transform.position = new Vector3(x, y, z);
        // transform.localRotation = new Quaternion(90, 0, 0, 1);
        transform.localScale = new Vector3(Mathf.Abs(placement[2].x - placement[3].x), Mathf.Abs(placement[0].z - placement[1].z),0 );
        PlayerPrefs.SetFloat("ScaleX", Mathf.Abs(placement[2].x - placement[3].x));
        PlayerPrefs.SetFloat("ScaleY", Mathf.Abs(placement[0].z - placement[1].z));
        PlayerPrefs.SetFloat("PosZ", z);
        PlayerPrefs.SetFloat("PosX", x);
        PlayerPrefs.SetFloat("PosY", y);
    }
    // Update is called once per frame
    void Update ()
	{
	
	}
}
