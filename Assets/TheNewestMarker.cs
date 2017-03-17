using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheNewestMarker : MonoBehaviour
{

    private Ray ray;
    private RaycastHit hit;
    private int touchScreenLayer, dragLayer;
    private Vector3 prevPos, dragDir, orgDrop, dragOffset;
    private Transform draggedTar;


    private void Awake()
    {

        touchScreenLayer = 1 << LayerMask.NameToLayer("TouchScreen");
        dragLayer = 1 << LayerMask.NameToLayer("DragLayer");
    }

    private void FixedUpdate()
    {
        ray.origin = transform.position - transform.forward * 0.5f;
        ray.direction = transform.forward;
        dragDir = transform.position - prevPos;
        if (Physics.Raycast(ray, out hit, 1, touchScreenLayer))
        {

        }
        if (Physics.Raycast(ray, out hit, 1, dragLayer))
        {
            if(draggedTar == null)
            {
                dragOffset = transform.position - draggedTar.position;
                draggedTar = hit.transform;
            }
        }
    }
    public void releaseSlider()
    {

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * 0.15f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.15f);
        if (draggedTar != null)
        {
            draggedTar.transform.position = transform.position - dragOffset;
        }
    }

}
