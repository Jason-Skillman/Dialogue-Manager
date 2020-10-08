using UnityEditor;
using UnityEngine;

namespace Dialogue {
	public static class GameObjectMenuEditor {

		[MenuItem("GameObject/Dialogue/Dialogue Manager", false, 10)]
		static void CreatePrefabDialogueManager(MenuCommand menuCommand) {
			//Check if the manager has already been created
			DialogueManager existingManager = GameObject.FindObjectOfType<DialogueManager>();

			if(existingManager != null) {
				Debug.LogWarning("Dialogue manager has already been created.");
				Selection.activeObject = existingManager;
				return;
			}

			//Use the asset database to fetch the console prefab
			GameObject managerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
				"Packages/com.jasonskillman.dialoguemanager/Runtime/Prefabs/DialogueManager.prefab");

			//Instantiate the prefab in the hierarchy
			PrefabUtility.InstantiatePrefab(managerPrefab);

			Selection.activeObject = managerPrefab;
		}

		[MenuItem("GameObject/Dialogue/Dialogue Script", false, 10)]
		static void CreatePrefabDialogueScript(MenuCommand menuCommand) {
			//Create an empty game object
			GameObject go = new GameObject("DialogueScript");
			go.AddComponent<DialogueScript>();

			//Ensure it gets reparented if this was a context click (otherwise does nothing)
			GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

			//Register the creation in the undo system
			Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
			Selection.activeObject = go;
		}

	}
}
