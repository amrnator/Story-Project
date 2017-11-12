using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DoorController : MonoBehaviour {

    public void openDoor() {
		GameLog.Instance.post("The Door has opened", Color.gray);

        transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
    }



}


