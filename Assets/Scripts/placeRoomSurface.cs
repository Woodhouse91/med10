using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeRoomSurface : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    private void Awake()
    {
        EventManager.OnUIPlaced += setRoomPos;
    }

    private void setRoomPos()
    {
        transform.rotation = new Quaternion(0, target.rotation.y, 0, target.rotation.w);
        transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
        Vector3 tarpos = target.position + transform.right * offset.x + transform.forward * offset.z;
        tarpos.y = 0;
        transform.position = tarpos;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Unsub()
    {
        EventManager.OnUIPlaced -= setRoomPos;
    }
    private void OnApplicationQuit()
    {
        Unsub();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void OnDisable()
    {
        Unsub();
    }
}
