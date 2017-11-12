using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dialogue action.
/// An action that will open a dialogue box and display text when activated
/// </summary>
public class DialogueAction : NarrativeEvent {

	public Dialogue dialogue;

	void Awake() {
		//get the condition list
		globalConditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();

		narrativeNode = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<NarrativeNode>();
	}

		
	public void Update()
	{
		#if UNITY_EDITOR
		choices = globalConditionList.ToStringArray();
		#endif
	}

	public override void action(){
		FindObjectOfType<DialogueManager> ().StartDialogue (dialogue);
	}
}
