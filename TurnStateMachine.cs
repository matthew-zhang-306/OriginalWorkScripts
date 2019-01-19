using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TurnState {
	START,
	PLAYERTURN,
	PLAYERMOVE,
	ENEMYTURN,
	ENEMYMOVE,
	WIN,
	LOSS,
	RESET
}


public class TurnStateMachine : MonoBehaviour {

	TurnState turnState;

	public GameObject heroObj;
	Entity hero;
	public GameObject villainObj;
	Entity villain;

	public GameObject bottomMenu;
	GameObject playerMenu;
	TextDisplay textDisplay;

	MoveMinigame activeMinigame;

	void Start () {
		foreach (Transform t in bottomMenu.transform) {
			switch (t.name) {
				case "PlayerMenu":
					playerMenu = t.gameObject;
					break;
				case "TextDisplay":
					textDisplay = t.gameObject.GetComponent<TextDisplay>();
					break;
			}
		}

		hero = heroObj.GetComponent<Entity>();
		villain = villainObj.GetComponent<Entity>();

		SetTurnState(TurnState.START);
	}

	void Update () {
		switch (turnState) {
			case TurnState.START:
				CheckForTextAdvance(TurnState.PLAYERTURN);
				break;
			case TurnState.PLAYERMOVE:
				if (activeMinigame == null)
					CheckForTextAdvance(villain.IsDead ? TurnState.WIN : TurnState.ENEMYTURN);
				break;
			case TurnState.ENEMYMOVE:
				CheckForTextAdvance(hero.IsDead ? TurnState.LOSS : TurnState.PLAYERTURN);
				break;
			case TurnState.WIN:
				CheckForTextAdvance(TurnState.RESET);
				break;
			case TurnState.LOSS:
				CheckForTextAdvance(TurnState.RESET);
				break;
		}
	}
	
	void SetTurnState (TurnState newState) {
		turnState = newState;
		switch (turnState) {
			case TurnState.START:
				textDisplay.UpdateText("Battle begin!");
				break;
			case TurnState.PLAYERTURN:
				textDisplay.SetTextActive(false);
				playerMenu.SetActive(true);
				break;
			case TurnState.PLAYERMOVE:
				textDisplay.SetTextActive(true);
				playerMenu.SetActive(false);
				break;
			case TurnState.ENEMYTURN:
				EnemyChoice();
				break;
			case TurnState.WIN:
				textDisplay.UpdateText("You won!");
				Destroy(villainObj);
				break;
			case TurnState.LOSS:
				textDisplay.UpdateText("You lost!");
				Destroy(heroObj);
				break;
			case TurnState.RESET:
				StartCoroutine(MainMenu.EndGameCoroutine("Battle Over", 0f));
				break;
		}
	}

	void CheckForTextAdvance(TurnState nextState) {
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
			if (textDisplay.IsLoading)
				textDisplay.FastLoad();
			else
				SetTurnState(nextState);
	}

	public void PlayerChoice (GameObject minigameObj, Move move) {
		if (turnState != TurnState.PLAYERTURN) return;

		activeMinigame = Instantiate(minigameObj, hero.transform.position, Quaternion.identity).GetComponent<MoveMinigame>();
		activeMinigame.Init(move, hero, villain, textDisplay);

		SetTurnState(TurnState.PLAYERMOVE);
	}

	public void EnemyChoice () {
		Move move = villain.MakeChoice();
		move.UseMoveOn(villain, hero, 1, textDisplay);
		SetTurnState(TurnState.ENEMYMOVE);
	}
}
