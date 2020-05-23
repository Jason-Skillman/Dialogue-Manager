using System;
using UnityEngine;

namespace Dialogue.DialogueManager {

    [Serializable]
    public class Dialogue {
		public Sprite spriteIcon;

		public string name;

        //[TextArea(3, 10)]
        public string[] sentences;
    }

    [Obsolete]
	[Serializable]
	public class DialogueSingle {
		public string name;
		public string sentence;
	}

}