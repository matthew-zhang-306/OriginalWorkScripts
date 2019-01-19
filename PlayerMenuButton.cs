using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public PointerEvent onPointerEnter;
	public PointerEvent onPointerExit;
	
	public void OnPointerEnter(PointerEventData eventData){ onPointerEnter.Invoke(eventData); }
	public void OnPointerExit(PointerEventData eventData){ onPointerExit.Invoke(eventData); }

	public GameObject playerObj;

	public Moves moveType;
	public GameObject moveMinigame;

	Move move;
	Entity player;
	Button button;
	Text buttonText;

	void Start () {
		move = Move.GetMove(moveType);
		player = playerObj.GetComponent<Entity>();
		button = GetComponent<Button>();
		buttonText = GetComponentInChildren<Text>();
	}

	void Update () {
		button.interactable = player.CanUseMove(move);
		buttonText.color = button.interactable ? button.colors.normalColor : button.colors.disabledColor;
	}

	public void OnClick () {
		GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnStateMachine>().PlayerChoice(moveMinigame, move);
	}
}

[System.Serializable]
public class PointerEvent : UnityEvent<PointerEventData> {}
