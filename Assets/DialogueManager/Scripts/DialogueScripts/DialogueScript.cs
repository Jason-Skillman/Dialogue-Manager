using UnityEngine;

namespace Dialogue.DialogueManager {
    public class DialogueScript : MonoBehaviour {

        public DialogueSpeaker[] dialogueSpeaker;
        public DialogueButton[] dialogueButton;


		///<summary>Main method to call when you whant to trigger this dialouge</summary>
		public void TriggerDialogue() {
            //DialogueManager.Instance.AddDialogue(dialogueSpeaker, dialogueButton);
        }

		///<summary>Main method to call when you whant to trigger this dialouge with a callback</summary>
		///<param name="callbackFinished">Called back when the dialoue has finished</param>
		/*public void TriggerDialogue(Callback callbackFinished) {
			DialogueManager.Instance.AddDialogue(dialogueSpeaker, dialogueButton, callbackFinished);
		}*/

	}

    
}