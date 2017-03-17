using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour {
    [SerializeField]
    private bool _ExposeData = false;
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
    public bool dataCompleted
    {
        get
        {
            return _dataCompleted;
        }
        set
        {
            if(_ExposeData)
                ExposeData(); //Print data to console
            calcRequiredBills();
            _dataCompleted = value;
        }
    }
    public struct billRef
    {
        public int _1000, _500, _200, _100, _50, _20, _10, _5, _2, _1;
    }
    public static List<billRef[]> BillsAtCategory_Month;
    private bool _dataCompleted = false;
    private static int[] _incomeData;
    private enum Months { January, February, March, April, May, June, July, August, September, October, November, December};
    private Months month;
    private enum Categories {Rent, Ensurance, Savings};
    private Categories category;
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
    private AddMoneyToTable am;
    private void passData()
    {
        am = FindObjectOfType<AddMoneyToTable>();
        am.spawnAllTheMoney(expenseData);
    }
    private void ExposeData()
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
            this.month = Months.January;
        else if (month.Contains("Feb"))
            this.month = Months.February;
        else if (month.Contains("Mar"))
            this.month = Months.March;
        else if (month.Contains("Apr"))
            this.month = Months.April;
        else if (month.Contains("Maj"))
            this.month = Months.May;
        else if (month.Contains("Jun"))
            this.month = Months.June;
        else if (month.Contains("Jul"))
            this.month = Months.July;
        else if (month.Contains("Aug"))
            this.month = Months.August;
        else if (month.Contains("Sep"))
            this.month = Months.September;
        else if (month.Contains("Okt"))
            this.month = Months.October;
        else if (month.Contains("Nov"))
            this.month = Months.November;
        else if (month.Contains("Dec"))
            this.month = Months.December;
        budgetStartMonth = month.GetHashCode();
    }
    public void setIncomeData(int[] val)
    {
        incomeData = new int[13];
        incomeData = val;
        //for(int x = 0; x<incomeData.Length-1; ++x)
        //{
        //    incomeData[12] += incomeData[x];
        //}
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
    private void calcRequiredBills()
    {
        BillsAtCategory_Month = new List<billRef[]>();
        for(int r = 0; r<expenseData.GetLength(1)-1; ++r)
        {
            billRef[] res = new billRef[12];
            for(int c = 1; c<expenseData.GetLength(0)-1; ++c)
            {
                int val = expenseData[c, r];
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
            }
            BillsAtCategory_Month.Add(res);
        }
    }

}
