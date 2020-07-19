using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInteractionReceiver : MonoBehaviour {

	public List<string> Tags = new List<string>();

	void OnTriggerEnter2D(Collider2D other){
		
		if (Tags.Contains (other.gameObject.tag)) {
			Destroy (this.gameObject);
		}

	}
}
