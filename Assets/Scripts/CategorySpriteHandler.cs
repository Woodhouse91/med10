using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySpriteHandler : MonoBehaviour {
    private static Sprite[] s;
	// Use this for initialization
	void Start () {
        s = Resources.LoadAll<Sprite>("BoxIcons");
        for(int x = 0; x<s.Length-1; ++x)
        {
            s[x] = Resources.Load<Sprite>("BoxIcons/" + x);
        }
        s[s.Length - 1] = Resources.Load<Sprite>("BoxIcons/unknown");
        
	}
    public static Sprite GetAt(int id)
    {
        if (id < 0 && id > s.Length - 1)
            return null;
        if (id == -1)
            return s[s.Length - 1];
        return s[id];
    }
}
