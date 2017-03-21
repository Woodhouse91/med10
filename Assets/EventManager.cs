using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void UIAction(GameObject go);
    public static event UIAction OnUIActionClick;
    public static event UIAction OnUIActionDoubleClick;
    public static event UIAction OnUIActionHoverBegin;
    public static event UIAction OnUIActionHoverEnd;
    public static event UIAction OnUIActionSelect;
    public static event UIAction OnUIActionDeselect;
    public static event UIAction OnUIActionPress;
    public static event UIAction OnUIActionRelease;
    public static event UIAction OnUIActionDragBegin;
    public static event UIAction OnUIActionDragEnd;

    public delegate void UIElementAction();
    public static event UIElementAction OnUIElementAdd;
    public static event UIElementAction OnUIElementRemove;
    public static event UIElementAction OnUIElementSplit;
    public static event UIElementAction OnUIPlaced;

    public delegate void LoadAction();
    public static event LoadAction OnExcelDataLoaded;
    public static event LoadAction OnMoneyInstantiated;

    public static void UIPlaced()
    {
        if (OnUIPlaced != null)
            OnUIPlaced();
    }
    public static void ExcelDataLoaded()
    {
        if (OnExcelDataLoaded != null)
            OnExcelDataLoaded();
    }
    public static void MoneyInstantiated()
    {
        if (OnMoneyInstantiated != null)
            OnMoneyInstantiated();
    }
}
