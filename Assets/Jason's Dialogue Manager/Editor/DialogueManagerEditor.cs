using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueManager {

	[CustomEditor(typeof(DialogueManager), true)]
	public class DialogueManagerEditor : Editor {

		bool foldout = false;

		public override void OnInspectorGUI() {
			DialogueManager myTarget = (DialogueManager)target;

			//base.OnInspectorGUI();

			GUILayout.Space(5);
			EditorGUILayout.LabelField("DialogueManager", EditorStyles.boldLabel);
			GUILayout.Space(5);

			myTarget.defaultPortrait = (Sprite)EditorGUILayout.ObjectField("Default Portrait", myTarget.defaultPortrait, typeof(Sprite), true);

			myTarget.typeSpeed = (TypeSpeed)EditorGUILayout.EnumPopup("Type Speed", myTarget.typeSpeed);

			//All default type speeds
			foldout = EditorGUILayout.Foldout(foldout, "Default Typing Speeds");
			if(foldout) {
				var level = EditorGUI.indentLevel;
				EditorGUI.indentLevel++;

				//Fields
				myTarget.typeSpeedFast = EditorGUILayout.FloatField(new GUIContent("Fast", "Tooltip"), myTarget.typeSpeedFast);
				myTarget.typeSpeedMedium = EditorGUILayout.FloatField(new GUIContent("Medium", "Tooltip"), myTarget.typeSpeedMedium);
				myTarget.typeSpeedSlow = EditorGUILayout.FloatField(new GUIContent("Slow", "Tooltip"), myTarget.typeSpeedSlow);
				//Fields

				EditorGUI.indentLevel = level;
			}
			GUILayout.Space(10);
			

			//Componets
			myTarget.dialogueNameText = (Text)EditorGUILayout.ObjectField("Name", myTarget.dialogueNameText, typeof(Text), true);
			myTarget.dialogueSentenceText = (Text)EditorGUILayout.ObjectField("Sentence", myTarget.dialogueSentenceText, typeof(Text), true);
			myTarget.imageNPSIcon = (Image)EditorGUILayout.ObjectField("Image", myTarget.imageNPSIcon, typeof(Image), true);
			myTarget.audioTyping = (AudioSource)EditorGUILayout.ObjectField("Audio Typing", myTarget.audioTyping, typeof(AudioSource), true);
			myTarget.audioEndOfTyping = (AudioSource)EditorGUILayout.ObjectField("Audio End of Typing", myTarget.audioEndOfTyping, typeof(AudioSource), true);
			GUILayout.Space(10);

			//Option Buttons
			myTarget.optionBtn1 = (GameObject)EditorGUILayout.ObjectField("Option Btn 1", myTarget.optionBtn1, typeof(GameObject), true);
			myTarget.optionBtn2 = (GameObject)EditorGUILayout.ObjectField("Option Btn 2", myTarget.optionBtn2, typeof(GameObject), true);
			myTarget.optionBtn3 = (GameObject)EditorGUILayout.ObjectField("Option Btn 3", myTarget.optionBtn3, typeof(GameObject), true);
			myTarget.optionBtn4 = (GameObject)EditorGUILayout.ObjectField("Option Btn 4", myTarget.optionBtn4, typeof(GameObject), true);
			myTarget.clickAreaField = (GameObject)EditorGUILayout.ObjectField("Click Area Field", myTarget.clickAreaField, typeof(GameObject), true);
			GUILayout.Space(10);

			//Readonly variables
			EditorGUILayout.LabelField("Is Dialogue Playing: " + myTarget.IsDialoguePlaying);
			GUILayout.Space(10);


			EditorUtility.SetDirty(myTarget);
		}
	} 

}