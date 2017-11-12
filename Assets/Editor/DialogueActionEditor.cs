using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(DialogueAction))]
public class DialogueActionEditor : Editor {

	DialogueAction t;
	SerializedObject GetTarget;
	SerializedProperty actionName;
	SerializedProperty PrecondList;
	SerializedProperty PostcondList;
	SerializedProperty ConditionList;
	SerializedProperty Dialogue;
	int ListSize;

	int precondChoice;
	int postcondChoice;

	void OnEnable()
	{
		t = (DialogueAction)target;
		GetTarget = new SerializedObject(t);
		PrecondList = GetTarget.FindProperty("preconditions");
		PostcondList = GetTarget.FindProperty("postConditions");
		actionName = GetTarget.FindProperty("actionName");
		ConditionList = GetTarget.FindProperty("globalConditionList");
		Dialogue = GetTarget.FindProperty ("dialogue");
	}

	public override void OnInspectorGUI()
	{
		//Update our list
		GetTarget.Update();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(actionName);

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		//preconditions lists
		EditorGUILayout.LabelField("Preconditions", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Define the Preconditions of this action");

		EditorGUILayout.BeginHorizontal();

		precondChoice = EditorGUILayout.Popup(precondChoice, t.choices);

		if (GUILayout.Button("Add New"))
		{
			ConditionList.Condition x = t.globalConditionList.conditionList[precondChoice];
			Precondition newCond = new Precondition(ref x);
			//newCond.refrencedCondition = t.globalConditionList.conditionList[precondChoice];
			if(t.preconditions == null) { Debug.Log("No t"); }
			t.preconditions.Add(newCond);
		}

		EditorGUILayout.EndHorizontal();

		//Display our list to the inspector window
		for (int i = 0; i < PrecondList.arraySize; i++)
		{

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			SerializedProperty MyListRef = PrecondList.GetArrayElementAtIndex(i);
			SerializedProperty referencedCond = MyListRef.FindPropertyRelative("refrencedCondition");
			SerializedProperty condName = referencedCond.FindPropertyRelative("conditionName");
			SerializedProperty condValue = MyListRef.FindPropertyRelative("value");

			// Display the property fields
			EditorGUILayout.BeginHorizontal("box");

			EditorGUILayout.LabelField(condName.stringValue);

			t.preconditions[i].boolValue = EditorGUILayout.Toggle(condValue.boolValue);

			if (GUILayout.Button("Remove"))
			{
				PrecondList.DeleteArrayElementAtIndex(i);
			}

			EditorGUILayout.EndHorizontal();


			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		//post conditions list
		EditorGUILayout.LabelField("Postconditions", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Define the Postconditions of this action");

		EditorGUILayout.BeginHorizontal();

		postcondChoice = EditorGUILayout.Popup(postcondChoice, t.choices);

		if (GUILayout.Button("Add New"))
		{
			ConditionList.Condition x = t.globalConditionList.conditionList[postcondChoice];
			Postcondition newCond = new Postcondition(ref x, t.globalConditionList);
			//newCond.refrencedCondition = t.globalConditionList.conditionList[postcondChoice];

			t.postConditions.Add(newCond);
		}

		EditorGUILayout.EndHorizontal();

		//Display our list to the inspector window
		for (int i = 0; i < PostcondList.arraySize; i++)
		{

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			SerializedProperty MyListRef = PostcondList.GetArrayElementAtIndex(i);
			SerializedProperty referencedCond = MyListRef.FindPropertyRelative("refrencedCondition");
			SerializedProperty condName = referencedCond.FindPropertyRelative("conditionName");
			SerializedProperty condValue = MyListRef.FindPropertyRelative("value");

			// Display the property fields
			EditorGUILayout.BeginHorizontal("box");

			EditorGUILayout.LabelField(condName.stringValue);

			t.postConditions[i].boolValue = EditorGUILayout.Toggle(condValue.boolValue);

			if (GUILayout.Button("Remove"))
			{
				PostcondList.DeleteArrayElementAtIndex(i);
			}

			EditorGUILayout.EndHorizontal();


			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
			
		EditorGUILayout.PropertyField (Dialogue);

		//Apply the changes to our list
		GetTarget.ApplyModifiedProperties();

	}

}
