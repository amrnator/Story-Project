using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NarrativeAction))]
public class NarrativeActionEditor : Editor {

    NarrativeAction t;
    SerializedObject GetTarget;
    SerializedProperty actionName;
    SerializedProperty PrecondList;
    SerializedProperty PostcondList;
    SerializedProperty ConditionList;
    SerializedProperty triggeredMethods;

    int ListSize;

    int precondChoice;
    int postcondChoice;
	int preCondIntegerValue;

    void OnEnable()
    {
        t = (NarrativeAction)target;
        GetTarget = new SerializedObject(t);
        PrecondList = GetTarget.FindProperty("preconditions");
        PostcondList = GetTarget.FindProperty("postConditions");
        actionName = GetTarget.FindProperty("actionName");
        ConditionList = GetTarget.FindProperty("globalConditionList");
        triggeredMethods = GetTarget.FindProperty("TriggeredMethods");
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
			SerializedProperty condBoolValue = MyListRef.FindPropertyRelative("boolValue");
			SerializedProperty condintValue = MyListRef.FindPropertyRelative("integerValue");
			SerializedProperty compType = MyListRef.FindPropertyRelative("comparisonType");

            // Display the property fields
            EditorGUILayout.BeginHorizontal("box");

			EditorGUILayout.LabelField(condName.stringValue);

			//change what values can be changed depending on type
			if (t.preconditions [i].refrencedCondition.conditonType == global::ConditionList.ConditionType.Boolean) {
				//if its a bool
				t.preconditions [i].boolValue = EditorGUILayout.Toggle (condBoolValue.boolValue);
			} else {
				//its an int
				GUILayout.Label("Integer Value");
				t.preconditions [i].integerValue = EditorGUILayout.IntField(condintValue.intValue);

				EditorGUILayout.PropertyField (compType);
			}

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
        
        //POST CONDITIONS

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
			SerializedProperty boolCondValue = MyListRef.FindPropertyRelative("boolValue");
			SerializedProperty intCondValue = MyListRef.FindPropertyRelative("integerValue");
			SerializedProperty asngType = MyListRef.FindPropertyRelative("assignmentType");

            // Display the property fields
            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField(condName.stringValue);

			//change what values can be changed depending on type
			if (t.postConditions [i].refrencedCondition.conditonType == global::ConditionList.ConditionType.Boolean) {
				//if its a bool
				t.postConditions[i].boolValue = EditorGUILayout.Toggle(boolCondValue.boolValue);
			} else {
				//its an int

				EditorGUILayout.BeginVertical ();

				EditorGUILayout.PropertyField (asngType);

				if (t.postConditions [i].assignmentType == AssignmentType.SET) {
					t.postConditions[i].integerValue = EditorGUILayout.IntField (intCondValue.intValue);
				}

				EditorGUILayout.EndVertical ();

			}

            if (GUILayout.Button("Remove"))
            {
                PostcondList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(triggeredMethods);

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();

    }

}
