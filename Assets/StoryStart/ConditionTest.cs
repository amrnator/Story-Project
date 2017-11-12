using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTest : MonoBehaviour {

    ConditionList condList;

    public string conditionName;

    void Start() {
        condList = GetComponent<ConditionList>();
    }

    void Update() {

        ConditionList.Condition found;

        foreach(ConditionList.Condition c in condList.conditionList)
        {
            if (c.conditionName.Equals(conditionName)) {
                found = c;

                //print(c.conditionName + "  " + c.boolean);

                break;
            }
        }

        

        
    }

}
