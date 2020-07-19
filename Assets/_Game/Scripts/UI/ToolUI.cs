using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour {
//
//	public Button[] Tools;
//	void Start(){
//		Tools = transform.GetComponentsInChildren<Button>();
//	}
//	// Use this for initialization
//	void OnEnable () {
//		ToolManager.OnToolChanged += SetTool;
//	}
//	
//	// Update is called once per frame
//	void OnDisable () {
//		ToolManager.OnToolChanged -= SetTool;
//
//	}
//
//	void SetTool(){
//		for (int i = 0; i < Tools.Length; i++) {
//			if (i == ToolManager.Instance.CurrentToolIndex) {
//				Tools [i].gameObject.transform.Find ("ActiveFrame").gameObject.SetActive (true);
//			} else {
//				Tools [i].gameObject.transform.Find ("ActiveFrame").gameObject.SetActive (false);
//
//			}
//		}
//	}

//	public void SetActiveTool(int pID){
//		ToolManager.Instance.CurrentToolIndex = pID;
//		ToolManager.Instance.ChangeCurrentTool ();
//	}
}
