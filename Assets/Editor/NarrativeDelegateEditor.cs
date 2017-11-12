using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(NarrativeDelegate))]
public class NarrativeDelegateEditor : Editor
{

    NarrativeDelegate t;
    SerializedObject GetTarget;
    SerializedProperty PrecondList;
    SerializedProperty ConditionList;
    SerializedProperty trigger;
    int ListSize;

    int precondChoice;
    int postcondChoice;

    void OnEnable()
    {
        t = (NarrativeDelegate)target;
        GetTarget = new SerializedObject(t);
        PrecondList = GetTarget.FindProperty("preconditions");
        ConditionList = GetTarget.FindProperty("globalConditionList");
        trigger = GetTarget.FindProperty("triggeredEvent");
    }

    public override void OnInspectorGUI()
    {
        //Update our list
        GetTarget.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(trigger);

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
            if (t.preconditions == null) { Debug.Log("No t"); }
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
            SerializedProperty condValue = MyListRef.FindPropertyRelative("boolValue");

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

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}

