using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public TPSManager manager;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Enemy")) {
			manager.OnDeath();
			Destroy(gameObject);
		}
	}
}
