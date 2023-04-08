using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenerateMapScript))]
public class GenerateNodesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateMapScript myScript = (GenerateMapScript)target;
        if(GUILayout.Button("Generate Nodes"))
        {
            myScript.PlaceNodes();
        }
    }
}

