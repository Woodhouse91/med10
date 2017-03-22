using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void UIAction();
    public static event UIAction OnUIPlaced;

    public delegate void LoadAction();
    public static event LoadAction OnExcelDataLoaded;
    public static event LoadAction OnMoneyInstantiated;

    public delegate void GameAction();
    public static event GameAction OnMoneySpawned;
    public static event GameAction OnBoxOpened;
    public static event GameAction OnBoxThrow;
    public static event GameAction OnCategoryButtonPressed;
    public static event GameAction OnMoneyMovedToScreen;
    public static event GameAction OnMoneyMovedToShelf;

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
    public static void MoneySpawned()
    {
        if (OnMoneySpawned != null)
            OnMoneySpawned();
    }
    public static void BoxOpened()
    {
        if (OnBoxOpened != null)
            OnBoxOpened();
    }
    public static void BoxThrow()
    {
        if (OnBoxThrow != null)
            OnBoxThrow();
    }
    public static void CategoryButtonPressed()
    {
        if (OnCategoryButtonPressed != null)
            OnCategoryButtonPressed();
    }
    public static void MoneyMovedToScreen()
    {
        if (OnMoneyMovedToScreen != null)
            OnMoneyMovedToScreen();
    }
    public static void MoneyMovedToShelf()
    {
        if (OnMoneyMovedToShelf != null)
            OnMoneyMovedToShelf();
    }
}
