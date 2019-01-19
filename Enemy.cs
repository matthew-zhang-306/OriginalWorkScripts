using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int totalHealth;
	int health;

	public TPSManager manager;

	void Start () {
		health = totalHealth;
	}

	public void Damage (int damage) {
		health -= damage;
		if (health <= 0) {
			manager.OnEnemyKill();
			Destroy(gameObject);
		}
	}

}
