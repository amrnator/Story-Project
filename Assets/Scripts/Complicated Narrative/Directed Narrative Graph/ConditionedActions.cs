using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionedActions : MonoBehaviour {

	public NarrativeController mainNarrativeController;

	public string actionName;

    [Header("Preconditions")]
	public ConditionStruct [] preconditionsStruct;
    
	[Space]

    [Header("Postconditions")]
    public ConditionStruct[] postconditionsStruct;

	public Dictionary <string, bool> preconditions;
	public Dictionary <string, bool> postconditions;

    void Awake () {
        // Check if all conditions defined are already defined in mainNarrativeController.
        // If so, add them to the dictionary
        // If not, throw an exception.

        preconditions = new Dictionary<string, bool>();
        postconditions = new Dictionary<string, bool>();

        //check preconditions
        foreach (ConditionStruct x in preconditionsStruct) {
            bool determinate = false;

            for (int i = 0; i < mainNarrativeController.conditionsStruct.Length; i++) {

                if (x.name.Equals(mainNarrativeController.conditionsStruct[i].name))
                {
                    determinate = true;     //set true if the condition exists in the Narrative Controller
                }

            }

            if (determinate)
            {
                //add to dictionary
                preconditions.Add(x.name, x.value);
            }
            else {
                throw new System.Exception("Preconditions in " + gameObject.name + " do not exist in the Narrative Controller");
            }
        }

        //check postconditions
        foreach (ConditionStruct x in postconditionsStruct)
        {
            bool determinate = false;

            for (int i = 0; i < mainNarrativeController.conditionsStruct.Length; i++)
            {

                if (x.name.Equals(mainNarrativeController.conditionsStruct[i].name))
                {
                    determinate = true;     //set true if the condition exists in the Narrative Controller
                }

            }

            if (determinate)
            {
                //add to dictionary
                postconditions.Add(x.name, x.value);
            }
            else {
                throw new System.Exception("Postconditions in " + gameObject.name + " do not exist in the Narrative Controller");
            }
        }
    }

	public bool checkPrecondition () {
		
		return true;
	}
}
