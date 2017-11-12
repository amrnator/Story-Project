using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using System;

/// <summary>
/// Behavior tree for player agent, while user can set certain actions for the player to take
/// </summary>
public class PlayerBehavior : MonoBehaviour , IHasBehaviorObject{

    #region Members

    public PlayerActions playerActions;

    public PlayerNavigation playerNav;

    BehaviorObject g_BehaviorObject;

    public BehaviorObject Behavior
    {
        get
        {
            return g_BehaviorObject;
        }
    }


    public bool InEvent
    {
        get
        {
            return g_BehaviorObject.Status != BehaviorStatus.Idle &&
                g_BehaviorObject.CurrentEvent != null;
        }
    }

    #endregion

    void Start() {
        g_BehaviorObject = new BehaviorObject();
    }

    public void PlayerCommandExplore() {
        print("Actor is exploring");
		GameLog.Instance.post ("Actor begins to explore", Color.cyan);
        playerActions.StopFollow();
        StartEvent(Explore());
    }

    public void PlayerCommandStop() {
        print("Actor has stopped");
		GameLog.Instance.post ("Actor stops in his tracks", Color.cyan);
        StartEvent(PlayerBehavior_StopFollow());
    }

    public void PlayerCommandInteract(InteractableObject interact) {
        print("Actor is investigating");
        StartEvent(Interact(interact));
    }

    public void StartEvent(Node behavior)
    {
        BehaviorEvent ev = new BehaviorEvent(doEvent => behavior, new IHasBehaviorObject[] { this });
        ev.StartEvent(InEvent ? g_BehaviorObject.CurrentPriority + 1f : 1f);
    }

    public Node Explore() {
		return new DecoratorLoop (
			new Sequence(
				//PlayerBehavior_StopFollow(),
                new LeafWait(1000),
				PlayerBehavior_Explore()
			)
		);
    }

    public Node Interact(InteractableObject interact)
    {

        return new Sequence(
            new LeafWait(1000),
            PlayerBehavior_Interact(interact)
            );
        
    }

    public Node PlayerBehavior_GoTo(Transform t, bool run = false)
    {
		return PlayerBehavior_GoTo(t.position, run);
    }


    public Node PlayerBehavior_GoTo(Vector3 t, bool run = false)
    {
        return new Sequence(
            //new LeafTrace("Started"),
            PlayerBehavior_StopFollow(),
			new LeafInvoke(() => Behavior_GoTo(t))
        );
    }


    public Node PlayerBehavior_StopFollow()
    {
        return new Sequence(
            new LeafInvoke(() => {
                playerActions.StopFollow();
                return RunStatus.Success;
            }
            )
        );

    }

	public Node PlayerBehavior_Explore()
    {
		print("behavior");
		return new Sequence(
				new LeafInvoke(() => this.Behavior_Wander(playerNav.navNodes)
	            )
	        );
    }

    public Node PlayerBehavior_Interact(InteractableObject interact)
    {
        return new Sequence(
                new LeafInvoke(() => Behavior_Interact(interact)
                )
            );

    }

    private RunStatus Behavior_Interact(InteractableObject interact) {
		print("Interacting");
		GameLog.Instance.post ("The hero finds something", Color.cyan);
        playerActions.FollowTarget(interact);
        //interact.activate();
        return RunStatus.Success;
    }

    private RunStatus Behavior_GoTo(Vector3 t)
    {
		Val<Vector3> position = Val.V(() => t);
		position.UseCache = true;
		position.Fetch ();

		if (playerActions.isCloseTo(position.Value))
        {
            playerActions.Debug("Finished go to");
            return RunStatus.Success;
        }
        else {
            try
            {
                //go to location
				playerActions.Goto(position.Value);
                //Debug.Log("Running");
                return RunStatus.Running;
            }
            catch (System.Exception e)
            {
                // this will occur if the target is unreacheable
                //Debug.Log("Failed");
                return RunStatus.Failure;
            }
        }
    }

	private RunStatus Behavior_Wander(List<GameObject> points) {

		if (!playerActions.isMoving) {
			try {
				print("finding new point");
				Transform t = points[(int)(UnityEngine.Random.value * (points.Count))].GetComponent<Transform>();

				Vector3 targetPosition = t.position;
				float rand = UnityEngine.Random.value;

				if (rand < 0.33f) {
					targetPosition -= transform.right * playerActions.wanderRadius;
				} else if (rand < 0.66f) {
					targetPosition -= transform.forward * playerActions.wanderRadius;
				} else {
					targetPosition += transform.right * playerActions.wanderRadius;
				}

				if (rand < .65f && Mathf.Abs(targetPosition.y - transform.position.y) > 1.5f)
					targetPosition = transform.position;


				playerActions.Goto(targetPosition);

				return RunStatus.Running;

			} catch (System.Exception e) {
				playerActions.Debug(e.Message);
				return RunStatus.Failure;
			}
		} else {print("not finding point");return RunStatus.Success; }
	}
}
