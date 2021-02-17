using UnityEngine;

namespace Dialogue {
	[CreateAssetMenu(menuName = "Dialogue/Dialogue Script", fileName = "New Dialogue Script", order = 1)]
	public class DialogueScript : ScriptableObject {

		public DialogueGroup dialogueGroup;
		
		public void TriggerDialogue() {
			DialogueManager.Instance.AddDialogue(dialogueGroup);
		}

	}
}
