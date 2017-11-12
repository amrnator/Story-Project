using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Narrative event.
/// A super class for all narative actions.
/// Narrative events are basically the affordances of an interactable object.
/// Essentially the set of actions that the player can take when interacting with something.
/// These are attached to Interactable objects, and there can be multiple actions per object
/// </summary>
[ExecuteInEditMode, System.Serializable]
public class NarrativeEvent : MonoBehaviour {

	public string actionName;
	[HideInInspector]
	public ConditionList globalConditionList;
	[HideInInspector]
	public List<Precondition> preconditions = new List<Precondition>();
	[HideInInspector]
	public List<Postcondition> postConditions = new List<Postcondition>();
	[HideInInspector]
	public string[] choices;
	[HideInInspector]
	public NarrativeNode narrativeNode;

	public virtual void action (){
		print ("Whatever");
	}

}
