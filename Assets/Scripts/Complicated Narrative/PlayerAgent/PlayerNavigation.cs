using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

/// <summary>
/// Component to give info to aid player navigation
/// </summary>
public class PlayerNavigation : MonoBehaviour {

	public List<GameObject> navNodes;

	// Use this for initialization
	void Awake () {
        navNodes = new List<GameObject>(GameObject.FindGameObjectsWithTag("NavNode"));
    }

    public Transform getRandomNode() {

        int choice = Random.Range(0, navNodes.Count);

        print("random Index: " + choice);

        return navNodes[choice].transform;

    }

	public Transform getUnexploredNode(){

		foreach (GameObject node in navNodes) {
			if(!node.GetComponent<NavigationNode>().visited){
				return node.transform;
			}
		}

		return null;
	}
}
