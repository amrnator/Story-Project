using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLog : MonoBehaviour {

	public static GameLog Instance { get; private set;}

	public GameObject logTextPrefab;

	void Awake () {
		Instance = this;
	}
	
	public void post(string log, Color textColor){

		GameObject instancedText = Instantiate (logTextPrefab, this.transform);

		instancedText.GetComponent<TextMeshProUGUI> ().text = log;
		instancedText.GetComponent<TextMeshProUGUI> ().color = textColor;

	}

}
