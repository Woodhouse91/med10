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
    public static Sprite GetSprite(int id)
    {
        print(id);
        if (id == -1)
            return s[s.Length - 1];
        return s[id];
    }
}
