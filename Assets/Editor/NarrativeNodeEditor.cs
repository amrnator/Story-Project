using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NarrativeNode))]
public class NarritoveNodeEditor : Editor
{

    NarrativeNode t;
    SerializedObject GetTarget;
	SerializedProperty transitionList;
	int sceneIndex = 0;
	int conditionIndex = 0;
	int ListSize;

    void OnEnable()
    {
        t = (NarrativeNode)target;
        GetTarget = new SerializedObject(t);
		transitionList = GetTarget.FindProperty ("transitionStates");
    }

    public override void OnInspectorGUI()
    {
		GetTarget.Update();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Transition States", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Define the Transition states of this node");

		ListSize = transitionList.arraySize;

		sceneIndex = EditorGUILayout.Popup(sceneIndex, t.scenes);

		//Or add a new item to the List<> with a button
		if (GUILayout.Button("Add New"))
		{
			TransitionState x = new TransitionState ();

			x.sceneName = t.scenes [sceneIndex];

			t.transitionStates.Add (x);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

        //Display our list to the inspector window

        for (int i = 0; i < transitionList.arraySize; i++)
        {

            SerializedProperty transitionState = transitionList.GetArrayElementAtIndex(i);
            SerializedProperty transitionConditionList = transitionState.FindPropertyRelative("transitionConditions");
            SerializedProperty sceneName = transitionState.FindPropertyRelative("sceneName");

            // Display the Transition states and their fields

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(sceneName.stringValue);

            if (GUILayout.Button("Remove Transition"))
            {
                transitionConditionList.DeleteCommand();
                transitionList.DeleteArrayElementAtIndex(i);
                //Apply the changes to our list
                GetTarget.ApplyModifiedProperties();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();

            //field for setting conditions
            EditorGUILayout.LabelField("Transition Conditions", EditorStyles.boldLabel);

            //field for adding new conditions
            EditorGUILayout.BeginHorizontal();

            conditionIndex = EditorGUILayout.Popup(conditionIndex, t.conditionChoices);

            //Or add a new item to the List<> with a button
            if (GUILayout.Button("Add New Condition"))
            {
                ConditionList.Condition refedCond = t.conditionListComponent.conditionList[conditionIndex];

                Precondition preCond = new Precondition(ref refedCond);

                t.transitionStates[i].transitionConditions.Add(preCond);
            }

            EditorGUILayout.EndHorizontal();

            //if (transitionConditionList.arraySize > 0) {

            ///SerializedProperty condListcopy = transitionConditionList.Copy();    

            //list conditions
            EditorGUILayout.BeginVertical();

            for (int z = 0; z < transitionConditionList.arraySize; z++)
            {

                //Debug.Log("array size:  " + transitionConditionList.arraySize);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                SerializedProperty MyListRef = transitionConditionList.GetArrayElementAtIndex(z);
                SerializedProperty referencedCond = MyListRef.FindPropertyRelative("refrencedCondition");
                SerializedProperty condName = referencedCond.FindPropertyRelative("conditionName");
                SerializedProperty condValue = MyListRef.FindPropertyRelative("boolValue");

                // Display the property fields
                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField(condName.stringValue);

                //t.postConditions[i].value = EditorGUILayout.Toggle(condValue.boolValue);
				t.transitionStates[i].transitionConditions[z].boolValue = EditorGUILayout.Toggle(condValue.boolValue);

                if (GUILayout.Button("Remove Condition"))
                {
                    //PostcondList.DeleteArrayElementAtIndex(i);
                    transitionConditionList.DeleteArrayElementAtIndex(z);

                }

                EditorGUILayout.EndHorizontal();


                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();

        }


        EditorGUILayout.Space();
        EditorGUILayout.Space();

       

		//Apply the changes to our list
		GetTarget.ApplyModifiedProperties();
    }



}
