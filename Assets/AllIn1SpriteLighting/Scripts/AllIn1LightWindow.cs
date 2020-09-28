#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class AllIn1LightWindow : EditorWindow
{
    [MenuItem("Window/AllIn1LightWindow")]
    public static void ShowAllIn1LightWindowWindow()
    {
        GetWindow<AllIn1LightWindow>("All In 1 Lighting Window");
    }

    private DefaultAsset materialTargetFolder = null;
    private DefaultAsset normalsTargetFolder = null;

    private void OnGUI()
    {
        GUILayout.Label("Through this window you'll be able to set the folders where the asset will save it's Materials and Normal Maps",
            EditorStyles.boldLabel);

        GUILayout.Space(20);

        materialTargetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
             "New Material Folder",
             materialTargetFolder,
             typeof(DefaultAsset),
             false);

        if (materialTargetFolder != null && IsAssetAFolder(materialTargetFolder))
        {
            string path = AssetDatabase.GetAssetPath(materialTargetFolder);
            PlayerPrefs.SetString("All1LightMaterials", path);
            EditorGUILayout.HelpBox(
                "Valid folder! Material save path: " + path,
                MessageType.Info,
                true);
        }
        else
        {
            EditorGUILayout.HelpBox(
                "Select the new Material Folder",
                MessageType.Warning,
                true);
        }

        GUILayout.Space(20);

        normalsTargetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
             "New Normal Map Folder",
             normalsTargetFolder,
             typeof(DefaultAsset),
             false);

        if (normalsTargetFolder != null)
        {
            string path = AssetDatabase.GetAssetPath(normalsTargetFolder);
            PlayerPrefs.SetString("All1LightNormals", path);
            EditorGUILayout.HelpBox(
                "Valid folder! Normal Map save path: " + path,
                MessageType.Info,
                true);
        }
        else
        {
            EditorGUILayout.HelpBox(
                "Select the new Normal Map Folder",
                MessageType.Warning,
                true);
        }
    }

    private static bool IsAssetAFolder(Object obj)
    {
        string path = "";

        if (obj == null) return false;

        path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

        if (path.Length > 0)
        {
            if (Directory.Exists(path)) return true;
            else return false;
        }
        return false;
    }
}
#endif