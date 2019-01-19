using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int index;
	public RaceManager race;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<CarController>() != null) {
			race.PlayerHitCheckpoint(index);
		}
	}
}
