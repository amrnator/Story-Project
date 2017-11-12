using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Player controller.
/// Main script that starts player actions, such as explore, stop, and interact.
/// Mouse and button presses trigger methods in this class, 
/// and the class then communicates with the PlayerBehavior classes.
/// Moving foreward I want this class to be the main connection between player input and the actor's behavior tree
/// </summary>
public class PlayerController : MonoBehaviour {
	
    public delegate void OnFocusChanged(InteractableObject newFocus);
    public OnFocusChanged onFocusChangedCallback;

    public InteractableObject focus;  	// Our current focus: Item, Enemy etc.

    public LayerMask interactionMask;   // Everything we can interact with

    Camera cam;             			// Reference to our camera

	PlayerBehavior playerBehavior;		//reference to our behavior tree

	bool isExploring = false;

    
    void Start()
    {
		// Get references
		playerBehavior = GetComponent<PlayerBehavior> ();
        cam = Camera.main;
    }

    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // If we press left mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Shoot out a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If we hit an interaction mask
            if (Physics.Raycast(ray, out hit, 100f, interactionMask))
            {
				//set focus of object
                SetFocus(hit.collider.GetComponent<InteractableObject>());
            }
        }

    }

    // Set our focus to a new focus
    void SetFocus(InteractableObject newFocus)
    {
        if (onFocusChangedCallback != null)
            onFocusChangedCallback.Invoke(newFocus);

        // If our focus has changed
        if (focus != newFocus && focus != null)
        {
            // Let our previous focus know that it's no longer being focused
            focus.OnDefocused();
        }

        // Set our focus to what we hit
        // If it's not an interactable, simply set it to null
        focus = newFocus;

        if (focus != null)
        {
            // Let our focus know that it's being focused
            focus.OnFocused(transform);
        }

		isExploring = false;

    }

	//defoucus for when we start exploring.
	void clearFocus(){
		
		if (focus != null)
		{
			focus.OnDefocused();
		}
	}

	//toggle explore, this method is usually used by buttons
	public void ToggleExplore()
	{
		if (isExploring) {
			playerBehavior.PlayerCommandStop ();
			isExploring = false;
		} else {
			playerBehavior.PlayerCommandExplore ();
			clearFocus ();
			isExploring = true;
		}
	}

}
