using System;
using UnityEngine.Events;

namespace Dialogue {
	[Serializable]
	public struct DialogueButton {
		public string name;
		public DialogueScript dialogueTrigger;
		public UnityEvent onClick;
	} 
}
