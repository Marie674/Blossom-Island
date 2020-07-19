using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(OccupySpace))]
[CanEditMultipleObjects]
public class OccupySpaceEditor : Editor {


	public override void OnInspectorGUI(){
		
		base.DrawDefaultInspector ();
		var targetSprite = (OccupySpace)target;


		EditorGUILayout.LabelField ("Item Collisions");
		if(GUILayout.Button("Initialize Grid"))
		{
			targetSprite.InitGrid();
		}

		if (targetSprite.Init) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();
			for (int i = 0; i < targetSprite.Height; i++) {
				EditorGUILayout.BeginHorizontal ();
				for (int j = 0; j < targetSprite.Width; j++) {
					if (targetSprite.TileCollisions[i].Collisions[j] == true) {
						GUI.color = Color.red;
					}
					if (GUILayout.Button ("", GUILayout.Width (32), GUILayout.Height (32))) {
						//targetSprite.TileCollisions [i].Collisions [j] = !targetSprite.TileCollisions [i].Collisions [j];
						//serializedObject.FindProperty("TileCollisions[i].collisions[j]").boolValue = !serializedObject.FindProperty("TileCollisions[i].collisions[j]").boolValue;
						bool occupied = serializedObject.FindProperty("TileCollisions").GetArrayElementAtIndex(i).FindPropertyRelative("Collisions").GetArrayElementAtIndex(j).boolValue;
						serializedObject.FindProperty("TileCollisions").GetArrayElementAtIndex(i).FindPropertyRelative("Collisions").GetArrayElementAtIndex(j).boolValue = !occupied;
						serializedObject.ApplyModifiedProperties();
					}
					GUI.color = Color.white;
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndHorizontal ();
		}

	}
}
