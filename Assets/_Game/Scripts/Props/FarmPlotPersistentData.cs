// using UnityEngine;
// using System.Collections;
// using PixelCrushers.DialogueSystem;

// public class PersistentDataTemplate : MonoBehaviour //<--Copy this file. Rename the file and class name.
// { 
// 	/// <summary>
// 	/// Assign a Lua variable name to keep track of whether the GameObject
// 	/// has been destroyed. If this is blank, it uses the name of the
// 	/// GameObject for the variable name. If the variable is <c>true</c>,
// 	/// the GameObject has been destroyed.
// 	/// </summary>
// 	[Tooltip("Unique Dialogue System variable (Boolean) to record whether the GameObject has been destroyed/disabled.")]
// 	public string variableName = string.Empty;
// 	protected string ActualVariableName
// 	{
// 		get { return string.IsNullOrEmpty(variableName) ? DialogueActor.GetPersistentDataName(transform) : variableName; }
// 	}

// 	FarmPlot Plot;
// 	void Start(){
// 		Plot = GetComponent<FarmPlot> ();
// 	}
//     public void OnRecordPersistentData()
//     {
//         // Add your code here to record data into the Lua environment.
//         // Typically, you'll record the data using a line similar to:
//         //    DialogueLua.SetVariable("someVarName", someData);
//         // or:
//         //    DialogueLua.SetActorField("myName", "myFieldName", myData);
//         //
//         // Note that you can use this static method to get the actor 
//         // name associated with this GameObject:
//         //    var actorName = OverrideActorName.GetActorName(transform);
// //		DialogueLua.SetVariable("someVarName", someData);

//     }

//     public void OnApplyPersistentData()
//     {
//         // Add your code here to get data from Lua and apply it (usually to the game object).
//         // Typically, you'll use a line similar to:
//         // myData = DialogueLua.GetActorField(name, "myFieldName").AsSomeType;
//         //
//         // When changing scenes, OnApplyPersistentData() is typically called at the same 
//         // time as Start() methods. If your code depends on another script having finished 
//         // its Start() method, use a coroutine to wait one frame. For example, in 
//         // OnApplyPersistentData() call StartCoroutine(DelayedApply());
//         // Then define DelayedApply() as:
//         // IEnumerator DelayedApply() {
//         //     yield return null; // Wait 1 frame for other scripts to initialize.
//         //     <your code here>
//         // }

//     }

//     public void OnEnable()
//     {
//         // This optional code registers this GameObject with the PersistentDataManager.
//         // One of the options on the PersistentDataManager is to only send notifications
//         // to registered GameObjects. The default, however, is to send to all GameObjects.
//         // If you set PersistentDataManager to only send notifications to registered
//         // GameObjects, you need to register this component using the line below or it
//         // won't receive notifications to save and load.
//         PersistentDataManager.RegisterPersistentData(this.gameObject);
//     }

//     public void OnDisable()
//     {
//         // Unsubscribe the GameObject from PersistentDataManager notifications:
//         PersistentDataManager.RegisterPersistentData(this.gameObject);
//     }

//     //--- Uncomment this method if you want to implement it:
//     //public void OnLevelWillBeUnloaded() 
//     //{
//     // This will be called before loading a new level. You may want to add code here
//     // to change the behavior of your persistent data script. For example, the
//     // IncrementOnDestroy script disables itself because it should only increment
//     // the variable when it's destroyed during play, not because it's being
//     // destroyed while unloading the old level.
//     //}

// }



// /**/
