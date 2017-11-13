using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Narrative action.
/// A basic action that can be assigned to interactive object. 
/// has a set of preconditions and post conditions.
/// when the player activates this action, it checks if the preconditions have been met,
/// if so, then it changes the conditionlist conditions to match those of the postconditions. 
/// </summary>
public class NarrativeAction : NarrativeEvent {

    public UnityEvent TriggeredMethods;

    void Awake() {
        //get the condition list
        globalConditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();

		narrativeNode = globalConditionList.GetComponent<NarrativeNode>();
    }

    
    public void Update()
    {
        #if UNITY_EDITOR
        choices = globalConditionList.ToStringArray();
        #endif
    }

	//delegate funtions for event handling
    public delegate void narrativeUpdate();
    public static event narrativeUpdate actionMade;

    //check conditions and execute if they are satisfied
	public override void action() {

        //check preconditions
        for (int i = 0; i < preconditions.Count; i++) {

            bool check = preconditions[i].checkCondition();

            if (!check) {
                Debug.Log("Action failed, precondition: " + preconditions[i].refrencedCondition.conditionName + " not satisfied");
                return ;
            }
        }

        //preconditions satisfied, execute post conditions
        print("Conditions satisfied executing: " + this.actionName);
        for (int i = 0; i < postConditions.Count; i++) {
            //change conditions
            postConditions[i].execute();
            //invoke methods
            TriggeredMethods.Invoke();
        }

        //ping the Narrative Node
        narrativeNode.checkState();

        //signal to everyone else that an action has taken place
		if (null != actionMade)
        {
            actionMade();
        }

        return ;
    }
}

public enum ComparisonType {LessThan, GreaterThan, Equal}

public enum AssignmentType {INCREMENT, DECREMENT, SET}

[System.Serializable]
public class Precondition {
    //refernce to condition in list
    public ConditionList.Condition refrencedCondition;

    //specified values
    public bool boolValue;

	public int integerValue;

	//specified comparison
	public ComparisonType comparisonType;

    public ConditionList conditionList;

    public Precondition(ref ConditionList.Condition condition)
    {
        refrencedCondition = condition;
    }

    void Start() {
        //TODO maybe improve this so conditonList can be passed by constructor without being reset in Play mode. 
        //conditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();
    }

    //check if precondition is satisfied
    public bool checkCondition() {
        
        conditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();

        Predicate<ConditionList.Condition> condFinder = (ConditionList.Condition c) => { return c.conditionName == refrencedCondition.conditionName; };

        ConditionList.Condition trueCondition = conditionList.conditionList.Find(condFinder);

		if (trueCondition.conditonType == ConditionList.ConditionType.Boolean) {
			if (trueCondition.boolean == boolValue) {
				return true;
			}
		}

		if (trueCondition.conditonType == ConditionList.ConditionType.Integer) {

			if (comparisonType == ComparisonType.Equal) {

				if (trueCondition.integer == integerValue) {
					return true;
				}
			
			}

			if (comparisonType == ComparisonType.LessThan) {

				if (trueCondition.integer < integerValue) {
					return true;
				}

			}

			if (comparisonType == ComparisonType.GreaterThan) {

				if (trueCondition.integer > integerValue) {
					return true;
				}

			}
		}


        return false;
    }
}

[System.Serializable]
public class Postcondition  {

    public ConditionList.Condition refrencedCondition;

    ConditionList.Condition trueCondition;

    public bool boolValue;

	public int integerValue;

	//specified assignment
	public AssignmentType assignmentType;

    //array of UnityEvents


    public ConditionList conditionList;

    public Postcondition(ref ConditionList.Condition condition, ConditionList glovalConditionList)
    {
        refrencedCondition = condition;

        conditionList = glovalConditionList;

		assignmentType = AssignmentType.INCREMENT;
    }

    void Start()
    {
        //TODO maybe improve this so conditonList can be passed by constructor without being reset in Play mode. 
        //conditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();
    }

    //change refenced condition to value
    public void execute() {
        //TODO maybe improve this so conditonList can be passed by constructor without being reset in Play mode. 
        conditionList = GameObject.FindGameObjectWithTag("NarrativeController").GetComponent<ConditionList>();

        Predicate<ConditionList.Condition> condFinder = (ConditionList.Condition c) => { return c.conditionName == refrencedCondition.conditionName; };

        trueCondition = conditionList.conditionList.Find(condFinder);

		//if its a bool, assign specified bool
		if (trueCondition.conditonType == ConditionList.ConditionType.Boolean) {
			trueCondition.boolean = boolValue;
		}

		if (trueCondition.conditonType == ConditionList.ConditionType.Integer) {

			if (assignmentType == AssignmentType.INCREMENT) {
				trueCondition.integer++;
			}
			if (assignmentType == AssignmentType.DECREMENT) {
				trueCondition.integer--;
			}
			if (assignmentType == AssignmentType.SET) {
				trueCondition.integer = this.integerValue;
			}
		}

    }
}

