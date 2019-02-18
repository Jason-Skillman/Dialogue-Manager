using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DialogueManager {
	public class DialogueManager : MonoBehaviour {

		//Singleton
		public static DialogueManager main;

		private List<Holder> queueList = new List<Holder>();
		private List<ButtonOption[]> optionBtnList = new List<ButtonOption[]>();

		private bool isDialoguePlaying = false;
		public bool IsDialoguePlaying {
			get {
				return isDialoguePlaying;
			}
		}

		//Type speed
		public TypeSpeed typeSpeed = TypeSpeed.Fast;
		public float typeSpeedFast = 0.01f;
		public float typeSpeedMedium = 0.05f;
		public float typeSpeedSlow = 0.1f;

		//Component
		public Text dialogueNameText;
		public Text dialogueSentenceText;
		public Image imageNPSIcon;
		public AudioSource audioTyping;
		public AudioSource audioEndOfTyping;

		//Defaults
		public Sprite defaultPortrait;

		//Option Buttons
		public GameObject optionBtn1;
		public GameObject optionBtn2;
		public GameObject optionBtn3;
		public GameObject optionBtn4;
		public GameObject clickAreaField;

		//Delegate
		Queue<Callback> queueCallbacks = new Queue<Callback>();

		private bool removeButtons = false;
		private EventSystem eventSystem;
		private Animator animator;


		void Awake() {
			DontDestroyOnLoad(gameObject);

			//Singleton
			if(main == null) {
				main = this;
			} else {
				Destroy(gameObject);
			}
			
			eventSystem = GameObject.Find("Event System").GetComponent<EventSystem>();
			animator = GetComponent<Animator>();
		}
		
		///<summary>Main method to call when adding new dialogue to the queue</summary>
		public void AddNewDialogue(Dialogue[] dialogue, ButtonOption[] optionBtn = null, Callback callbackFinished = null) {
			animator.SetBool("IsOpen", true);

			List<Holder> tempList = new List<Holder>();

			//Add the callback delegate to the callback list
			if(callbackFinished != null) {
				queueCallbacks.Enqueue(callbackFinished);
			}

			//Loop through dialogue amount
			for(int i = 0; i < dialogue.Length; i++) {
				//Loop through sentances amount
				foreach(string sentence in dialogue[i].sentences) {
					Holder holder = new Holder();
					holder.name = dialogue[i].name;
					holder.sentences = sentence;
					holder.btnOn = false;

					tempList.Add(holder);
				}
			}

			//Check if their is a button option and add it to last item
			if(optionBtn != null && optionBtn.Length != 0) {
				tempList[tempList.Count - 1].btnOn = true;

				optionBtnList.Add(optionBtn);
			}

			//Add all created lists to master list
			foreach(Holder hold in tempList) {
				queueList.Add(hold);
			}

			//Startup for the first time
			if(isDialoguePlaying == false) {
				DisplayNextSentence();
			}
		}

		///<summary>Main method to call when adding new dialogue to the queue</summary>
		public void AddNewDialogue(List<DialogueSingle> oldDialogue) {
			Dialogue[] newDialoguesOLD = new Dialogue[oldDialogue.Count];
			List<Dialogue> newdialogues = new List<Dialogue>();

			for(int i = 0; i < oldDialogue.Count; i++) {
				newdialogues.Add(new Dialogue());

				newdialogues[i].name = oldDialogue[i].name;

				newdialogues[i].sentences = new string[1];
				newdialogues[i].sentences[0] = oldDialogue[i].sentence;
			}
			
			AddNewDialogue(newdialogues.ToArray());
		}
		

		///<summary>Main method to call when to start the next sentance</summary>
		public void DisplayNextSentence() {

			//Has the dialogue ended?
			if(queueList.Count <= 0) {
				EndDialogue();
				return;
			}

			//Remove the buttons if the previous frame had them
			if(removeButtons) {
				removeButtons = false;

				HideAllOptionButtons();
				optionBtnList.RemoveAt(0);
			}

			//If the current dialogue frame has a button option layout
			if(queueList[0].btnOn) {

				for(int i = 0; i < optionBtnList[0].Length; i++) {
					if(i == 0) {
						optionBtn1.SetActive(true);

						eventSystem.SetSelectedGameObject(optionBtn1);

						optionBtn1.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn1.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn1.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][0].trigger.TriggerDialogue();
							DisplayNextSentence();
						});
					} else if(i == 1) {
						optionBtn2.SetActive(true);
						optionBtn2.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn2.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn2.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][1].trigger.TriggerDialogue();
							DisplayNextSentence();
						});
					} else if(i == 2) {
						optionBtn3.SetActive(true);
						optionBtn3.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn3.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn3.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][2].trigger.TriggerDialogue();
							DisplayNextSentence();
						});
					} else if(i == 3) {
						optionBtn4.SetActive(true);
						optionBtn4.GetComponent<Text>().text = optionBtnList[0][i].name;
						optionBtn4.GetComponent<Button>().onClick.RemoveAllListeners();
						optionBtn4.GetComponent<Button>().onClick.AddListener(delegate {
							optionBtnList[0][3].trigger.TriggerDialogue();
							DisplayNextSentence();
						});
					}
				}

				removeButtons = true;
				clickAreaField.SetActive(false);
			}
			//If the current frame does not have a button option layout reset the buttons and frame
			else {
				HideAllOptionButtons();
				clickAreaField.SetActive(true);
				eventSystem.SetSelectedGameObject(clickAreaField);
			}

			//Set the name and sentance to the next in the queue
			dialogueNameText.text = queueList[0].name;
			if(queueList[0].sprite != null) {
				imageNPSIcon.sprite = queueList[0].sprite;
			} else {
				imageNPSIcon.sprite = defaultPortrait;
			}
			string sentence = queueList[0].sentences;

			//Remove items from the queue after done using them
			queueList.RemoveAt(0);


			isDialoguePlaying = true;
			audioTyping.Play();

			//Start typing out sentance
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}

		///<summary>Called internaly when the dialouge has finished typing out the sentance</summary>
		private void FinishedTypingSentance() {
			//Fades out the audio
			StartCoroutine(FadeOutAudio(audioTyping, 0.5f));
			
			//Start the EndOfTyping Audio
			audioEndOfTyping.Play();
			//StartCoroutine(FadeOutAudio(audioEndOfTyping, 0.5f));
		}

		///<summary>Called internaly when the dialouge box has finished</summary>
		private void EndDialogue() {
			animator.SetBool("IsOpen", false);

			//isDialoguePlaying = false;
			
			while(queueCallbacks.Count > 0) {
				Callback callback = queueCallbacks.Dequeue();
				callback();
			}
		}

		///<summary>Called externaly by the animator controller when the dialouge box has finished its close animation</summary>
		private void DialogueBoxClosed() {
			isDialoguePlaying = false;
		}

		private void HideAllOptionButtons() {
			optionBtn1.SetActive(false);
			optionBtn2.SetActive(false);
			optionBtn3.SetActive(false);
			optionBtn4.SetActive(false);
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
			audioSource.volume = 1;	//1 is Max volume
			while(audioSource.volume > 0) {
				audioSource.volume -= 1 * Time.deltaTime / FadeTime;

				yield return null;
			}

			audioSource.Stop();
			audioSource.volume = 1;
		}

	}

	public delegate void Callback();

	public enum TypeSpeed {
		Fast,
		Medium,
		Slow
	}

	[System.Serializable]
	public class Holder {
		public Sprite sprite;
		public string name;
		public string sentences;
		public bool btnOn;
	}
	
}