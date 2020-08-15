using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(OccupySpace))]
[CanEditMultipleObjects]
public class OccupySpaceEditor : Editor
{


    public override void OnInspectorGUI()
    {

        base.DrawDefaultInspector();
        var targetSprite = (OccupySpace)target;

        Color OccupiedColor = Color.red;
        Color WallColor = Color.blue;
        Color CounterColor = new Color(1, 0.8f, 0.8f);
        Color TreeColor = Color.green;


        EditorGUILayout.LabelField("Item Collisions");
        if (GUILayout.Button("Initialize Grid"))
        {
            targetSprite.InitGrid();
        }

        if (targetSprite.Init)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < targetSprite.Height; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < targetSprite.Width; j++)
                {
                    if (targetSprite.TileCollisions[i].Collisions[j] != -1)
                    {
                        int val = targetSprite.TileCollisions[i].Collisions[j];
                        if (val == 0)
                        {
                            GUI.color = OccupiedColor;

                        }
                        else if (val == 1)
                        {
                            GUI.color = WallColor;

                        }
                        else if (val == 2)
                        {
                            GUI.color = CounterColor;

                        }
                        else if (val == 3)
                        {
                            GUI.color = TreeColor;

                        }
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }
                    if (GUILayout.Button("", GUILayout.Width(32), GUILayout.Height(32)))
                    {
                        //targetSprite.TileCollisions [i].Collisions [j] = !targetSprite.TileCollisions [i].Collisions [j];
                        //serializedObject.FindProperty("TileCollisions[i].collisions[j]").boolValue = !serializedObject.FindProperty("TileCollisions[i].collisions[j]").boolValue;
                        // bool occupied = serializedObject.FindProperty("TileCollisions").GetArrayElementAtIndex(i).FindPropertyRelative("Collisions").GetArrayElementAtIndex(j).boolValue;
                        int val = serializedObject.FindProperty("TileCollisions").GetArrayElementAtIndex(i).FindPropertyRelative("Collisions").GetArrayElementAtIndex(j).intValue;
                        val = val + 1;
                        if (val == 4)
                        {
                            val = -1;
                        }
                        serializedObject.FindProperty("TileCollisions").GetArrayElementAtIndex(i).FindPropertyRelative("Collisions").GetArrayElementAtIndex(j).intValue = val;
                        serializedObject.ApplyModifiedProperties();
                    }

                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

    }
}
