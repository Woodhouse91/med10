using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryModelHandler : MonoBehaviour
{
    private static Transform[] s;
    // Use this for initialization
    void Start()
    {
        s = Resources.LoadAll<Transform>("BoxModels");
        print(s.Length);
        if (s.Length > 0)
        {
            for (int x = 0; x < s.Length - 1; ++x)
            {
                s[x] = Resources.Load<Transform>("BoxModels/" + x);
            }
            s[s.Length - 1] = Resources.Load<Transform>("BoxModels/unknown");
        }

    }
    public static Transform GetAt(int id)
    {
        if (id < 0 && id > s.Length - 1)
            return null;
        if (id == -1)
            return s[s.Length - 1];
        return s[id];
    }
}
