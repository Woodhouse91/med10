using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySpriteHandler : MonoBehaviour {
    private static Sprite[] s;
	// Use this for initialization
	void Start () {
        s = Resources.LoadAll<Sprite>("BoxIcons/");
	}
    public static Sprite GetSprite(int id)
    {
        return s[id];
    }
}
