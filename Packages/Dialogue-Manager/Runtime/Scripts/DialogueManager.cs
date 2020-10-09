using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogue {
	public class DialogueManager : MonoBehaviour {

		public Sprite defaultPortrait;

		//Type speed
		public TypeSpeed typeSpeed = TypeSpeed.Fast;
		public float typeSpeedFast = 0.01f;
		public float typeSpeedMedium = 0.05f;
		public float typeSpeedSlow = 0.1f;

		//References
		public TMP_Text dialogueNameText, dialogueSentenceText;
		public Image imagePortrait;
		public AudioSource audioTyping, audioEndOfTyping;
		public GameObject clickArea, optionBtn1, optionBtn2, optionBtn3, optionBtn4;

		private Animator animator;
		private EventSystem eventSystem;
		
		private Queue<DialogueData> queue = new Queue<DialogueData>();
		private DialogueGroup currentGroup;
		private DialogueSpeaker currentDialogueSpeaker;
		private Coroutine coroutineTyping, coroutineFadeOutAudio;
		private bool hasButtons;
		private string currentSentence;

		private event Action OnFinish;

		public static DialogueManager Instance { get; private set; }

		public bool IsDialoguePlaying { get; private set; }
		
		private bool IsTyping { get; set; }

		private void Awake() {
			DontDestroyOnLoad(gameObject);
			if(Instance == null) Instance = this;
			else Destroy(gameObject);

			animator = GetComponent<Animator>();
		}

		private void Update() {
			if(eventSystem == null) {
				eventSystem = FindObjectOfType<EventSystem>();
				return;
			}
		}

		///<summary>
		/// Main method for adding new dialogue to the queue
		///</summary>
		public void AddDialogue(DialogueGroup dialogueGroup) {
			currentGroup = dialogueGroup;
			hasButtons = false;
			HideAllOptionButtons();
			
			//Loop through all of the speakers and setup the queue
			foreach(DialogueSpeaker speaker in dialogueGroup.dialogueSpeakers) {
				Sprite sprite = speaker.sprite;
				string name = speaker.name;
				
				//Loop through all of the sentences
				foreach(string sentence in speaker.sentences) {
					DialogueData data = new DialogueData();
					data.sprite = sprite;
					data.name = name;
					data.sentence = sentence;
					queue.Enqueue(data);
				}
			}
			
			//Startup for the first time
			if(IsDialoguePlaying == false) {
				IsDialoguePlaying = true;
				animator.SetBool("IsOpen", true);
				
				NextSentence();
			}
		}

		///<summary>
		/// Main method to call when to start the next sentence
		///</summary>
		public void NextSentence() {
			//Has the dialogue ended?
			if(queue.Count <= 0) {
				EndDialogue();
				return;
			}
			
			//Focus on the clickArea
			if(eventSystem)
				eventSystem.SetSelectedGameObject(clickArea);

			//Fetch the next data block
			DialogueData data = queue.Dequeue();
			
			//Setup the name
			dialogueNameText.text = data.name;

			//Setup the sprite
			if(data.sprite != null)
				imagePortrait.sprite = data.sprite;
			else
				imagePortrait.sprite = defaultPortrait;

			//Setup the sentence
			string sentence = data.sentence;
			
			//Setup the buttons
			//Is this the last sentence
			if(queue.Count <= 0) {
				//Is there buttons to setup
				if(currentGroup.dialogueButtons != null && currentGroup.dialogueButtons.Count > 0) {
					hasButtons = true;
					SetupButtons(currentGroup.dialogueButtons);
				}
			}
			
			coroutineTyping = StartCoroutine(TypeSentence(sentence));
		}

		///<summary>
		/// Called internally when the dialogue box has finished
		///</summary>
		private void EndDialogue() {
			//Close the box
			animator.SetBool("IsOpen", false);

			OnFinish?.Invoke();
			OnFinish = null;
		}

		///<summary>
		/// Called externally by the animator controller when the dialogue box has finished its close animation
		///</summary>
		public void OnDialogueBoxClose() {
			IsDialoguePlaying = false;
		}

		///<summary>
		/// Called externally by the area box button
		///</summary>
		public void OnAreaBoxSubmit() {
			if(IsTyping) {
				StopCoroutine(coroutineTyping);
				dialogueSentenceText.text = currentSentence;
				FinishedTypingSentence();
			} else if(hasButtons) {
				//Prevent input if buttons are showing
			} else {
				NextSentence();
			}
		}

		private IEnumerator TypeSentence(string sentence) {
			IsTyping = true;
			currentSentence = sentence;
			dialogueSentenceText.text = "";
			if(audioTyping) audioTyping.Play();

			float typeSpeedDelay = typeSpeedFast;
			switch(typeSpeed) {
				case TypeSpeed.Fast:
					typeSpeedDelay = typeSpeedFast;
					break;
				case TypeSpeed.Medium:
					typeSpeedDelay = typeSpeedMedium;
					break;
				case TypeSpeed.Slow:
					typeSpeedDelay = typeSpeedSlow;
					break;
			}

			foreach(char letter in sentence.ToCharArray()) {
				dialogueSentenceText.text += letter;
				yield return new WaitForSeconds(typeSpeedDelay);
			}

			FinishedTypingSentence();
		}
		
		///<summary>
		/// Called internally when the dialogue has finished typing out the sentence
		/// </summary>
		private void FinishedTypingSentence() {
			IsTyping = false;
			
			//Fades out the audio
			coroutineFadeOutAudio = StartCoroutine(FadeOutAudio(audioTyping, 0.5f));

			//Start the EndOfTyping Audio
			audioEndOfTyping.Play();
		}

		private IEnumerator FadeOutAudio(AudioSource audioSource, float FadeTime) {
			if(!audioSource) StopCoroutine(coroutineFadeOutAudio);
			audioSource.volume = 1; //1 is Max volume
			while(audioSource.volume > 0.05f) {
				yield return null;

				audioSource.volume -= 1 * Time.deltaTime / FadeTime;
			}

			audioSource.Stop();
			audioSource.volume = 1;
		}

		private void SetupButtons(List<DialogueButton> buttons) {
			for(int i = 0; i < buttons.Count; i++) {
				switch(i) {
					case 0:
						optionBtn1.SetActive(true);

						if(eventSystem)
							eventSystem.SetSelectedGameObject(optionBtn1);

						optionBtn1.GetComponentInChildren<TMP_Text>().text = buttons[i].name;
						optionBtn1.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn1.GetComponent<Button>().onClick.AddListener(delegate {
							DialogueScript script = buttons[0].dialogueTrigger;
							if(script) script.TriggerDialogue();
							NextSentence();
							
							buttons[0].onClick?.Invoke();
						});
						break;
					case 1:
						optionBtn2.SetActive(true);
						optionBtn2.GetComponentInChildren<TMP_Text>().text = buttons[i].name;
						optionBtn2.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn2.GetComponent<Button>().onClick.AddListener(delegate {
							DialogueScript script = buttons[1].dialogueTrigger;
							if(script) script.TriggerDialogue();
							NextSentence();
							
							buttons[1].onClick?.Invoke();
						});
						break;
					case 2:
						optionBtn3.SetActive(true);
						optionBtn3.GetComponentInChildren<TMP_Text>().text = buttons[i].name;
						optionBtn3.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn3.GetComponent<Button>().onClick.AddListener(delegate {
							DialogueScript script = buttons[2].dialogueTrigger;
							if(script) script.TriggerDialogue();
							NextSentence();
							
							buttons[2].onClick?.Invoke();
						});
						break;
					case 3:
						optionBtn4.SetActive(true);
						optionBtn4.GetComponentInChildren<TMP_Text>().text = buttons[i].name;
						optionBtn4.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn4.GetComponent<Button>().onClick.AddListener(delegate {
							DialogueScript script = buttons[3].dialogueTrigger;
							if(script) script.TriggerDialogue();
							NextSentence();
							
							buttons[3].onClick?.Invoke();
						});
						break;
					default:
						throw new TooManyDialogueButtonsException();
				}
			}
		}

		private void HideAllOptionButtons() {
			optionBtn1.SetActive(false);
			optionBtn2.SetActive(false);
			optionBtn3.SetActive(false);
			optionBtn4.SetActive(false);
		}
		
		[Serializable]
		private struct DialogueData {
			public Sprite sprite;
			public string name;
			public string sentence;
		}

		private class TooManyDialogueButtonsException : Exception { }

	}

	public enum TypeSpeed {
		Fast,
		Medium,
		Slow
	}
}
