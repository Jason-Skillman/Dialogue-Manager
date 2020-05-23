using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue.DialogueManager {
	public class DialogueListScript : MonoBehaviour {

		[FormerlySerializedAs("dialogueList")]
		public List<Dialogue> listDialogue = new List<Dialogue>();


		///<summary>
		/// Main method to call when you whant to trigger this dialouge
		/// </summary>
		public void TriggerDialogue() {
			DialogueManager.main.AddNewDialogue(listDialogue.ToArray());
		}

	}

}