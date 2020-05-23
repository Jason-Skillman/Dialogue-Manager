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
		public AudioSource audioTyping,audioEndOfTyping;

		//Option Buttons
		public GameObject optionBtn1, optionBtn2, optionBtn3, optionBtn4;
		
		private bool removeButtons;
		private EventSystem eventSystem;
		private Animator animator;
		private Coroutine coroutineFadeOutAudio;

		private Queue<DialogueSpeaker> queue;
		//private List<DialogueButton[]> optionBtnList = new List<DialogueButton[]>();

		private event Action OnFinish;
		
		public static DialogueManager Instance { get; private set; }
		
		public bool IsDialoguePlaying { get; private set; }

		
		void Awake() {
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
		public void AddDialogue(List<DialogueSpeaker> dialogueList, Action onFinish = null) {
			//Open the box
			animator.SetBool("IsOpen", true);

			//List<Holder> tempList = new List<Holder>();

			//Add the callback delegate to the callback list
			OnFinish += onFinish;

			//Loop through dialogue amount
			/*for(int i = 0; i < dialogueList.Count; i++) {
				//Loop through sentances amount
				foreach(string sentence in dialogueList[i].sentences) {
					Holder holder = new Holder();
					holder.name = dialogueList[i].name;
					holder.sentences = sentence;
					holder.btnOn = false;

					tempList.Add(holder);
				}
			}*/

			//Check if their is a button option and add it to last item
			/*if(btn != null && btn.Length != 0) {
				tempList[tempList.Count - 1].btnOn = true;

				optionBtnList.Add(btn);
			}*/

			//Add all created lists to master list
			/*foreach(Holder hold in tempList) {
				queueList.Add(hold);
			}*/
			
			queue = new Queue<DialogueSpeaker>(dialogueList);

			//Startup for the first time
			if(IsDialoguePlaying == false) {
				DisplayNextSentence();
			}
		}

		///<summary>
		/// Main method to call when to start the next sentence
		///</summary>
		public void DisplayNextSentence() {
			//Has the dialogue ended?
			if(queue.Count <= 0) {
				EndDialogue();
				return;
			}

			DialogueSpeaker currentDialoug = queue.Dequeue();

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
			dialogueNameText.text = currentDialoug.name;
			
			//Setup the sprite
			if(currentDialoug.sprite != null)
				imagePortrait.sprite = currentDialoug.sprite;
			else 
				imagePortrait.sprite = defaultPortrait;
			
			//Todo:
			//Setup the sentence
			string sentence = "hi";
			/*string sentence = currentDialoug.sentences;

			//Remove items from the queue after done using them
			queue.RemoveAt(0);*/


			IsDialoguePlaying = true;
			if(audioTyping) audioTyping.Play();

			//Start typing out sentance
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}

		///<summary>
		/// Called internally when the dialogue has finished typing out the sentence
		/// </summary>
		private void FinishedTypingSentance() {
			//Fades out the audio
			coroutineFadeOutAudio = StartCoroutine(FadeOutAudio(audioTyping, 0.5f));

			//Start the EndOfTyping Audio
			audioEndOfTyping.Play();
			//StartCoroutine(FadeOutAudio(audioEndOfTyping, 0.5f));
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
		private void DialogueBoxClosed() {
			IsDialoguePlaying = false;
		}

		private IEnumerator TypeSentence(string sentence) {
			dialogueSentenceText.text = "";

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

			FinishedTypingSentance();
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

			StopCoroutine(coroutineFadeOutAudio);
		}
		
		private void HideAllOptionButtons() {
			optionBtn1.SetActive(false);
			optionBtn2.SetActive(false);
			optionBtn3.SetActive(false);
			optionBtn4.SetActive(false);
		}

	}

	public enum TypeSpeed {
		Fast,
		Medium,
		Slow
	}

	/*[Serializable]
	public class Holder {
		public Sprite sprite;
		public string name;
		public string sentences;
		public bool btnOn;
	}*/

}