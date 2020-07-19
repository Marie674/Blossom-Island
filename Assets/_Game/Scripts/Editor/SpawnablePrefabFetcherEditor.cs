using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnablePrefabFetcher))]
[CanEditMultipleObjects]
public class SpawnablePrefabFetcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnablePrefabFetcher myScript = (SpawnablePrefabFetcher)target;
        #region Green
        GUI.color = Color.green;
        if (GUILayout.Button("Generate"))
        {
            myScript.Fetch();
        }
        #endregion
    }
}



