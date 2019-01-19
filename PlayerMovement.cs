using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb2d;
	
	public float maxSpeed;
	public float slowMultiplier;

	public GameObject bullet;
	public float rechargeTime;
	float cooldownTimer;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		
		// Rotate player to face mouse
		float angle = Vector3.SignedAngle(Vector3.up, mousePos - transform.position, new Vector3(0, 0, 1));
		transform.eulerAngles = new Vector3(0, 0, angle);

		// If player can shoot and is trying to shoot, initialize a new bullet
		if (cooldownTimer <= 0 && Input.GetAxisRaw("Fire") > 0) {
			SimpleBullet b = GameObject.Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<SimpleBullet>();
			b.SetDirection(mousePos - transform.position);
			cooldownTimer = rechargeTime;
		}
		else {
			cooldownTimer -= Time.deltaTime;
		}
	}

	void FixedUpdate () {
		// Get directional input
		Vector2 rawVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		// Calculate resultant velocity
		rb2d.velocity = rawVelocity * maxSpeed;
		if (Input.GetAxisRaw("Slow") > 0)
			rb2d.velocity *= slowMultiplier;
	}

	
}
