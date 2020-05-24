using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogue.DialogueManager {

	public class DialogueManager : MonoBehaviour {

		public Sprite defaultPortrait;

		//Type speed
		public TypeSpeed typeSpeed = TypeSpeed.Fast;
		public float typeSpeedFast = 0.01f;
		public float typeSpeedMedium = 0.05f;
		public float typeSpeedSlow = 0.1f;

		//References
		public Text dialogueNameText;
		public Text dialogueSentenceText;
		public Image imagePortrait;
		//Todo: remove
		public GameObject clickAreaField;
		public AudioSource audioTyping, audioEndOfTyping;

		//Option Buttons
		public GameObject optionBtn1, optionBtn2, optionBtn3, optionBtn4;

		private bool removeButtons;
		private EventSystem eventSystem;
		private Animator animator;
		private string currentSentence;
		private Coroutine coroutineTyping, coroutineFadeOutAudio;

		//private Queue<string> queue;
		//private List<DialogueButton[]> optionBtnList = new List<DialogueButton[]>();
		//private DialogueGroup currentDialogueGroup;
		//private DialogueSpeaker currentSpeaker;

		//private int indexSpeaker, indexSentence;
		
		private DialogueSpeaker currentDialogueSpeaker;
		private Queue<DialogueData> queue = new Queue<DialogueData>();

		private event Action OnFinish;

		public static DialogueManager Instance { get; private set; }

		public bool IsDialoguePlaying { get; private set; }
		
		private bool IsTyping { get; set; }


		private void Awake() {
			DontDestroyOnLoad(gameObject);
			if(Instance == null) Instance = this;
			else Destroy(gameObject);

			animator = GetComponent<Animator>();

			//Todo: remove event system reference
			eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		}

		///<summary>
		/// Main method for adding new dialogue to the queue
		///</summary>
		public void AddDialogue(DialogueGroup dialogueGroup) {
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

			coroutineTyping = StartCoroutine(TypeSentence(sentence));



			//Todo:
			//Remove the buttons if the previous frame had them
			/*if(removeButtons) {
				removeButtons = false;

				HideAllOptionButtons();
			}*/

			//Todo:
			//If the current dialogue frame has a button option layout
			/*if(queueList.Peek().dialogueButton.Length > 0) {

				for(int i = 0; i < optionBtnList[0].Length; i++) {
					if(i == 0) {
						optionBtn1.SetActive(true);

						eventSystem.SetSelectedGameObject(optionBtn1);

						optionBtn1.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn1.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn1.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][0].dialogueTrigger.TriggerDialogue();
							DisplayNextSentence();
						});
					} else if(i == 1) {
						optionBtn2.SetActive(true);
						optionBtn2.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn2.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn2.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][1].dialogueTrigger.TriggerDialogue();
							DisplayNextSentence();
						});
					} else if(i == 2) {
						optionBtn3.SetActive(true);
						optionBtn3.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn3.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn3.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][2].dialogueTrigger.TriggerDialogue();
							DisplayNextSentence();
						});
					} else if(i == 3) {
						optionBtn4.SetActive(true);
						optionBtn4.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn4.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn4.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][3].dialogueTrigger.TriggerDialogue();
							DisplayNextSentence();
						});
					}
				}

				removeButtons = true;
				clickAreaField.SetActive(false);
			}*/
			//If the current frame does not have a button option layout reset the buttons and frame
			/*else {
				HideAllOptionButtons();
				clickAreaField.SetActive(true);
				eventSystem.SetSelectedGameObject(clickAreaField);
			}*/

			//Setup the name
			/*dialogueNameText.text = currentDialogue.name;

			//Setup the sprite
			if(currentDialogue.sprite != null)
				imagePortrait.sprite = currentDialogue.sprite;
			else
				imagePortrait.sprite = defaultPortrait;*/

			//Todo:
			//Setup the sentence
			/*string sentence = currentDialoug.sentences;

			//Remove items from the queue after done using them
			queue.RemoveAt(0);*/


			/*IsDialoguePlaying = true;
			if(audioTyping) audioTyping.Play();

			//Start typing out sentance
			StopAllCoroutines();
			*/
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

	}

	public enum TypeSpeed {
		Fast,
		Medium,
		Slow
	}

}