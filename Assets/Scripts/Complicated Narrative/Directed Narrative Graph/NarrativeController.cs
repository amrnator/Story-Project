using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Narrative controller used for processing preconditions and postconditions of actions
/// Actors in the scene will have actions attached to them, and those actions interact with this single script
/// </summary>
public class NarrativeController : MonoBehaviour {
	
	public Text conditionsText;
	public ConditionStruct [] conditionsStruct;     //Array of conditions set in the scene
	private Dictionary <string, bool> conditions;   //Disctionary of World conditions

//	[System.Serializable]
//	public struct CurrentConditions {
//		public string conditionName;
//		public bool isTrue;
//	}
//	public CurrentConditions[] currentConditions;

	void Awake () {
		conditions = new Dictionary <string, bool> ();
		string temp = "";
		for (int i = 0; i < conditionsStruct.Length; i++) {
			conditions.Add (conditionsStruct [i].name, conditionsStruct [i].value);
			temp += conditionsStruct [i].name + " = " + conditionsStruct [i].value.ToString () + "\n";
		}
		conditionsText.text = temp;
	}

    /// <summary>
    /// Executes a set of actions described by the Conditioned Actions script of the Gameobject that called this method 
	/// Returns true if action was completed, false if preconditions were not met. 
    /// </summary>

	public bool Action(){ 
		return this.Action (null);
	}

	//method allows one to pass a specific Conditioned action if needed
	public bool Action (ConditionedActions action) {
		bool status;

		//Gameobject that called this method
		GameObject actionSender;

		if (action == null) {
			actionSender = EventSystem.current.currentSelectedGameObject;
		} else {
			actionSender = action.gameObject;
		}

		//Debug.Log (actionSender.name + " initiated. Checking preconditions...");

		ConditionStruct[] pre;
		ConditionStruct[] post;

        //get pre and post conditions
		if (action == null) { //if action is null this is an event, use get component
			pre = actionSender.GetComponent <ConditionedActions> ().preconditionsStruct;
			post = actionSender.GetComponent <ConditionedActions> ().postconditionsStruct;
		} else { //if action isn't null, then the action has been passed
			pre = action.preconditionsStruct;
			post = action.postconditionsStruct;
		}


        //check if preconditions have been satisfied
		bool determinant = true;
		foreach (ConditionStruct pc in pre) {
			if (conditions [pc.name] != pc.value) {
				determinant = false;    //set to false if preconditions have not been satisfied 
			}
		}

        //if preconditons met, execute post conditions
		if (determinant == true) {
			//Debug.Log ("Preconditions meet. Changing post conditions...");
			foreach (ConditionStruct pc in post) {
				conditions [pc.name] = pc.value;
			}
			//Debug.Log ("Postconditions changed.");
			status = true;
		} else {
			Debug.Log ("Preconditions check failed. Aborted.");
			status = false;	//return false if action failed
		}

        //list in UI
		string temp = "";
		foreach (KeyValuePair <string, bool> kvp in conditions) {
			temp += kvp.Key + " = " + kvp.Value.ToString () + "\n";
		}
		conditionsText.text = temp;

		return status; 	//return true if action is successful

//		if (checkPrecondition (actionName)) {
//			Debug.Log ("Preconditions matched. Running behaviors...");
//			// Run behavior trees
//			// ...
//			// ...
//			// Behaviors completed
//			setPostcondition (actionName);
//			Debug.Log (actionName + " successfully completed.");
//			string temp = "";
//			foreach( KeyValuePair <string, bool> kvp in conditions )
//			{
//				temp += kvp.Key + " = " + kvp.Value.ToString () + "\n";
//			}
//			conditionsText.text = temp;
//		} else {
//			Debug.Log (actionName + " failed to complete: preconditions have no match.");
//		}

	}

//	private bool checkPrecondition (string actionName) {
//		bool result1, result2;
//		bool noErr1 = conditions.TryGetValue ("Player 1 Idle", out result1);
//		bool noErr2 = conditions.TryGetValue ("Player 2 Idle", out result2);
//		if (noErr1 && noErr2) {
//			switch (actionName) {
//			case "Action 1":
//				if (result1 && result2) {
//					return true;
//				} else
//					return false;
//			case "Action 2":
//				if (result1 && !result2) {
//					return true;
//				} else
//					return false;
//			case "Action 3":
//				if (!result1 && result2) {
//					return true;
//				} else
//					return false;
//			case "Action 4":
//				if (!result1 && !result2) {
//					return true;
//				} else
//					return false;
//			default:
//				return false;
//			}
//		} else {
//			return false;
//		}
//	}
//
//	private void setPostcondition (string actionName) {
//		switch (actionName) {
//		case "Action 1": 
//			conditions ["Player 1 Idle"] = true;
//			conditions ["Player 2 Idle"] = false;
//			break;
//		case "Action 2": 
//			conditions ["Player 1 Idle"] = false;
//			conditions ["Player 2 Idle"] = true;
//			break;
//		case "Action 3": 
//			conditions ["Player 1 Idle"] = true;
//			conditions ["Player 2 Idle"] = true;
//			break;
//		case "Action 4": 
//			conditions ["Player 1 Idle"] = false;
//			conditions ["Player 2 Idle"] = false;
//			break;
//		}
//	}
}
