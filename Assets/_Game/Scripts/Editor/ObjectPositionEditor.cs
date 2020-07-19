using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPosition))]
[CanEditMultipleObjects]

public class ObjectPositionEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		ObjectPosition myScript = (ObjectPosition)target;
		#region Green
		GUI.color = Color.green;
		if(GUILayout.Button("Snap Positions"))
		{
			myScript.AdjustPositions ();
		}
		#endregion
	}
}