using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnload : MonoBehaviour {
    private static bool enableMesh = false;

    // Use this for initialization
    void Start () {
        GetComponent<MeshRenderer>().enabled = false;
        enableMesh = false;
        StartCoroutine(waitForMesh());
	}
    public static void EnableMesh() {
        enableMesh = true;
    }
    private IEnumerator waitForMesh()
    {
        while (!enableMesh)
            yield return null;
        GetComponent<MeshRenderer>().enabled = true;
    }
}
