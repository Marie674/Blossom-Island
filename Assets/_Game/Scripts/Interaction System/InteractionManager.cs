using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	bool InRange=false;
	public List<GameObject> ObjectsInRange = new List<GameObject> ();

	private static InteractionManager s_Instance=null;

//	public static InteractionManager Instance{
//		get{
//			if (s_Instance == null) {
//				s_Instance = FindObjectOfType (typeof(InteractionManager)) as InteractionManager;
//			}
//			if (s_Instance == null) {
//				GameObject obj = new GameObject ("InteractionManager");
//				s_Instance = obj.AddComponent (typeof(InteractionManager)) as InteractionManager;
//				Debug.Log ("Could not locate InteractionManager object. Created one.");
//			}
//			return s_Instance;
//		}
//	}

	public bool Interact(){

		if (InRange == true) {
			if (ObjectsInRange [ObjectsInRange.Count - 1] == null) {
				ObjectsInRange.Remove (ObjectsInRange [ObjectsInRange.Count - 1]);
				if (ObjectsInRange.Count > 0) {
					Interact ();
				} else {
					InRange = false;
				}
			}
			//ObjectsInRange [ObjectsInRange.Count - 1].SendMessage ("Interact",SendMessageOptions.DontRequireReceiver);
			return true;
		}
		return false;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.isTrigger == true && !ObjectsInRange.Contains(other.gameObject)) {
			ObjectsInRange.Add (other.gameObject);
			print ("added " + other.gameObject.name);
		}
		if (ObjectsInRange.Count <= 0) {
			InRange = false;
		} else {
			InRange = true;
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.isTrigger == true && ObjectsInRange.Contains(other.gameObject)) {
			ObjectsInRange.Remove(other.gameObject);
			print ("removed " + other.gameObject.name);

		}
		if (ObjectsInRange.Count <= 0) {
			InRange = false;
		} else {
			InRange = true;
		}
	}
}
