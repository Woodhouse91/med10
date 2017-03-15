using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour {
    public int[,] expenseData
    {
        get
        {
            return _expenseData;
        }
    }
    private int[,] _expenseData;
    public int[] incomeData
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
            ExposeData();
            _dataCompleted = value;
        }
    }
    private bool _dataCompleted = false;
    private int[] _incomeData;
    private enum Months { January, February, March, April, May, June, July, August, September, October, November, December};
    private Months month;
    private enum Categories {Rent, Ensurance, Savings};
    private Categories category;
    private int budgetStartMonth;
    List<string> BudgetCategories;
    private int catCounter = 0;
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
        for(int l = 0; l<BudgetCategories.Count; ++l)
        {
            print("     " + BudgetCategories[l]);
        }
    }
    private void Start()
    {
        BudgetCategories = new List<string>();
    }
    public void AddCategory(string cat)
    {
        BudgetCategories.Add(cat);
    }
    public void setExpenseArray(int categoryCount)
    {
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
        for(int x = 0; x<incomeData.Length-1; ++x)
        {
            incomeData[12] += incomeData[x];
        }
    }
    public void setExpenseData(int[] val)
    {
        bool skipCategory = true;
        _expenseData[0, catCounter] = val[0];
        int sum = 0;
        for(int x = 1; x<val.Length-1; ++x)
        {
            if (val[x] != 0)
                skipCategory = false;
            _expenseData[x, catCounter] = val[x];
            sum += val[x];
        }
        _expenseData[13, catCounter] = sum;
        if (!skipCategory)
            ++catCounter;
        else if(catCounter<BudgetCategories.Count)
            BudgetCategories.Remove(BudgetCategories[catCounter]);
    }
}
