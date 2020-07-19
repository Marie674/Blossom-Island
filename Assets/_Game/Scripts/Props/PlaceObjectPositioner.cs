// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using CreativeSpore.RpgMapEditor;

// public class PlaceObjectPositioner : MonoBehaviour {

// 	public PlaceObject TargetObject;
// 	private Vector3 OPos;
// 	private Vector3 OScale;
// 	private Quaternion ORot;

// 	PlayerCharacter PlayerCharacter;

// 	private Bounds SpriteBounds;
// 	private Vector3 SpriteExtents;

// 	private AnchorPosition OldPosition = AnchorPosition.None;

// 	private Vector3 OldAnchorPos = Vector3.zero;
// 	private enum AnchorPosition
// 	{
// 		Up,
// 		Down,
// 		Left,
// 		Right,
// 		None,
// 	}

// 	private AnchorPosition CurrentPosition = AnchorPosition.Down;

// 	public Transform Anchor;

// 	// Use this for initialization
// 	void Start () {
// 		if (TargetObject != null) {
// 			OPos = TargetObject.transform.localPosition;
// 			OScale = TargetObject.transform.localScale;
// 			ORot = TargetObject.transform.localRotation;
// 			PlayerCharacter = GameObject.FindWithTag ("Player").GetComponent<PlayerCharacter> ();
// 		}
// 		UpdatePos ();
// 	}

// 	void OnEnable(){
// 		PlayerCharacter.OnPlayerDirectionChange += UpdatePos;
// 		TargetObject.OnObjectChanged += SetObject;

// 	}

// 	void OnDisable(){
// 		PlayerCharacter.OnPlayerDirectionChange -= UpdatePos;
// 		TargetObject.OnObjectChanged -= SetObject;

// 	}

// 	void SetObject(){
// 		TargetObject.transform.rotation = ORot;
// 		TargetObject.transform.position = OPos;
// 		TargetObject.transform.localScale = OScale;
// 		SpriteBounds = TargetObject.GetComponent<SpriteRenderer> ().bounds;
// 		SpriteExtents = SpriteBounds.extents;
// 		OldPosition = AnchorPosition.None;
// 		UpdatePos ();
// 	}

// 	void Update(){
// 		if (TargetObject == null) {
// 			return;
// 		}
// 		if (TargetObject.IsPlaceableObject == false) {
// 			return;
// 		}

// 		if(Input.GetButtonDown("Anchor Up")){
// 			CurrentPosition = AnchorPosition.Up;
// 			UpdatePos ();
// 		}
// 		if(Input.GetButtonDown("Anchor Down")){
// 			CurrentPosition = AnchorPosition.Down;
// 			UpdatePos ();
// 		}
// 		if(Input.GetButtonDown("Anchor Left")){
// 			CurrentPosition = AnchorPosition.Left;
// 			UpdatePos ();
// 		}
// 		if(Input.GetButtonDown("Anchor Right")){
// 			CurrentPosition = AnchorPosition.Right;
// 			UpdatePos ();
// 		}



// 	}


// 	void UpdatePos () {

// 		Vector3 newAnchorPos = PlayerCharacter.transform.position;

// 			switch (CurrentPosition) {
// 			case AnchorPosition.Up:
// 				if (TargetObject.transform.localRotation.eulerAngles.z == 0 || TargetObject.transform.localRotation.eulerAngles.z == 180) {
// 					newAnchorPos.y = newAnchorPos.y + 0.32f + (SpriteExtents.y * 2f);
// 					newAnchorPos.x = newAnchorPos.x - (SpriteExtents.x / 2f);
// 				} else {
// 					newAnchorPos.y = newAnchorPos.y + 0.32f + (SpriteExtents.x * 2f);
// 					newAnchorPos.x = newAnchorPos.x + (SpriteExtents.y / 2f);
// 				}

// 				break;
// 			case AnchorPosition.Down:
// 				if (TargetObject.transform.localRotation.eulerAngles.z == 0 || TargetObject.transform.localRotation.eulerAngles.z == 180) {
// 					newAnchorPos.y = newAnchorPos.y - 0.16f;
// 					newAnchorPos.x = newAnchorPos.x - (SpriteExtents.x / 2f);
// 				} else {

// 					newAnchorPos.x = newAnchorPos.x + (SpriteExtents.y / 2f);
// 					newAnchorPos.y = (newAnchorPos.y - 0.16f) - (SpriteExtents.x * 2f);
// 				}

// 				break;
// 			case AnchorPosition.Left:
// 				if (TargetObject.transform.localRotation.eulerAngles.z == 0 || TargetObject.transform.localRotation.eulerAngles.z == 180) {
// 					newAnchorPos.y = newAnchorPos.y + 0.32f + (SpriteExtents.y / 2f);
// 					newAnchorPos.x = (newAnchorPos.x - 0.32f) - (SpriteExtents.x * 2f);
// 				} else {
// 					newAnchorPos.x = newAnchorPos.x - 0.16f;
// 					newAnchorPos.y = newAnchorPos.y - (SpriteExtents.x / 2f);
// 				}

// 				break;
// 			case AnchorPosition.Right:
// 				if (TargetObject.transform.localRotation.eulerAngles.z == 0 || TargetObject.transform.localRotation.eulerAngles.z == 180) {
// 					newAnchorPos.y = newAnchorPos.y + 0.32f + (SpriteExtents.y / 2f);
// 					newAnchorPos.x = (newAnchorPos.x + 0.32f);
// 				} else {

// 					newAnchorPos.x = newAnchorPos.x + 0.32f + (SpriteExtents.y * 2f);
// 					newAnchorPos.y = newAnchorPos.y - (SpriteExtents.x / 2f);
// 				}
// 				break;
// 			default:
// 				break;
// 			}
	
// 		newAnchorPos.x = 0.32f * Mathf.Round (newAnchorPos.x / 0.32f);
// 		newAnchorPos.y = 0.32f * Mathf.Round (newAnchorPos.y / 0.32f);
// 		Anchor.transform.position = newAnchorPos;

// 		//do I need to set offset each time...?
// 		TargetObject.SetObjectOffset ();

// 	}

// }
