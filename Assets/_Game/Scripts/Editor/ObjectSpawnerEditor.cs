using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectSpawner))]
[CanEditMultipleObjects]
public class ObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectSpawner myScript = (ObjectSpawner)target;
        #region Green
        GUI.color = Color.green;
        if (GUILayout.Button("Generate"))
        {
            myScript.EditorGenerate();
        }
        #endregion
    }
}
