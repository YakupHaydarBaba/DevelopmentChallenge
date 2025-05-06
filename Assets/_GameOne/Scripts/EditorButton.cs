using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorButton))]
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridController script = (GridController)target;

        if (GUILayout.Button("CreateGrid"))
        {
            
        }
    }
}