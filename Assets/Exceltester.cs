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
public class Exceltester : MonoBehaviour
{
    private IWorkbook workbook = null;
    private ISheet sheet = null;
    private void Start()
    {
        try
        {
            using (FileStream fileStream = new FileStream(Application.dataPath + "/Generic Categories.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new XSSFWorkbook(fileStream);
                sheet = workbook.GetSheet(GetSheetNames()[0]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        ICell cell = sheet.GetRow(0).GetCell(0);
        print(cell.NumericCellValue);
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
}
