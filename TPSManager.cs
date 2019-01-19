using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TPSManager : MonoBehaviour {

	public GameObject playerObj;
	public GameObject enemies;
	int numEnemies;

	void Start () {
		playerObj.GetComponent<PlayerHealth>().manager = this;
		foreach (Transform t in enemies.transform) {
			if (t.GetComponent<Enemy>() != null) {
				t.GetComponent<Enemy>().manager = this;
				numEnemies++;
			}
		}

	}

	public void OnDeath() {
		StartCoroutine(MainMenu.EndGameCoroutine("Game Over", 2f));
	}
	public void OnEnemyKill() {
		numEnemies--;
		if (numEnemies == 0)
			StartCoroutine(MainMenu.EndGameCoroutine("You Win!", 2f));
	}

	IEnumerator Reset () {
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}
