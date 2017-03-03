using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    private Color orgCol;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float clickGrey, hoverGrey, selectGrey;
    bool clicked = false, hovered = false, selected = false;
    void Start()
    {
        orgCol = GetComponent<Image>().material.color;
    }

    void OnApplicationQuit()
    {
        GetComponent<Image>().material.color = orgCol;
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PointerExit");
        hovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PointerClick");
        selected = !selected;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
        selected = hovered;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
