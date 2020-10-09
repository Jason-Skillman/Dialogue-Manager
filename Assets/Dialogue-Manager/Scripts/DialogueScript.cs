using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {
	public class DialogueScript : MonoBehaviour {

		public DialogueGroup dialogueGroup;
		
		public void TriggerDialogue() {
			DialogueManager.Instance.AddDialogue(dialogueGroup);
		}

	}
}
