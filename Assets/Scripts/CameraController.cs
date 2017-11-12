using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speed;

	Vector3 vectorBefore;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        var z = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var x = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		vectorBefore = new Vector3 (z, x, 0);

		var vector = Quaternion.Euler (30, 45, 0) * vectorBefore;

        transform.Rotate(0, 0, 0);
		transform.Translate(vector);
		//transform.Translate(new Vector3(z,0,x).
    }
}
