using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TreeSharpPlus;

/// <summary>
/// Defines the set of actions and behavior the player can take, chosen by the Behavior tree
/// </summary>
public class PlayerActions : MonoBehaviour
{

    #region Members

    public float proximityThreshold;

	public float wanderRadius;

	public PlayerNavigation playerNav;

    NavMeshAgent navAgent;

	public bool isMoving = false;
    
	public Transform target;

	public Vector3 selectedNode;

	public bool mapExplored = false;

	PlayerUI UI;

    [HideInInspector]
    public bool pathAvailable;
    public NavMeshPath navMeshPath;

    #endregion

    #region Unity Methods

    void Start() {
        navAgent = GetComponent<NavMeshAgent>();

		playerNav = GetComponent<PlayerNavigation>();

		UI = GameObject.FindGameObjectWithTag ("UI").GetComponent<PlayerUI> ();

        navMeshPath = new NavMeshPath();

        navAgent.stoppingDistance = proximityThreshold;
    }

    void Update()
    {
        isCloseToSelected();

        if (target != null)
        {
            //not exploring
            MoveToPoint(target.position);
            navAgent.isStopped = false;
            
            isMoving = true;
            FaceTarget();
        }

    }
    #endregion

    #region Public Methods

    public void MoveToPoint(Vector3 point)
    {
        navAgent.SetDestination(point);
    }

    //basic movement command
    public void Goto(Vector3 t) {

		UI.removeInteractions ();

        //check if path is valid
        if (CalculateNewPath(t))
        {
            print("path validated: " + t) ;
            selectedNode = t;

            navAgent.stoppingDistance = proximityThreshold;

            navAgent.SetDestination(t);
            navAgent.isStopped = false;
            isMoving = true;
            
        }
        else {
            print("stopped bc of bad path" + t);
        }

		
    }

    //check this agent's proximity to a position
    public bool isCloseTo(Vector3 t)
    {

        float distance = Vector3.Distance(transform.position, t);

        if (distance < navAgent.stoppingDistance)
        {
            return true;
        }

        return false;
    }

    public bool isCloseToSelected() {

		if (target == null) {

			float distance = Vector3.Distance (transform.position, selectedNode);

			if (distance <= navAgent.stoppingDistance) {

                isMoving = false;

				return true;
			}
		}

		return false;
	}

    public void FollowTarget(InteractableObject newTarget) {

        navAgent.stoppingDistance = newTarget.radius  * .8f;

        target = newTarget.transform;
    }

    public void StopFollow() {

        navAgent.stoppingDistance = 0f;
        navAgent.updateRotation = true;

        isMoving = false;

        navAgent.isStopped = true;

        target = null;
    }

    //basic debug mesasge
    public void Debug(string msg) {
        UnityEngine.Debug.Log(msg);
    }

    //check if path is valid
    bool CalculateNewPath(Vector3 node)
    {
        if (selectedNode != null) {

            navAgent.CalculatePath(node, navMeshPath);

            if (navMeshPath.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }
            else {
                return true;
            }
        }

        return false;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    #endregion
}
