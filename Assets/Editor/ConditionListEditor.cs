
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ConditionList))]

public class CustomListEditor : Editor
{
    
    ConditionList t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable()
    {
        t = (ConditionList)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("conditionList"); // Find the List in our script and create a refrence of it
    }

    public override void OnInspectorGUI()
    {
        //Update our list
        GetTarget.Update();

        //Resize our list
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Define the global conditions");
        ListSize = ThisList.arraySize;
        ListSize = EditorGUILayout.IntField("List Size", ListSize);

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Or add a new Condition to the List<> with a button
        if (GUILayout.Button("Add New"))
        {
            t.conditionList.Add(new ConditionList.Condition());
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Display our list to the inspector window
        for (int i = 0; i < ThisList.arraySize; i++)
        {
        
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyConditionName = MyListRef.FindPropertyRelative("conditionName");
            SerializedProperty MyInt = MyListRef.FindPropertyRelative("integer");
            SerializedProperty Mybool = MyListRef.FindPropertyRelative("boolean");
            SerializedProperty Myenum = MyListRef.FindPropertyRelative("conditonType");

            // Display the property fields

            EditorGUILayout.PropertyField(MyConditionName);
			EditorGUILayout.PropertyField(Myenum);

			//display property field depending on type
			if (Myenum.enumValueIndex == (int)ConditionList.ConditionType.Boolean) {
				EditorGUILayout.PropertyField (Mybool);
			} else {
				EditorGUILayout.PropertyField(MyInt);
			}
				
            // Array fields with remove at index
            if (GUILayout.Button("Remove This Index (" + i.ToString() + ")"))
            {
                ThisList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}


