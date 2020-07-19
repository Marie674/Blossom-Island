// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class HandCraftingUI : MonoBehaviour {

// 	public List<RecipeContainer> HandRecipes;

// 	List<RecipeContainer> KnownRecipes;
// 	private List<Transform> DrawnItems = new List<Transform>();
// 	public RecipeUI RecipeUIPrefab;
// 	public Transform RecipeUIContainer;


// 	void OnEnable(){
// 		KnownRecipes = CraftingManager.Instance.KnownRecipes;
// 		GetComponent<WindowToggle> ().Window.onOpen.AddListener (delegate {
// 			Draw (HandRecipes);	
// 		});
// 	}



// 	void OnDisable(){
// 		//Clear ();
// 		GetComponent<WindowToggle> ().Window.onOpen.RemoveAllListeners();
// 	}

// 	void Clear(){
// 		foreach (Transform RecipeUI in DrawnItems) {
// 			Destroy (RecipeUI.gameObject);
// 		}
// 		DrawnItems.Clear ();
// 	}

// 	public void Draw(List<RecipeContainer> pRecipes){
// 		Clear ();
// 		foreach (RecipeContainer recipeContainer in pRecipes) {
// 			if (KnownRecipes.Contains (recipeContainer)) {
// 			RecipeUI recipeUI = Instantiate (RecipeUIPrefab, RecipeUIContainer);
// 			DrawnItems.Add (recipeUI.transform);
// 				recipeUI.Recipe = recipeContainer.Recipe;
// 				recipeUI.Init (GetComponent<WindowToggle>(), true,null);
// 			}
// 		}

// 	}

// }
