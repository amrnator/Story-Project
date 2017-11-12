using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

/// <summary>
/// Narrative node.
/// Representation of a contained node in a graph of story states.
/// 
/// Contains:
/// 	Transition scenes: what scenes can come after this one.
/// 	required transition state: state needed to switch to a certain scene.
/// 	
/// What it does:
/// 	Monitor the story state and load a specific scene when a transition state is satisfied.
/// </summary>
[ExecuteInEditMode, System.Serializable]
public class NarrativeNode : MonoBehaviour {

    public ConditionList conditionListComponent;

    public string[] conditionChoices;

	public List<TransitionState> transitionStates = new List<TransitionState>();

    public LevelLoader loader;

	public string[] scenes;

    public void Awake() {
		
        conditionListComponent = gameObject.GetComponent<ConditionList>();

        loader = GetComponent<LevelLoader>();
    }

    public void Update() {
		#if UNITY_EDITOR
        conditionChoices = conditionListComponent.ToStringArray();

		//update scene names
		Reset();
		#endif
    }

	/// <summary>
	/// Checks the conditionList to see if it satisfys any TransitionStates.
	/// If it does, then the associated scene is loaded
	/// </summary>
	public void checkState(){

		foreach(TransitionState t in transitionStates){

			bool check = true;

			foreach(Precondition c in t.transitionConditions){
				
				if (!c.checkCondition ()) {
					//transition state not statisfied
					check = false;
					break;
				}
			}

			if (!check) {
				//transition state not statisfied
				continue;
			}

            //change scene to specified scene
			Debug.Log("Transition state achieved, loading scene:  " + t.sceneName );
            loader.LoadLevel(t.sceneName);
		}
	}


	//#if UNITY_EDITOR
	private static string[] ReadNames()
	{
		List<string> temp = new List<string>();
		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				temp.Add(name);
			}
		}
		return temp.ToArray();
	}

	//[UnityEditor.MenuItem("CONTEXT/ReadSceneNames/Update Scene Names")]
	private static void UpdateNames(UnityEditor.MenuCommand command)
	{
		NarrativeNode context = (NarrativeNode)command.context;
		context.scenes = ReadNames();
	}

	private void Reset()
	{
		scenes = ReadNames();
	}
	//#endif
}

[System.Serializable]
public class TransitionState {
	
	public List<Precondition> transitionConditions  = new List<Precondition> ();

	public string sceneName;
}



