using UnityEngine;
using System.Collections;

public class Player2D : MonoBehaviour {

	public float speed = 1;

	void Update() {
		transform.position = new Vector3(
			transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime,
			transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime,
			transform.position.z);
	}
}
