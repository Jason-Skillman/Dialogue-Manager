namespace JasonSkillman.Dialogue {
	using System;
	using UnityEngine.Events;
	
	[Serializable]
	public struct DialogueButton {
		public string name;
		public DialogueScript dialogueTrigger;
		public UnityEvent onClick;
	} 
}
