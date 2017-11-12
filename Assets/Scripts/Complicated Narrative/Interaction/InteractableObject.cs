using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable object.
/// A component of an object that the player can interact with. 
/// Once an Interactable object is focused, it start the inteaction command in the behavior tree.
/// </summary>
public class InteractableObject : MonoBehaviour {

    public float radius = 3f;

    bool isFocus = false;

	bool isClose = false;

    PlayerBehavior playerBehavior;

	//list of available actions on this object
	List<NarrativeEvent> actions;

    Transform player;

	PlayerUI UI;

    void Start() {
        playerBehavior = FindObjectOfType<PlayerBehavior>();
		NarrativeEvent[] acts = GetComponents<NarrativeEvent>();
		UI = GameObject.FindGameObjectWithTag ("UI").GetComponent<PlayerUI>();

		actions = new List<NarrativeEvent> ();

		foreach (NarrativeEvent a in acts) {
			if (a.enabled) {
				actions.Add (a);
			}
		}
    }

    void Update() {

        if (isFocus) {
            float distance = Vector3.Distance(player.position, transform.position);
			if (distance <= radius && !isClose) {
				//add the available actions of this object to the menu
				UI.addInteractions(actions);

				isClose = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform) {
        isFocus = true;
        player = playerTransform;
        playerBehavior.PlayerCommandInteract(this);
    }

    public void OnDefocused()
    {
		isFocus = false;
		isClose = false;
        player = null;
		UI.removeInteractions ();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
