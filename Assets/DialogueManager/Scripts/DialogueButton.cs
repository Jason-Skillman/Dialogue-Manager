using System;
using UnityEngine.Events;

namespace Dialogue.DialogueManager {
	
	[Serializable]
	public struct DialogueButton {
		public string name;
		public DialogueScript dialogueTrigger;
		public UnityEvent onClick;
	} 
	
}