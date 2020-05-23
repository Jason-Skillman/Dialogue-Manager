using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Dialogue.DialogueManager {
	[CustomEditor(typeof(DialogueListScript))]
	public class DialogueListScriptEditor : Editor {

		private DialogueListScript myTarget;
		private ReorderableList reorderableList;
		private Dictionary<string, ReorderableList> dictionaryInnerList = new Dictionary<string, ReorderableList>();


		public void OnEnable() {
			myTarget = (DialogueListScript) target;

			SerializedProperty dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");

			/*reorderableList = new ReorderableList(serializedObject,
				serializedObject.FindProperty("listDialogue"),
				true, true, true, true);*/
			reorderableList = new ReorderableList(serializedObject,
				dialogueGroupProperty.FindPropertyRelative("dialogueSpeakers"),
				true, true, true, true);
			reorderableList.drawHeaderCallback += OnDrawHeader;
			reorderableList.drawElementCallback += OnDrawElement;
			reorderableList.elementHeightCallback += GetElementHeight;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			reorderableList.DoLayoutList();
			
			//Todo: dia buttons

			serializedObject.ApplyModifiedProperties();
		}

		private void OnDrawHeader(Rect rect) {
			EditorGUI.LabelField(rect, "Dialogue Card");
		}

		private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused) {
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

			rect.y += 2;
			float width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.fieldWidth - 12;

			EditorGUI.PropertyField(new Rect(rect.x, rect.y, width, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("sprite"));

			EditorGUI.PropertyField(new Rect(rect.x, rect.y + 20, width, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("name"));

			
			//Create the inner sentence list
			rect.y += 2;
			SerializedProperty innerListProp = element.FindPropertyRelative("sentences");
			
			//Find the reorderable list in the dictonary else create a new one
			ReorderableList innerReorderableList;
			if(dictionaryInnerList.ContainsKey(element.propertyPath)) {
				innerReorderableList = dictionaryInnerList[element.propertyPath];
			} else {
				//Create reorderable list and store it in dict
				innerReorderableList = new ReorderableList(element.serializedObject, innerListProp,
					true, true, true, true);
				
				innerReorderableList.drawHeaderCallback = (innerRect) => EditorGUI.LabelField(innerRect, "Sentences");
				
				innerReorderableList.drawElementCallback = (innerRect, innerIndex, innerA, innerH) => {
					// Get element of inner list
					SerializedProperty innerElement = innerListProp.GetArrayElementAtIndex(innerIndex);

					float innerLabelWidth = EditorGUIUtility.labelWidth - 90;
					float innerFieldWidth = innerRect.width - innerLabelWidth;
					
					//Draw the label
					EditorGUI.LabelField(new Rect(innerRect.x, innerRect.y, innerLabelWidth, EditorGUIUtility.singleLineHeight),
						"#" + (innerIndex + 1));
					
					//Draw the text area
					innerElement.stringValue = EditorGUI.TextArea(new Rect(innerRect.x + innerLabelWidth, innerRect.y + 2, innerFieldWidth, EditorGUIUtility.singleLineHeight * 3),
						innerElement.stringValue);
				};

				innerReorderableList.elementHeightCallback = (innerHeightIndex) => {
					return EditorGUIUtility.singleLineHeight * 3 + 4;
				};
				
				dictionaryInnerList[element.propertyPath] = innerReorderableList;
			}
			
			//Setup the inner list
			var height = (innerListProp.arraySize + 3) * EditorGUIUtility.singleLineHeight;
			innerReorderableList.DoList(new Rect(rect.x, rect.y + 40, rect.width, height));


			//Debug
			//EditorGUILayout.PropertyField(element.FindPropertyRelative("sentences"));
		}

		private float GetElementHeight(int index) {
			SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			
			//The height of one card
			float cardHeight = EditorGUIUtility.singleLineHeight * 3 + 4;
			//Default value when no cards exist
			float extraInnerHeight = 20;
			//Find the amount of cards and get the final height value
			if(dictionaryInnerList.ContainsKey(element.propertyPath))
				if(dictionaryInnerList[element.propertyPath].count > 0)
					extraInnerHeight = dictionaryInnerList[element.propertyPath].count * cardHeight;
			
			return EditorGUIUtility.singleLineHeight * 5 + 10 + extraInnerHeight;
		}

	}
	
}