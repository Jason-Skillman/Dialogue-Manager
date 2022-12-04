namespace JasonSkillman.Dialogue {
	using System;
	using System.Collections.Generic;
	
	[Serializable]
	public struct DialogueGroup {
		public List<DialogueSpeaker> dialogueSpeakers;
		public List<DialogueButton> dialogueButtons;
	}
}
