using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyIntoButton : MonoBehaviour {
    [SerializeField]
    private Color normalColor, hoverColor, pressColor, disabledColor;
    private Color orgColor, matOrgColor;
    private Vector3 orgPos, pressPos;
    private bool isHovered = false, inTransition = false;
    private bool hovering = false;
    private bool normalling = false;
    public enum state { Hovered, Pressed, Normal}
    [HideInInspector]
    public state curState;
    private state prevState;
    private float transitionTime = 0.2f;
    private bool pressed = false;

    private void setColors(Color c)
    {
        GetComponent<SpriteRenderer>().color = orgColor * c;
        transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.color = matOrgColor * c;
    }
    private void Start()
    {
        EventManager.OnUIPlaced += loadPos;
    }
    private void loadPos()
    {
        orgPos = transform.position;
        pressPos = transform.TransformPoint(new Vector3(0, 0, -0.01f));
        transform.position = pressPos;
        Material newMat = new Material(transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial);
        newMat.name = "" + GetInstanceID();
        transform.GetChild(0).GetComponent<MeshRenderer>().material = newMat;
        orgColor = GetComponent<SpriteRenderer>().color;
        matOrgColor = transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.color;
        curState = state.Normal;
        prevState = curState;
        StartCoroutine(Activate());
    }
    public void setSprite(Sprite s)
    {
        GetComponent<SpriteRenderer>().sprite = s;
    }
    private void Update()
    {
        if (prevState != curState && !pressed)
        {
            switch (curState)
            {
                case state.Normal:
                    StartCoroutine(Normal());
                    break;
                case state.Hovered:
                    StartCoroutine(Hover());
                    break;
                case state.Pressed:
                    pressed = true;
                    StartCoroutine(Press());
                    break;
            }
            prevState = curState;
        }
    }
    private IEnumerator Hover()
    {
        Vector3 snapPos = transform.position;
        Color snapColor = GetComponent<SpriteRenderer>().color;
        float t = 0;
        while (curState == state.Hovered)
        {
            if (t <= transitionTime)
            {
                float normT = t / transitionTime;
                setColors(Color.Lerp(snapColor, hoverColor, normT));
                transform.position = Vector3.Lerp(snapPos, orgPos, normT);
                t += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        yield break;
    }
    private IEnumerator Normal()
    {
        Vector3 snapPos = transform.position;
        Color snapColor = GetComponent<SpriteRenderer>().color;
        float t = 0;
        while (curState == state.Normal)
        {
            if (t <= transitionTime)
            {
                float normT = t / transitionTime;
                setColors(Color.Lerp(snapColor, normalColor, normT));
                transform.position = Vector3.Lerp(snapPos, orgPos, normT);
                t += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        yield break;
    }
    private IEnumerator Press()
    {
        Vector3 snapPos = transform.position;
        Color snapColor = GetComponent<SpriteRenderer>().color;
        float t = 0;
        EventManager.CategorySliderDone();
        while (t <= transitionTime)
        {
            float normT = t / transitionTime;
            setColors(Color.Lerp(snapColor, pressColor, normT));
            transform.position = Vector3.Lerp(snapPos, pressPos, normT);
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
    private IEnumerator Activate()
    {
        setColors(disabledColor);
        Vector3 snapPos = transform.position;
        float t = 0;
        while (t <= transitionTime)
        {
            setColors(Color.Lerp(disabledColor, normalColor, t / transitionTime));
            transform.position = Vector3.Lerp(snapPos, orgPos,t);
            t += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(Normal());
        gameObject.SetActive(false);
        yield break;
    }
}
