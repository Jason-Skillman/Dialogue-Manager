using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue.DialogueManager {
	public class DialogueListScript : MonoBehaviour {

		//public List<DialogueSpeaker> listDialogue = new List<DialogueSpeaker>();
		public DialogueGroup dialogueGroup;
		
		
		public void TriggerDialogue() {
			//DialogueManager.Instance.AddDialogue(dialogueGroup);
		}

	}

}