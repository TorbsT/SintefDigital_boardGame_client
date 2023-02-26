using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using View;

[CustomEditor(typeof(GraphCreator))]
public class GraphCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GraphCreator graphCreator = (GraphCreator)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Serialize graph"))
            graphCreator.SerializeGraph();
        if (GUILayout.Button("Add Node"))
            graphCreator.AddNode();
    }
}