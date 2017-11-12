using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Narrative delegate ties methods and events to the Narrative's condition list,
/// The user specifies the conditions neccessary for a certain event to trigger. 
/// </summary>
[ExecuteInEditMode, System.Serializable]
public class NarrativeDelegate : MonoBehaviour {

    //list of preconditions needed to activate event
    public List<Precondition> preconditions = new List<Precondition>();

    public ConditionList globalConditionList;
    //delegate  
    public UnityEvent triggeredEvent;

    public string[] choices;

    void Awake()
    {
        if (triggeredEvent == null)
            triggeredEvent = new UnityEvent();

        //get the condition list
        globalConditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();
    }
    public void Update()
    {
        #if UNITY_EDITOR
                choices = globalConditionList.ToStringArray();
        #endif
    }

    //set specific event to actionMade delegate
    void OnEnable()
    {
        NarrativeAction.actionMade += open;
    }
    void OnDisable()
    {
        NarrativeAction.actionMade -= open;
    }

    //check conditionlist is the current state allows for this concrete action
    void open()
    {
        //check preconditions
        for (int i = 0; i < preconditions.Count; i++)
        {

            bool check = preconditions[i].checkCondition();

            if (!check)
            {
                return;
            }
        }

        //if conditions are satidied, invoke the specified method
        triggeredEvent.Invoke();
    }



}
