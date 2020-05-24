using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue.DialogueManager {
	public class DialogueListScript : MonoBehaviour {

		public DialogueGroup dialogueGroup;
		
		
		public void TriggerDialogue() {
			DialogueManager.Instance.AddDialogue(dialogueGroup);
		}

	}

}