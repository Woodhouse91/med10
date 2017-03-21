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
    private bool slideReleaseWait = true;


    private void Awake()
    {

        touchScreenLayer = 1 << LayerMask.NameToLayer("TouchScreen");
        dragLayer = 1 << LayerMask.NameToLayer("DragLayer");
    }

    private void Update()
    {
        ray.origin = transform.position - transform.forward * 0.5f;
        ray.direction = transform.forward;
        dragDir = transform.position - prevPos;
        if (Physics.Raycast(ray, out hit, 1, touchScreenLayer))
        {

        }
        if (Physics.Raycast(ray, out hit, 1, dragLayer) && draggedTar == null && slideReleaseWait)
        {
            draggedTar = hit.transform;
            dragOffset = transform.position - draggedTar.position;
            draggedTar.GetComponent<SlideAbleObject>().TakeControl(this);
        }
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * 0.15f);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 0.15f);
        if (draggedTar != null)
        {
            draggedTar.GetComponent<SlideAbleObject>().setOwnerPosition(transform.position - dragOffset);
        }
    }
    public void releaseSlider()
    {
        draggedTar = null;
        StartCoroutine(slideWait());
    }
    IEnumerator slideWait()
    {
        slideReleaseWait = false;
        yield return new WaitForSeconds(1f);
        slideReleaseWait = true;
    }
    private void OnDisable()
    {
        if (draggedTar != null)
        {
            draggedTar.GetComponent<SlideAbleObject>().ReleaseControl();
            draggedTar = null;
        }
    }

}
