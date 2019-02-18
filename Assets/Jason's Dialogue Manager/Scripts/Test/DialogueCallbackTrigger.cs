using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManager;

public class DialogueCallbackTrigger : MonoBehaviour {

	public DialogueScript dialogue;

	private Callback callbackFinished;
	

	public void TriggerDialogue() {
		callbackFinished = OnDialogueFinish;
		dialogue.TriggerDialogue(callbackFinished);
	}

	private void OnDialogueFinish() {
		Debug.Log("The dialogue has ended.");
	}

}
