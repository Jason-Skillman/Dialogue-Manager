using UnityEngine;

namespace DialogueManager {
    public class DialogueScript : MonoBehaviour {

        public Dialogue[] dialogue;
        public ButtonOption[] buttonOptions;


		///<summary>Main method to call when you whant to trigger this dialouge</summary>
		public void TriggerDialogue() {
            DialogueManager.main.AddNewDialogue(dialogue, buttonOptions);
        }

		///<summary>Main method to call when you whant to trigger this dialouge with a callback</summary>
		///<param name="callbackFinished">Called back when the dialoue has finished</param>
		public void TriggerDialogue(Callback callbackFinished) {
			DialogueManager.main.AddNewDialogue(dialogue, buttonOptions, callbackFinished);
		}

	}

    [System.Serializable]
    public struct ButtonOption {
        public string name;
        public DialogueScript trigger;
    } 
}