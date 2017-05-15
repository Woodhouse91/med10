using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input(x);
        }
    }

    private int x = 1;
    private void Update()
    {

        if (Input.GetKey(KeyCode.Alpha1))
            x = 1;
        if (Input.GetKey(KeyCode.Alpha2))
            x = 2;
        if (Input.GetKey(KeyCode.Alpha3))
            x = 3;
        if (Input.GetKey(KeyCode.Alpha4))
            x = 4;


        transform.position = controller.transform.pos;
        transform.rotation = controller.transform.rot;
    }
}
