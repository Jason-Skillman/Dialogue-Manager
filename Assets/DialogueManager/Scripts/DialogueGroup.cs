using System;
using System.Collections.Generic;

namespace Dialogue.DialogueManager {
	
	[Serializable]
	public struct DialogueGroup {
		public List<DialogueSpeaker> dialogueSpeakers;
		public List<DialogueButton> dialogueButtons;
	}
	
}