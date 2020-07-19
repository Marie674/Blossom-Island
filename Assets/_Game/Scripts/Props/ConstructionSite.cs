using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
using ItemSystem;
using UnityEngine.SceneManagement;

public class ConstructionSite : MonoBehaviour {
	
	private STETilemap Map;
	public SpriteRenderer DisplaySprite;

	public bool ReadyForContractor = false;
	public bool ConstructionStarted = false;

	public Building Building;
	public StorageObject Box;
	public ConstructionSign Sign;

	public string LevelName;
	public float TimeStarted;
	public float TargetTime;

	public List<GameObject> Markers;
	public GameObject Collider;

	void Awake(){
		LevelName = SceneManager.GetActiveScene ().name;
	}

	void Start () {
		DisplaySprite = GetComponent<SpriteRenderer> ();
	}

	public bool CheckMaterials(){
		foreach (BuildingMaterial material in Building.Materials)
		{
			InventoryItemStack boxStack = Box.FindItemStack(material.Item.item);
			if(boxStack==null){
				return false;
			}
			if(boxStack.Amount<material.TargetAmount){
				return false;
			}
		}
		return true;
	}
	public void ConstructionReady(){
		ReadyForContractor = true;
		BuildingManager.Instance.ReadySites.Add (this);
	}

	public void SetPhase(int pNumber){
		Building.CurrentPhase = pNumber;
		foreach (BuildingPhase phase in Building.Phases) {
			phase.Object.SetActive (false);
		}
		Building.Phases [Building.CurrentPhase].Object.SetActive (true);
		if (Building.CurrentPhase >= Building.Phases.Count - 1) {
			BuildingComplete ();
		}
	}
	void BuildingComplete(){
		TimeManager.OnMinuteChanged -= ProcessConstruction;
		GetComponent<SpriteRenderer> ().enabled = false;
		Collider.SetActive (false);
		foreach (GameObject marker in Markers) {
			marker.SetActive (false);
		}
	}

	public void StartConstruction(){
		if(BuildingManager.Instance.ReadySites.Contains(this)){
			BuildingManager.Instance.ReadySites.Remove (this);
		}
		Destroy(Sign.gameObject);
		Destroy(Box.gameObject);
		TimeStarted = TimeManager.Instance.PassedMinutes;
		TargetTime = TimeStarted + Building.Phases [Building.CurrentPhase].BuildingTime;
		TimeManager.OnMinuteChanged += ProcessConstruction;
		Collider.SetActive (true);
	}

	void ProcessConstruction(){
		if (TimeManager.Instance.PassedMinutes > TargetTime) {
			TimeManager.OnMinuteChanged -= ProcessConstruction;
			SetPhase (Building.CurrentPhase+1);
			if (Building.CurrentPhase >= Building.Phases.Count - 1) {
				BuildingComplete ();
			} else {
				StartConstruction();
			}
		}
	}

}
