using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogText : MonoBehaviour {

	TextMeshProUGUI text;

	public float timeUntilFade;

	void Awake(){
		text = this.GetComponent<TextMeshProUGUI> ();

	}
		
	void Start () {
		StartCoroutine ("fadeAndDie");
	}
	
	IEnumerator fadeAndDie(){

		//wait before starting fade
		yield return new WaitForSeconds (timeUntilFade);

		//start fade
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = text.color;
			c.a = f;
			text.color = c;
			yield return null;
		}

		//destroy self
		Destroy(this.gameObject);
	}
}
