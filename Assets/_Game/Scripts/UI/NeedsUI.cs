using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsUI : MonoBehaviour {

	NeedUI[] Needs;

	public void Init(){
		Needs = GetComponentsInChildren<NeedUI> ();
		foreach (NeedUI need in Needs) {
			need.Init ();
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
