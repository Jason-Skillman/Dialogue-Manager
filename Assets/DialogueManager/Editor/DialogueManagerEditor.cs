using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.DialogueManager {

	[CustomEditor(typeof(DialogueManager), true)]
	public class DialogueManagerEditor : Editor {

		private bool foldoutTypeSpeeds;

		
		public override void OnInspectorGUI() {
			DialogueManager myTarget = (DialogueManager)target;
			serializedObject.Update();
			
			
			EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Is Playing: " + myTarget.IsDialoguePlaying);
			
			
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultPortrait"));
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Typing Speed", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("typeSpeed"));
			
			foldoutTypeSpeeds = EditorGUILayout.Foldout(foldoutTypeSpeeds, "Default Typing Speeds");
			if(foldoutTypeSpeeds) {
				EditorGUI.indentLevel++;
				
				EditorGUILayout.PropertyField(serializedObject.FindProperty("typeSpeedFast"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("typeSpeedMedium"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("typeSpeedSlow"));

				EditorGUI.indentLevel--;
			}
			
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueNameText"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueSentenceText"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("imagePortrait"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("audioTyping"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("audioEndOfTyping"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("clickArea"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("optionBtn1"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("optionBtn2"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("optionBtn3"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("optionBtn4"));
			
			
			serializedObject.ApplyModifiedProperties();
		}
	} 

}