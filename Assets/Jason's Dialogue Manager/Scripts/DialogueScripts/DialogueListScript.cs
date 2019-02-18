using System.Collections.Generic;
using UnityEngine;

namespace DialogueManager {
	public class DialogueListScript : MonoBehaviour {

		public List<DialogueSingle> dialogueList = new List<DialogueSingle>();


		///<summary>Main method to call when you whant to trigger this dialouge</summary>
		public void TriggerDialogue() {
			DialogueManager.main.AddNewDialogue(dialogueList);
		}

	}

}