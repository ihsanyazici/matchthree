using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCreator))]
public class GridCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridCreator gc = (GridCreator)target;

        if (GUILayout.Button("Create Grids"))
        {
            gc.InitGridCreator();
            EditorUtility.SetDirty(gc);
        }
        if (GUILayout.Button("Delete Grids"))
        {
            gc.EraseGridCreator();
            EditorUtility.SetDirty(gc);
        }
        if (GUILayout.Button("Save Changes"))
        {
            EditorUtility.SetDirty(gc);
        }
    }
}
