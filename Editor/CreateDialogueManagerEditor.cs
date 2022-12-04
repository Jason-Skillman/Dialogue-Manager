namespace JasonSkillman.Dialogue.Editor {
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.EventSystems;
	
	public static class CreateDialogueManagerEditor {

		[MenuItem("GameObject/Dialogue/Dialogue Manager", false, 10)]
		static void CreateDialogueManager(MenuCommand menuCommand) {
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
			
			
			//Instantiate an EventSystem if one does not exist
			GameObject eventSystem = GameObject.Find("EventSystem");
			if(eventSystem != null) return;
        
			eventSystem = new GameObject("EventSystem");
			eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();
		}
		
	}
}
