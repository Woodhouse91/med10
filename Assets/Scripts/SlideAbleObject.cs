using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAbleObject : MonoBehaviour
{
    private Vector3 orgPos;
    private enum DirBehaviour { Horizontal, Vertical, TowardsTarget, Free }
    private enum DestBehaviour { Stay, Destroy, ReturnToOrigin }
    private enum ReleaseBehaviour { Stay, Return, CarryMomentum}
    [SerializeField]
    private ReleaseBehaviour ReleaseSetting;
    [SerializeField]
    private DirBehaviour DirectionSetting;
    [SerializeField]
    private DestBehaviour DestinationSetting;
    [HideInInspector]
    public Transform target, slider, area;
    private float dist, normDist;
    [SerializeField]
    private float returnSpeed;
    TheNewestMarker owner;
    private bool returning = false;
    private BoxBehaviour bh;
    private cardBoardManager cbm;
    private GameObject[] models;

    private void Start()
    {
        target = transform.GetChild(1);
        area = transform.GetChild(2);
        slider = transform.GetChild(0);
        bh = FindObjectOfType<BoxBehaviour>();
        cbm = FindObjectOfType<cardBoardManager>();
        EventManager.OnUIPlaced += SetVariables;
        if (DirectionSetting == DirBehaviour.Horizontal)
            EventManager.OnBoxAtTable += NextCategory;
        else
           EventManager.OnBoxEmptied += NextObject;
    }

    private void NextCategory()
    {
        bh = cbm.CardBoxList[EventManager.CurrentCategory].GetComponent<BoxBehaviour>();
        for (int x = 0; x < transform.childCount; ++x)
        {
            transform.GetChild(x).gameObject.SetActive(true);
        }
    }

    private void Unsub()
    {
        if (DirectionSetting == DirBehaviour.Horizontal)
            EventManager.OnBoxAtTable -= NextCategory;
        else
            EventManager.OnBoxEmptied -= NextObject;
        EventManager.OnUIPlaced -= SetVariables;
    }

    private void NextObject()
    {
        for (int x = 0; x < transform.childCount; ++x)
        {
            transform.GetChild(x).gameObject.SetActive(true);
        }
        models = GameObject.FindGameObjectsWithTag("ModelOnTable");
        for(int x = 0; x<models.Length; ++x)
        {
            StartCoroutine(controlModel(models[x].transform));
        }
    }

    IEnumerator controlModel(Transform obj)
    {
        yield return new WaitForSeconds(.5f);
        Vector3 origin = obj.position, tar, dir;
        origin.y = 0;
        while (slider.gameObject.activeSelf)
        {
            while (owner!=null)
            {
                tar = origin + Vector3.up * (1+normDist);
                dir = tar - obj.position;
                obj.position += dir * (1+dir.sqrMagnitude) * .1f * Time.deltaTime;
                float t = Time.deltaTime / 2f*dir.magnitude;
                obj.position += Vector3.right * UnityEngine.Random.Range(-t, t) + Vector3.forward * UnityEngine.Random.Range(-t, t);
                yield return null;
            }
            yield return null;
        }
        yield break;
    }

    public void TakeControl(TheNewestMarker script)
    {
        if (owner == null)
        {
            owner = script;
        }
    }
    public void ReleaseControl()
    {
        owner = null;
        switch (ReleaseSetting)
        {
            case ReleaseBehaviour.Return:
                if (!returning)
                    StartCoroutine(Return());
                break;
            case ReleaseBehaviour.Stay:
                break;
            case ReleaseBehaviour.CarryMomentum:
                StartCoroutine(Glide());
                break;
        }
        if(models!=null)
            for(int x = 0; x<models.Length; ++x)
            {
                models[x].GetComponent<Rigidbody>().isKinematic = false;
            }
    }

    private IEnumerator Glide()
    {

        yield break;
    }

    private void SetVariables()
    {
        orgPos = slider.position;
        Vector3 tar = transform.InverseTransformPoint(target.position);
        Vector3 slid = transform.InverseTransformPoint(slider.position);
        tar.z = 0;
       
        switch (DirectionSetting)
        {
            case DirBehaviour.Horizontal:
                tar.y = slid.y;
                break;
            case DirBehaviour.Vertical:
                tar.x = slid.x;
                break;
            default:
                break;
        }
        target.position = transform.TransformPoint(tar);
        normDist = 0;
        dist = Vector3.Distance(slider.position, target.position);
        if (DirectionSetting == DirBehaviour.Horizontal)
            EventManager.OnBoxAtTable += NextCategory;
        else
        {
            for (int x = 0; x < transform.childCount; ++x)
            {
                transform.GetChild(x).gameObject.SetActive(false);
            }
        }
    }
    public void setOwnerPosition(Vector3 pos)
    {
        if (DataHandler.dataCompleted)
        {
            Vector3 moddedPos = slider.InverseTransformPoint(pos);
            if(models!=null)
                for (int x = 0; x < models.Length; ++x)
                {
                    models[x].GetComponent<Rigidbody>().isKinematic = true;
                }
            switch (DirectionSetting)
            {
                case DirBehaviour.Horizontal:
                    moddedPos.y = 0;
                    moddedPos.z = 0;
                    break;
                case DirBehaviour.Vertical:
                    moddedPos.z = 0;
                    moddedPos.x = 0;
                    break;
            }
            moddedPos = slider.TransformPoint(moddedPos);
            normDist = 1f - Vector3.Distance(moddedPos, target.position) / dist;
            if (normDist<0)
            {
                normDist = 0;
                moddedPos = orgPos;
            }
            if(normDist >= 0.95f)
            {
                normDist = 1;
                moddedPos = target.position;
                owner.releaseSlider();
                owner = null;
                if (DirectionSetting == DirBehaviour.Horizontal)
                    bh.setTapeRip(1f);
                DestinationInvoke();
           
                switch (DestinationSetting)
                {
                    case DestBehaviour.Destroy:
                        instantReturn();
                        slider.gameObject.SetActive(false);
                        area.gameObject.SetActive(false);
                        target.gameObject.SetActive(false);
                        return;
                    case DestBehaviour.ReturnToOrigin:
                        StartCoroutine(Return());
                        break;
                }
            
            }
            if(bh!=null)
                if (DirectionSetting == DirBehaviour.Horizontal)
                    bh.setTapeRip(normDist);
            slider.position = moddedPos;
        }
    }

    private void DestinationInvoke()
    {
        bh = null;
        normDist = 0;
        if (DirectionSetting == DirBehaviour.Horizontal)
            EventManager.RipTapeSliderDone();
        else
            EventManager.CategorySliderDone();
       
    }
    private void instantReturn()
    {
        slider.position = orgPos;
    }
    private IEnumerator Return()
    {
        if(returnSpeed==0)
        {
            slider.position = orgPos;
            yield break;
        }
        returning = true;
        Vector3 dir = -(slider.position - orgPos).normalized;
        while (Vector3.Distance(slider.position, orgPos)>0.05f && owner==null)
        {
            slider.position += dir * returnSpeed * Time.deltaTime;
            if (DirectionSetting == DirBehaviour.Horizontal)
            {
                normDist = 1f - Vector3.Distance(slider.position, target.position) / dist;
                bh.setTapeRip(normDist);
            }
            yield return null;
        }
        returning = false;
        if(owner != null)
            yield break;
        slider.position = orgPos;
        if (DirectionSetting == DirBehaviour.Horizontal)
        {
            bh.setTapeRip(0f);
        }
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
