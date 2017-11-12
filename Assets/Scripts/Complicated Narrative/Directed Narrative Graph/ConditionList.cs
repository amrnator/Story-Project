using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionList : MonoBehaviour {

    [System.Serializable]
    public class Condition
    {
        public string conditionName;

        public ConditionType conditonType;

        public bool boolean;

        public int integer;

        public virtual bool checkCondition()
        {
            throw new NotImplementedException();
        }

		public Condition(){
			conditonType = ConditionType.Boolean;
		}
    }

    public enum ConditionType {Integer, Boolean}

    public List<Condition> conditionList = new List<Condition>(1);

    void AddNew()
    {
        //Add a new index position to the end of our list
        conditionList.Add(new Condition());
    }

    void Remove(int index)
    {
        //Remove an index position from our list at a point in our list array
        conditionList.RemoveAt(index);
    }

    public string[] ToStringArray() {

        string[] result = new string[conditionList.Count];

        int iterator = 0;
        foreach (Condition c in conditionList) {
            result[iterator] = c.conditionName;
            iterator++;
        }

        return result;
    }
}
