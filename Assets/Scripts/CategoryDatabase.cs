using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CategoryDatabase : MonoBehaviour {
    private static Dictionary<string, int> lookupDB;
    private static List<string[]> genericLookup;

    public static int doGenericLookup(string s)
    {
        int ret = -1;
        s = s.ToLower();
        int res;
        if (lookupDB.TryGetValue(s, out res))
            ret = res;
        else
        {
            for(int x = 0; x<genericLookup.Count; ++x)
            {
                for(int k = 0; k<genericLookup[x].Length; ++k)
                {
                    if (s.Contains(genericLookup[x][k]))
                    {
                        ret = x;
                        break;
                    }
                }
            }
        }
        return ret;
    }
    
    public static string GetName(int i)
    {
        if (i == -1)
            return "UKENDT";
        return genericLookup[i][0].ToUpper();
    }   

    public static void addGeneric(List<string> s)
    {
        if (genericLookup == null)
            genericLookup = new List<string[]>();
        genericLookup.Add(s.ToArray());

    }
    public static void genericAddComplete()
    {
        if(lookupDB == null)
            lookupDB = new Dictionary<string, int>();
        for(int x = 0; x<genericLookup.Count; ++x)
        {
            for(int k = 0; k<genericLookup[x].Length; ++k)
            {
                lookupDB.Add(genericLookup[x][k], x);
            }
        }
    }
}
