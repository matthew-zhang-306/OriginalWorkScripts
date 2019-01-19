using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour {

	public GameObject lapTextObj;
	Text lapText;
	public GameObject timerTextObj;
	Text timerText;

	int numCheckpoints;
	int currentCheckpoint;
	int lapNum;
	public int totalLaps;

	public float maxTime;
	float timer;
	bool hasFinished;

	void Start () {
		int indexer = 0;
		foreach (Transform check in transform) {
			var checkpoint = check.GetComponent<Checkpoint>();
			checkpoint.race = this;
			checkpoint.index = indexer;
			indexer++;
		}

		numCheckpoints = indexer;
		currentCheckpoint = 0;
		lapNum = 1;
		timer = maxTime;

		lapText = lapTextObj.GetComponent<Text>();
		timerText = timerTextObj.GetComponent<Text>();
	}

	void Update() {
		if (hasFinished) return;

		timer -= Time.deltaTime;
		timerText.text = timer.ToString("F");

		if (lapNum > totalLaps)
			FinishRace("Finish!");
		else if (timer <= 0)
			FinishRace("Time out!");
	}
	
	public void PlayerHitCheckpoint(int index) {
		if (hasFinished) return;

		if ((currentCheckpoint + 1) % numCheckpoints == index) {
			currentCheckpoint = index;
			if (currentCheckpoint == 0)
				lapNum++;
		}
		else if ((currentCheckpoint + numCheckpoints - 1) % numCheckpoints == index) {
			if (currentCheckpoint == 0)
				lapNum--;
			currentCheckpoint = index;
		}

		UpdateLapText();
	}

	void UpdateLapText() {
		lapText.text = "Lap " + lapNum + "/" + totalLaps;
	}

	void FinishRace(string end) {
		hasFinished = true;
		StartCoroutine(MainMenu.EndGameCoroutine(end, 3f));
	}
}
