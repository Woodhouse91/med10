using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void UIAction();
    public static event UIAction OnUIPlaced;

    public delegate void LoadAction();
    public static event LoadAction OnExcelDataLoaded;
    public static event LoadAction OnMoneyInstantiated;

    public delegate void IntroAction();
    public static event IntroAction OnBoxEmptied;
    public static event IntroAction OnRipTapeSliderDone;
    public static event IntroAction OnCategorySliderDone;
    public static event IntroAction OnStartNextCategory;
    public static event IntroAction OnCategoryDone;
    public static event IntroAction OnBoxAtTable;
    public static event IntroAction OnObjectsPlacedAtShelves;
    public static event IntroAction OnStartScale;
    public static event IntroAction OnCategoryFinished;
    public static float scaleTime
    {
        get
        {
            return 1f;
        }
    }
    private static int _currentCategory = 0;
    public static int CurrentCategory { get { return _currentCategory; } }
    private static Transform _table;
    public static Transform Table
    {
        get
        {
            if (_table == null)
                _table = GameObject.Find("Table").transform;
            return _table;
        }
    }
    public static void StartScale()
    {
        if (OnStartScale != null)
            OnStartScale();
    }
    public static void ObjectsPlacedAtShelves()
    {
        if (OnObjectsPlacedAtShelves != null)
            OnObjectsPlacedAtShelves();
    }
    public static void BoxAtTable()
    {
        if (OnBoxAtTable != null)
            OnBoxAtTable();
    }
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
    public static void BoxEmptied()
    {
        if (OnBoxEmptied != null)
            OnBoxEmptied();
    }
    public static void RipTapeSliderDone()
    {
        if (OnRipTapeSliderDone != null)
            OnRipTapeSliderDone();
    }
    public static void CategorySliderDone()
    {
        if (OnCategorySliderDone != null)
            OnCategorySliderDone();
    }
    public static void CategoryDone()
    {
        _currentCategory++;
        if(_currentCategory >= DataHandler.BudgetCategories.Count)
        {
            if (OnCategoryFinished != null)
                OnCategoryFinished();
        }
        else if (OnCategoryDone != null)
            OnCategoryDone();
    }
    public static void StartNextCategory()
    {
        if (OnStartNextCategory != null)
            OnStartNextCategory();
    }
}
