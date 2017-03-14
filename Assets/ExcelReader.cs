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
    private IWorkbook workbook = null;
    private ISheet sheet = null;
    IRow row;
    ICell cell;
    private string ExcelFileName;
    [SerializeField]
    private short sheetNumber;
    public void ExcelQuery()
    {
        try
        {
            using (FileStream fileStream = new FileStream(Application.dataPath+"/Excel budget/"+ExcelFileName+ ".xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new XSSFWorkbook(fileStream);
                sheet = workbook.GetSheet(GetSheetNames()[sheetNumber]);
            }
        }

        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }



    public string[] GetSheetNames()
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
}