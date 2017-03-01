using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void UIAction(GameObject go);
    public event UIAction OnUIActionClick;
    public event UIAction OnUIActionDoubleClick;
    public event UIAction OnUIActionHoverBegin;
    public event UIAction OnUIActionHoverEnd;
    public event UIAction OnUIActionSelect;
    public event UIAction OnUIActionDeselect;
    public event UIAction OnUIActionPress;
    public event UIAction OnUIActionRelease;
    public event UIAction OnUIActionDragBegin;
    public event UIAction OnUIActionDragEnd;

    public delegate void UIElementAction(GameObject go);
    public event UIElementAction OnUIElementAdd;
    public event UIElementAction OnUIElementRemove;
    public event UIElementAction OnUIElementSplit;

    //NOT DONE


    void Start () {
		
	}
	
	void Update () {
		
	}
}
