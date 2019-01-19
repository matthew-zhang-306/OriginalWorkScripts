using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	Rigidbody2D rb2d;
	IEntityInput input;

	public float maxSpeed;
	public float acceleration;
	public float turnSpeed;
	public float offRoadMultiplier;

	bool onRoad;

	void Start () {
		if (this.CompareTag("Player"))
			input = new PlayerInput();
		else
			input = new RivalInput();
		
		onRoad = true;

		rb2d = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
		InputHolder buttons = input.GetInput();

		float theMaxSpeed = maxSpeed;
		if (!onRoad)
			theMaxSpeed *= offRoadMultiplier;

		float currForwardVelocity = Vector2.Dot(transform.right, rb2d.velocity);

		if (buttons.Accelerate) {
			Debug.Log(transform.right * Mathf.Min(currForwardVelocity + acceleration, theMaxSpeed));
			rb2d.velocity = transform.right * Mathf.Min(currForwardVelocity + acceleration, theMaxSpeed);
		}
		else if (buttons.Brake) {
			rb2d.velocity = transform.right * Mathf.Max(0f, currForwardVelocity - acceleration);
		}
		else {
			rb2d.velocity = transform.right * Mathf.Max(0f, currForwardVelocity - acceleration/2);
		}

		rb2d.angularVelocity = -buttons.Turn * turnSpeed;
	}


	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Road"))
			onRoad = true;
	}
	void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag("Road"))
			onRoad = false;
	}
}
