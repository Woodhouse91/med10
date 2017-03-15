using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CategoryDatabase : MonoBehaviour {
    Dictionary<string, int> lookupDB;
    List<string[]> genericLookup;

	// Use this for initialization
	void Start () {
    }
	

    public int doGenericLookup(string s)
    {
        int ret = -1;
        s = s.ToLower();
        //string ss = string.Empty;
        //for(int x = 0; x<s.Length-4; ++x)
        //{
        //    if (!(s[x] == 'a' && s[x + 1] == 'f' && s[x + 2] == 't' && s[x + 3] == '.'))
        //        ss.Insert(x, s[x].ToString());
        //}
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
    public void addGeneric(List<string> s)
    {
        if (genericLookup == null)
            genericLookup = new List<string[]>();
        genericLookup.Add(s.ToArray());

    }
    public void genericAddComplete()
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
