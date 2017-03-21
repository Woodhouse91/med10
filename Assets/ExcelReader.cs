using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;
using System.Linq;
using System.ComponentModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
public class ExcelReader : MonoBehaviour
{
    [SerializeField]
    private string expenseIdentifierString, incomeIdentifierString;
    private IWorkbook workbook = null;
    private ISheet sheet = null;
    IRow row;
    ICell cell;
    private string ExcelFileName;
    [SerializeField]
    private int monthRow, monthCell, sheetNumber, rowBuffer, categoryCell;
    DataHandler dat;
    private int categoryRowEnd, catCount, valueStart, categoryRowStart, valueDataEnd, incomeRow;
    CategoryDatabase CDb;
    int[] transferValue, incomeValue;
    private void Start()
    {
        CDb = FindObjectOfType<CategoryDatabase>();
        dat = FindObjectOfType<DataHandler>();
        try
        {
            using (FileStream fileStream = new FileStream(Application.dataPath + "/Generic Categories.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new XSSFWorkbook(fileStream);
                sheet = workbook.GetSheet(GetSheetNames()[0]);
                StartCoroutine(findGenerics());
                fileStream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private IEnumerator findGenerics()
    {
        int r = 0;
        int c = 0;
        List<string> s = new List<string>();
        while (true)
        {
            try {
                sheet.GetRow(r).GetCell(0);
            }
            catch {
                break;
            }
            try {
                s.Add(sheet.GetRow(r).GetCell(c).ToString());
                ++c;
            }
            catch
            {
                CDb.addGeneric(s);
                s.Clear();
                c = 0;
                ++r;
            }
            yield return null;
        }
        CDb.genericAddComplete();
        FindObjectOfType<ListBudgets>().GenerateBudgetList();
        yield break;
    }
    private void ExcelQuery()
    {
        try
        {
            using (FileStream fileStream = new FileStream(Application.dataPath+"/Excel budget/"+ExcelFileName+ ".xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new XSSFWorkbook(fileStream);
                sheet = workbook.GetSheet(GetSheetNames()[sheetNumber]);
                fileStream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        StartCoroutine(findIdentifiers());
    }
    private IEnumerator findCategoryCount()
    {
        int i = categoryRowStart;
        while (true)
        {
            try
            {
                string s = sheet.GetRow(i).GetCell(categoryCell).ToString();
            }
            catch { break; }
            dat.AddCategory(sheet.GetRow(i).GetCell(categoryCell).StringCellValue);
            i++;
            yield return null;
        }
        dat.setExpenseArray(i - categoryRowStart);
        catCount = i - categoryRowStart;
        valueStart = monthCell;
        StartCoroutine(findExpenseData());
        yield break;
    }
    private string[] GetSheetNames()
    {
        List<string> sheetList = new List<string>();
        int numSheets = workbook.NumberOfSheets;
        for (int i = 0; i < numSheets; i++)
        {
            sheetList.Add(workbook.GetSheetName(i));
        }
        return (sheetList.Count > 0) ? sheetList.ToArray() : null;
    }

    public void setExcelFile(string s)
    {
        ExcelFileName = s;
        ExcelQuery();
    }
    private IEnumerator findExpenseData()
    {
        int i = categoryRowStart;
        int k = valueStart;
        transferValue = new int[14];
        transferValue[0] = CDb.doGenericLookup(sheet.GetRow(i).GetCell(categoryCell).StringCellValue);
        bool breakOnNext = false;
        while (true)
        {
            try
            {
                transferValue[k - valueStart + 1] = (int) sheet.GetRow(i).GetCell(k).NumericCellValue;
            }
            catch
            {
                transferValue[k - valueStart + 1] = 0;
            }
            ++k;
            if(k-valueStart == transferValue.Length-1)
            {
                k = valueStart;
                dat.setExpenseData(transferValue);
                ++i;
                try
                {
                    transferValue[0] = CDb.doGenericLookup(sheet.GetRow(i).GetCell(categoryCell).StringCellValue);
                }
                catch
                {
                    transferValue[0] = -2;
                    if (breakOnNext)
                        break;
                    breakOnNext = true;
                }
            }
            yield return null;
        }
        StartCoroutine(findIncomeData());
        yield break;
    }
    private IEnumerator findIncomeData()
    {
        incomeValue = new int[13];
        int r = 0;
        int c = valueStart-1;
        bool count = true;
        while (true)
        {
            try
            {
                if (count)
                {
                    string s = (sheet.GetRow(incomeRow + r).GetCell(categoryCell).ToString());
                }
                else
                {
                    incomeValue[c - valueStart] = (int)sheet.GetRow(incomeRow + r).GetCell(c).NumericCellValue;
                }
            }
            catch
            {
                if (!count)
                {
                    incomeValue[c - valueStart] = 0;
                }
                count = false;
            }
            if(!count)
                ++c;
            if (count)
                ++r;
            if (c - valueStart == incomeValue.Length)
                break;
            yield return null;
        }
        dat.setStartMonth(sheet.GetRow(monthRow).GetCell(monthCell).StringCellValue);
        dat.setIncomeData(incomeValue);
        DataHandler.dataCompleted = true;
        yield break;
    }
    private IEnumerator findIdentifiers()
    {
        int r = 0;
        bool foundExpense = false;
        bool foundIncome = false;
        while (!(foundIncome && foundExpense))
        {
            try
            {
                if (sheet.GetRow(r).GetCell(categoryCell).StringCellValue == incomeIdentifierString)
                {
                    incomeRow = r;
                    foundIncome = true;
                }
                else if (sheet.GetRow(r).GetCell(categoryCell).StringCellValue == expenseIdentifierString)
                {
                    categoryRowStart = r + rowBuffer;
                    foundExpense = true;
                }
            }
            catch
            {
            }
            ++r;
            yield return null;
        }
        StartCoroutine(findCategoryCount());
        yield break;
    }
}