using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour {

	public float speed;
	public float aliveTime;
	float timer;

	Vector3 direction = Vector3.zero;
	Rigidbody2D rb2d;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update () {
		timer += Time.deltaTime;
		
		// Check for elapsed lifespan
		if (timer >= aliveTime)
			KillSelf();
	}

	void FixedUpdate () {
		rb2d.velocity = direction * speed;
	}

	void OnTriggerEnter2D (Collider2D other) {
		// Check for wall collision
		if (other.CompareTag("Wall"))
			KillSelf();
		// Check for enemy collision
		if (other.CompareTag("Enemy")) {
			other.gameObject.GetComponent<Enemy>().Damage(1);
			KillSelf();
		}
	}

	void KillSelf() {
		Destroy(gameObject);
	}

	public void SetDirection (Vector3 dir) {
		// Vector3.zero is the default value of direction to check if it has yet to be set
		if (direction == Vector3.zero)
			direction = dir.normalized;
	}

}
