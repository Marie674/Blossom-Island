using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
public class RecipeCreatorWindow : OdinEditorWindow
{

    [MenuItem("Assets/Recipe Creator")]
    private static void OpenWindow()
    {
        GetWindow<RecipeCreatorWindow>().Show();
    }
}
