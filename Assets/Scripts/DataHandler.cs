using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour {
    [SerializeField]
    public static int[,] expenseData
    {
        get
        {
            return _expenseData;
        }
    }
    private static int[,] _expenseData;
    public static int[] incomeData
    {
        get
        {
            return _incomeData;
        }
        set
        {
            _incomeData = value;
        }
    }
    public static bool dataCompleted
    {
        get
        {
            return _dataCompleted;
        }
        set
        {
            calcRequiredBills();
            if(_ExposeData)
                ExposeData(); //Print data to console
            EventManager.ExcelDataLoaded();
            _dataCompleted = value;
        }
    }
    public struct billRef
    {
        public int _1000, _500, _200, _100, _50, _20, _10, _5, _2, _1;
    }
    public static List<billRef[]> BillsAtCategory_Month;
    private static bool _dataCompleted = false, _ExposeData = false;
    private static int[] _incomeData;
    private enum Months { January, February, March, April, May, June, July, August, September, October, November, December};
    private static Months month;
    private enum Categories {Rent, Ensurance, Savings};
    private Categories category;
    public struct TotalBills
    {
        public int _1000, _500, _200, _100, _50, _20, _10, _5, _2, _1;
    }
    public static TotalBills tBills;
    public static int startMonth
    {
        get
        {
            return budgetStartMonth;
        }
        
    }
    private static int budgetStartMonth;
    public static List<string> BudgetCategories
    {
        get
        {
            return _budgetCategories;
        }
    }
    private static List<string> _budgetCategories;
    private int catCounter = 0, totalCategories;
    public static int tExpense, tIncome;
  
    private static void ExposeData()
    {
        print("Expense data:");
        for (int x = 0; x < expenseData.GetLength(1); ++x)
            for (int k = 0; k < expenseData.GetLength(0); ++k)
            {
                print("     " + expenseData[k, x]);
            }
        print("Income data:");
        for (int p = 0; p < incomeData.Length; ++p)
            print("     " + incomeData[p]);
        print("Month: "+month);
        print("Categories:");
        for(int l = 0; l<_budgetCategories.Count; ++l)
        {
            print("     " + _budgetCategories[l]);
        }
    }
    private void Start()
    {
        _budgetCategories = new List<string>();
    }
    public void AddCategory(string cat)
    {
        _budgetCategories.Add(cat);
    }
    public void setExpenseArray(int categoryCount)
    {
        totalCategories = categoryCount;
        _expenseData = new int[14, categoryCount];
        catCounter = 0;
    }
    public void setStartMonth(string month)
    {
        if (month.Contains("Jan"))
            DataHandler.month = Months.January;
        else if (month.Contains("Feb"))
            DataHandler.month = Months.February;
        else if (month.Contains("Mar"))
            DataHandler.month = Months.March;
        else if (month.Contains("Apr"))
            DataHandler.month = Months.April;
        else if (month.Contains("Maj"))
            DataHandler.month = Months.May;
        else if (month.Contains("Jun"))
            DataHandler.month = Months.June;
        else if (month.Contains("Jul"))
            DataHandler.month = Months.July;
        else if (month.Contains("Aug"))
            DataHandler.month = Months.August;
        else if (month.Contains("Sep"))
            DataHandler.month = Months.September;
        else if (month.Contains("Okt"))
            DataHandler.month = Months.October;
        else if (month.Contains("Nov"))
            DataHandler.month = Months.November;
        else if (month.Contains("Dec"))
            DataHandler.month = Months.December;
        budgetStartMonth = month.GetHashCode();
    }
    public void setIncomeData(int[] val)
    {
        incomeData = new int[13];
        incomeData = val;
    }
    public void setExpenseData(int[] val)
    {
        bool skipCategory = true;
        _expenseData[0, catCounter] = val[0];
       // int sum = 0;
        for(int x = 1; x<val.Length; ++x)
        {
            if (val[x] != 0)
                skipCategory = false;
            _expenseData[x, catCounter] = val[x];
         //   sum += val[x];
        }
        //_expenseData[13, catCounter] = sum;
        if (!skipCategory)
            ++catCounter;
        else if(catCounter<_budgetCategories.Count)
            _budgetCategories.Remove(_budgetCategories[catCounter]);
    }
    public static float getScale(int category, int month)
    {
        return _expenseData[month, category] / tExpense;
    }
    private static void calcRequiredBills()
    {
        BillsAtCategory_Month = new List<billRef[]>();
        tBills = new TotalBills();
        tExpense = 0;
        tIncome = _incomeData[12];
        for(int r = 0; r<expenseData.GetLength(1)-1; ++r)
        {
            billRef[] res = new billRef[12];
            for(int c = 1; c<expenseData.GetLength(0)-1; ++c)
            {
                int val = expenseData[c, r];
                tExpense += val;
                res[c - 1]._1000 = val / 1000;
                val %= 1000;
                res[c - 1]._500 = val / 500;
                val %= 500;
                res[c - 1]._200 = val / 200;
                val %= 200;
                res[c - 1]._100 = val / 100;
                val %= 100;
                res[c - 1]._50 = val / 50;
                val %= 50;
                res[c - 1]._20 = val / 20;
                val %= 20;
                res[c - 1]._10 = val / 10;
                val %= 10;
                res[c - 1]._5 = val / 5;
                val %= 5;
                res[c - 1]._2 = val / 2;
                val %= 2;
                res[c - 1]._1 = val;
                tBills._1000 += res[c - 1]._1000;
                tBills._500 += res[c - 1]._500;
                tBills._200 += res[c - 1]._200;
                tBills._100 += res[c - 1]._100;
                tBills._50 += res[c - 1]._50;
                tBills._20 += res[c - 1]._20;
                tBills._10 += res[c - 1]._10;
                tBills._5 += res[c - 1]._5;
                tBills._2 += res[c - 1]._2;
                tBills._1 += res[c - 1]._1;
            }
            BillsAtCategory_Month.Add(res);
        }
        int dIncome = tIncome - tExpense;
        tBills._1000 += dIncome / 1000;
        dIncome %= 1000;
        tBills._500 += dIncome / 500;
        dIncome %= 500;
        tBills._200 += dIncome / 200;
        dIncome %= 200;
        tBills._100 += dIncome / 100;
        dIncome %= 100;
        tBills._50 += dIncome / 50;
        dIncome %= 50;
        tBills._20 += dIncome / 20;
        dIncome %= 20;
        tBills._10 += dIncome / 10;
        dIncome %= 10;
        tBills._5 += dIncome / 5;
        dIncome %= 5;
        tBills._2 += dIncome / 2;
        dIncome %= 2;
        tBills._1 += dIncome / 1;
    }

}
