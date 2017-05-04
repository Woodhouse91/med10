using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Text;
using System.IO;

public class DataAppLauncher : MonoBehaviour
{
    private const string dataFileName = "/data.txt";
    private const string completedDataFileName = "/completedData.txt";
    private const string dataBreak = "DATABREAK";
    private const string rowBreak = "ROWBREAK";
    private static string shortcutPath;
    private static string spaceCheck;
    private static bool doLaunch;
    private static ProcessStartInfo startInfo;
    private List<string> GenericsList;
    private List<string> BudgetCat;
    private List<int> Income;
    private List<List<int>> Expense;
    private List<int> ExpenseList;
    public static void LaunchApplication(string budgetFile)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Environment.GetFolderPath(Environment.SpecialFolder.Programs));
        sb.Append("\\");
        sb.Append("Excel2Text");
        sb.Append("\\");
        sb.Append("Excel2Text.appref-ms ");
        shortcutPath = sb.ToString();
        startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = true;
        startInfo.WindowStyle = ProcessWindowStyle.Minimized;
        spaceCheck = Application.streamingAssetsPath+","+budgetFile;
        spaceCheck = spaceCheck.Replace(" ", "<MakeASpace>");
        startInfo.Arguments = spaceCheck;
        startInfo.FileName = shortcutPath;
        if (File.Exists(Application.streamingAssetsPath + completedDataFileName))
            File.Delete(Application.streamingAssetsPath + completedDataFileName);
        if (File.Exists(Application.streamingAssetsPath + dataFileName))
            File.Delete(Application.streamingAssetsPath + dataFileName);
        doLaunch = true;
        
    }
    private void Start()
    {
        StartCoroutine(WaitForProcess());
    }

    private IEnumerator WaitForProcess()
    {
        while (!doLaunch)
        {
            yield return null;
        }
        if (!File.Exists(shortcutPath))
        {
            Process.Start(Application.streamingAssetsPath + "/setup.exe");
        }
        while (!File.Exists(shortcutPath))
        {
            yield return null;
        }
        Process.Start(startInfo);
        while (!File.Exists(Application.streamingAssetsPath + completedDataFileName))
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(populateArrays());
        yield break;
    }
    private IEnumerator populateArrays()
    {
        int val;
        GenericsList = new List<string>();
        BudgetCat = new List<string>();
        Income = new List<int>();
        Expense = new List<List<int>>();
        ExpenseList = new List<int>();
        FileStream stream = new FileStream(Application.streamingAssetsPath+completedDataFileName,FileMode.Open);
        StreamReader sr = new StreamReader(stream);
        string res = sr.ReadLine();
        #region Generics
        while (res != dataBreak)
        {
            if (res == rowBreak)
            {
                CategoryDatabase.addGeneric(GenericsList);
                GenericsList.Clear();
            }
            else
            {
                GenericsList.Add(res);
            }
            res = sr.ReadLine();
            yield return null;
        }
        CategoryDatabase.genericAddComplete();
        #endregion
        #region Month
        res = sr.ReadLine();
        while (res != dataBreak)
        {
            try
            {
                DataHandler.startMonth = int.Parse(res);
            }
            catch { }
            res = sr.ReadLine();
            yield return null;
        }
        #endregion
        #region Income
        res = sr.ReadLine();
        while (res != dataBreak)
        {
            try
            {
                Income.Add(int.Parse(res));
            }
            catch
            {
                Income.Add(0);
            }
            res = sr.ReadLine();
            yield return null;
        }
        #endregion
        #region Expense
        res = sr.ReadLine();
        bool isCategory = true;
        
        while (true)
        {
            while (res != rowBreak && res!=dataBreak)
            {
                if (isCategory)
                {
                    ExpenseList.Add(CategoryDatabase.doGenericLookup(res));
                    BudgetCat.Add(res);
                    isCategory = false;
                }
                else
                {
                    if (int.TryParse(res, out val))
                        ExpenseList.Add(val);
                    else
                        ExpenseList.Add(0);

                }
                res = sr.ReadLine();
                yield return null;
            }
            isCategory = true;
            Expense.Add(ExpenseList);
            ExpenseList = new List<int>();
            if (res == dataBreak)
                break;
            res = sr.ReadLine();
            yield return null;
        }
        #endregion
        #region PRINT DATA
        //print("Month: " + DataHandler.startMonth);
        //print("EXPENSE DATA: ");
        //print(Expense.Count);
        //for (int x = 0; x < Expense.Count; ++x)
        //{
        //    print(BudgetCat[x]);
        //    for (int k = 0; k < Expense[x].Count; ++k)
        //    {
        //        print(Expense[x][k]);
        //    }
        //}
        //print("INCOME DATA: ");
        //for (int x = 0; x < Income.Count; ++x)
        //{
        //    print(Income[x]);
        //}
        #endregion
        int[,] dhAr = new int[14, Expense.Count];
        for(int x = 0; x<Expense.Count; ++x)
        {
            for(int y = 0; y<14; ++y)
            {
                dhAr[y, x] = Expense[x][y];
            }
        }
        DataHandler.expenseData = dhAr;
        DataHandler.incomeData = Income.ToArray();
        DataHandler.BudgetCategories = BudgetCat;
        DataHandler.dataCompleted = true;
        sr.Close();
        //if (File.Exists(Application.streamingAssetsPath + completedDataFileName))
        //    File.Delete(Application.streamingAssetsPath + completedDataFileName);
       
        yield break;
    }
}
