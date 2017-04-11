using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormatHandler : MonoBehaviour {
    [SerializeField]
    private static int maxStringLength = 10;


    public static string FormatCurrency(int val)
    {
        decimal moneyvalue = val;
        string res = String.Format("{0:N}", moneyvalue);
        res = res.Remove(res.Length - 3);
        res = res.Replace(",", ".");
        res += ",-";
        return res;
    }
    public static string FormatCategory(string s)
    {
        if (s.Contains("aft."))
        {
            int id = 0;
            s = s.Replace("aft.", "<");
            for (int x = 0; x < s.Length; ++x)
            {
                if (s[x] == '<')
                {
                    id = x - 3;
                    break;
                }
            }
            s = s.Substring(0, id);
        }
        s.Trim('\n');
        return s;
    }
}
