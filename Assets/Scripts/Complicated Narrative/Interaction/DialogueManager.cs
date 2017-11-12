using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

	public Queue<string> sentences;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI dialogueText;

	public Animator animator;

	public Animator interactionAnimator;

	public Animator gameLogAnimator;

	void Start () {
		sentences = new Queue<string> ();
	}

	public void StartDialogue(Dialogue dialog)
	{
		//open and close appropriate UI
		animator.SetBool ("isOpen", true);
		interactionAnimator.SetBool ("isOpen", false);
		gameLogAnimator.SetBool ("isOpen", false);

		print ("Starting Convo with: " + dialog.name);

		nameText.text = dialog.name;

		sentences.Clear();

		for(int i = 0; i < dialog.sentences.Length; i++) {
			sentences.Enqueue (dialog.sentences[i]);
		}

		displayNextSentence();
	}

	public void displayNextSentence(){

		if (sentences.Count == 0) {
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue ();

		StopAllCoroutines ();

		StartCoroutine (typeSentence(sentence));
	}

	IEnumerator typeSentence(string sentence){
		dialogueText.text = "";

		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;

			//wait two frames
			yield return null;
			yield return null;
		}
	}


	void EndDialogue(){
		//open and close appropriate UI elements
		animator.SetBool ("isOpen", false);
		interactionAnimator.SetBool ("isOpen", true);
		gameLogAnimator.SetBool ("isOpen", true);

		print ("Convo over");
	}
}
