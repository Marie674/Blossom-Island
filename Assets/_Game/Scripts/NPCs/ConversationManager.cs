using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ConversationManager : MonoBehaviour {

//	private static ConversationManager s_Instance=null;
//
//	//public Character CurrentCharacter;
//
//	public static ConversationManager Instance{
//		get{
//			if (s_Instance == null) {
//				s_Instance = FindObjectOfType (typeof(ConversationManager)) as ConversationManager;
//			}
//			if (s_Instance == null) {
//				GameObject obj = new GameObject ("ConversationManager");
//				s_Instance = obj.AddComponent (typeof(ConversationManager)) as ConversationManager;
//				Debug.Log ("Could not locate ConversationManager object. Created one.");
//			}
//			return s_Instance;
//		}
//	}


	private List<Conversation> EligibleConversations = new List<Conversation>();

//	void GetEligibleConversations(Character character){
//		
//		EligibleConversations.Clear ();
//		List<Conversation> conversations = new List<Conversation>();
//
//		if (conversations.Count>0) {
//			foreach(Conversation conversation in conversations){
//				if (conversation.CheckConditions () == true) {
//					if (!character.SaidToday.Contains (conversation)) {
//						EligibleConversations.Add (conversation);
//					}
//				}
//			}
//			if (EligibleConversations.Count < 1) {
//				character.SaidToday.Clear ();
//				foreach(Conversation conversation in conversations){
//					if (conversation.CheckConditions () == true) {
//						EligibleConversations.Add (conversation);
//					}
//				}
//			}
//		}
//
//	}
//
//	void GetEligibleConversations(Character character, ConversationTypes pType){
//		EligibleConversations.Clear ();
//		List<Conversation> conversations = new List<Conversation>();
//		switch (pType) {
//		case ConversationTypes.Greeting:
//			conversations = character.Greetings;
//			break;
//		case ConversationTypes.Meeting:
//			conversations = character.Meetings;
//			break;
//		case ConversationTypes.Normal:
//			conversations = character.Conversations;
//			break;
//		default:
//			break;
//		}
//		if (conversations.Count>0) {
//			foreach(Conversation conversation in conversations){
//				if (conversation.CheckConditions () == true) {
//					if (!character.SaidToday.Contains (conversation)) {
//						EligibleConversations.Add (conversation);
//					}
//				}
//			}
//			if (EligibleConversations.Count < 1) {
//				character.SaidToday.Clear ();
//				foreach(Conversation conversation in conversations){
//					if (conversation.CheckConditions () == true) {
//							EligibleConversations.Add (conversation);
//					}
//				}
//			}
//		}
//
//	}


	Conversation GetSingleConversation(){
		var weights = new Dictionary<Conversation,int> ();
		foreach (Conversation conversation in EligibleConversations) {
//			weights.Add (conversation, conversation.Weight);
		}

		return WeightedRandomizer.From (weights).TakeOne ();
	}

	public void StartConversation () {

//		GetEligibleConversations (pCharacter, pType);
//
//		ConversationUI.Show ();
//		pCharacter.SaidToday.Add (CurrentConversation);
	}
//


}
