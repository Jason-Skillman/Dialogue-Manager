using System;
using UnityEngine;

namespace Dialogue.DialogueManager {

    [Serializable]
    public struct DialogueSpeaker {
		public Sprite sprite;
		public string name;
        public string[] sentences;
    }

}