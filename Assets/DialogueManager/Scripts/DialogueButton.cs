using System;

namespace Dialogue.DialogueManager {
	
	[Serializable]
	public struct DialogueButton {
		public string name;
		public DialogueScript dialogueTrigger;
	} 
	
}