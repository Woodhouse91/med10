using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {
    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input(1);
        }
    }

    
    private void Update()
    {
        transform.position = controller.transform.pos;
        transform.rotation = controller.transform.rot;
    }
}
