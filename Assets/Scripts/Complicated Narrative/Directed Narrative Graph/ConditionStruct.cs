using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serialized object for a single condition
/// Used to define pre and post conditions of actions
/// </summary>
[System.Serializable]
public class ConditionStruct {

	public string name;
	public bool value;
	public float priority;

}
