// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class WorkStationCraftingUI : MonoBehaviour {

// 	List<RecipeContainer> KnownRecipes;
// 	private List<Transform> DrawnItems = new List<Transform>();
// 	public RecipeUI RecipeUIPrefab;
// 	public Transform RecipeUIContainer;


// 	void OnEnable(){
// 		KnownRecipes = CraftingManager.Instance.KnownRecipes;
// 	}

// 	void OnDisable(){
// 		//Clear ();
// 	}

// 	void Clear(){
// 		foreach (Transform RecipeUI in DrawnItems) {
// 			Destroy (RecipeUI.gameObject);
// 		}
// 	}

// 	public void Draw(List<RecipeContainer> pRecipes, WorkStation pStation){
// 		GetComponent<WindowToggle> ().Toggle ();
// 		Clear ();
// 		foreach (RecipeContainer recipeContainer in pRecipes) {
// 			if (KnownRecipes.Contains (recipeContainer)) {
// 			RecipeUI recipeUI = Instantiate (RecipeUIPrefab, RecipeUIContainer);
// 			DrawnItems.Add (recipeUI.transform);
// 				recipeUI.Recipe = recipeContainer.Recipe;
// 				recipeUI.Init (GetComponent<WindowToggle>(),false,pStation);
// 			}
// 		}

// 	}

// }
