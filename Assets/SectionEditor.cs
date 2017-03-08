using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SectionHandler))]
public class SectionEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SectionHandler myScript = (SectionHandler)target;
        if (GUILayout.Button("Add Section"))
        {
            myScript.AddSection();
        }
        if (GUILayout.Button("Reset List"))
        {
            myScript.ResetSectionList();
        }
    }
}

