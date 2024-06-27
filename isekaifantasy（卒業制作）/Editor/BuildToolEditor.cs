using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildTool))]
[CanEditMultipleObjects]
public class BuildToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BuildTool myScript = (BuildTool)target;
        if (GUILayout.Button("随机放置"))
            myScript.RandomSet();
        if (GUILayout.Button("缩小"))
            myScript.HalfSet();
    }
}