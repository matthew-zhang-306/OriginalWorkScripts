using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject player;
	Transform playerT;

	Vector3 baseCameraOffset;
	Vector3 midScreen;

	public float lerpSpeed;
	public float maxSeeAhead;

	void Start () {
		playerT = player.transform;
		baseCameraOffset = transform.position;

		midScreen = new Vector3(Screen.width, Screen.height, 0) / 2;
	}
	
	void FixedUpdate () {
		if (playerT == null) return;
		
		Vector3 mouseOffset = Input.mousePosition - midScreen;
		mouseOffset = new Vector3(mouseOffset.x / midScreen.x, mouseOffset.y / midScreen.y, 0) * maxSeeAhead;
		
		transform.position = Vector3.Lerp(transform.position, playerT.position + mouseOffset + baseCameraOffset, lerpSpeed);
	}
}
