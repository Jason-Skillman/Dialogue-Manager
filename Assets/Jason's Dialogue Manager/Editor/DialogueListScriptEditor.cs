using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DialogueManager {
	[CustomEditor(typeof(DialogueListScript))]
	[CanEditMultipleObjects]
	public class DialogueListScriptEditor : Editor {

		public DialogueListScript main;
		private ReorderableList reorderableList;

		//Constants
		const float k_DefaultElementHeight = 48f;
		const float k_PaddingBetweenRules = 13f;
		const float k_SingleLineHeight = 16f;
		const float k_LabelWidth = 53f;

		const float k_MaxInspectorWidth = 250f;
		const float k_MarginBetweenElements = 20f;


		public void OnEnable() {
			main = (DialogueListScript)target;

			reorderableList = new ReorderableList(main.dialogueList, typeof(DialogueSingle), true, true, true, true);
			reorderableList.drawHeaderCallback += OnDrawHeader;
			reorderableList.drawElementCallback += OnDrawElement;
			reorderableList.elementHeightCallback += GetElementHeight;
			reorderableList.onReorderCallbackWithDetails += ReorderCallbackDelegateWithDetails;
		}

		public override void OnInspectorGUI() {
			//base.OnInspectorGUI();
			EditorGUILayout.Space();

			//EditorGUILayout.LabelField("This is the ScriptableEvent thing", EditorStyles.boldLabel);

			if(reorderableList != null && main.dialogueList != null)
				reorderableList.DoLayoutList(); //Must be called in OnInspectorGUI() method

			EditorUtility.SetDirty(target);
		}

		private void OnDrawHeader(Rect rect) {
			GUI.Label(rect, "Reorderable Dialogue List");
		}
		private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused) {

			DialogueSingle dialogue = main.dialogueList[index];

			float y = rect.yMin + 2;
			float width = rect.width;
			float height = rect.height;


			EditorGUI.BeginChangeCheck();

			Rect newRect = new Rect(rect.xMin, y, width, k_SingleLineHeight);

			//Element 1
			dialogue.name = EditorGUI.TextField(newRect, "Name", dialogue.name);
			y += k_MarginBetweenElements;

			//Element 2
			newRect = new Rect(rect.xMin, y, width, k_SingleLineHeight);
			EditorGUI.LabelField(newRect, "Sentance");

			//Element 3
			int offset = 120;
			newRect = new Rect(rect.xMin + offset, y, width - offset, k_SingleLineHeight * 3);
			dialogue.sentence = EditorGUI.TextArea(newRect, dialogue.sentence);
			y += k_MarginBetweenElements;

			if(EditorGUI.EndChangeCheck())
				SaveTile();
		}
		private float GetElementHeight(int index) {
			return 90;
		}
		private void ReorderCallbackDelegateWithDetails(ReorderableList list, int oldIndex, int newIndex) {
			SaveTile();
		}
		
		private void SaveTile() {
			EditorUtility.SetDirty(target);
			SceneView.RepaintAll();
		}
	}
}