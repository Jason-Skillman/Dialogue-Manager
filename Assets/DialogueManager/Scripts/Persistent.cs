using UnityEngine;

public class Persistent : MonoBehaviour {

	public static Persistent main;

	void Awake() {
		DontDestroyOnLoad(gameObject);

		//Singleton
		if(!main)
			main = this;
		else
			Destroy(gameObject);
	}

}
