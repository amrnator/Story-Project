using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationNode : MonoBehaviour {

	public bool visited = false;

	void OnCollisionEnter(Collision collision)
	{
		visited = true;
	}
}
