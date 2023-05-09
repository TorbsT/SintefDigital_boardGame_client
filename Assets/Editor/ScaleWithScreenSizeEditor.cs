#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Common;

namespace Editors
{
    [CustomEditor(typeof(ScaleWithScreenSize))]
    public class ScaleWithScreenSizeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var graphCreator = (ScaleWithScreenSize)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Reset original dimensions"))
                graphCreator.ResetOriginalDimensions();
        }
    }
}
#endif
