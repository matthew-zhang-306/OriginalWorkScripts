using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour {

	string currentlyWriting;
	Text text;

	public int charactersPerSecond;
	public bool IsLoading;

	bool started;

	void Start () {
		if (!started) {
			text = GetComponent<Text>();
			text.text = "";
			IsLoading = false;
			
			started = true;
		}
	}

	public void UpdateText (string desiredText) {
		currentlyWriting = desiredText;
		StopAllCoroutines();
		StartCoroutine(WriteText(desiredText, false));
	}

	public void FastLoad () {
		StopAllCoroutines();
		StartCoroutine(WriteText(currentlyWriting, true));
	}

	IEnumerator WriteText(string desiredText, bool fastLoad) {
		if (!started) Start();

		IsLoading = true;

		if (fastLoad)
			text.text = desiredText;
		else {
			text.text = "";
			foreach (char c in desiredText.ToCharArray()) {
				text.text += c;
				yield return new WaitForSeconds(1.0f / charactersPerSecond);
			}
		}

		IsLoading = false;
	}

	public void SetTextActive (bool active) {
		text.enabled = active;
	}
}
