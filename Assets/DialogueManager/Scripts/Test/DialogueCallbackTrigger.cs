using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue.DialogueManager;

public class DialogueCallbackTrigger : MonoBehaviour {

	public DialogueScript dialogue;

	private Action callbackFinished;
	

	public void TriggerDialogue() {
		callbackFinished = OnDialogueFinish;
		//dialogue.TriggerDialogue(callbackFinished);
	}

	private void OnDialogueFinish() {
		Debug.Log("The dialogue has ended.");
	}

}
