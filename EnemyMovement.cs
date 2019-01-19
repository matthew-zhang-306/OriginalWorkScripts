using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public bool debugMode;

	public float maxSpeed;

	Pathfinder path;
	List<Vector3> currentTraversal;

	Transform player;
	Rigidbody2D rb2d;

	Vector2Int playerPreviousNode;

	void Start () {
		if (debugMode)
			Time.timeScale = 0.25f;

		WallGrid grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<WallGrid>();
		path = new Pathfinder(grid, debugMode);

		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
		if (player != null) {
			var playerCurrentNode = player.position.WorldPointToNode();

			// If the player has moved, the path might have to be recalculated.
			if (playerCurrentNode != playerPreviousNode) {
				// If there is no path, find a new one
				if (currentTraversal == null)
					FindPath();
				else {
					int i = currentTraversal.IndexOf(playerCurrentNode.NodeToWorldPoint());

					// If the player happens to still be on the path, just trim the traversal
					if (i >= 0)
						currentTraversal.RemoveRange(i, currentTraversal.Count - i);
					else
						FindPath();
				}
			}
			
			if (currentTraversal != null && currentTraversal.Count > 0) {
				// If first destination has been reached, remove and continue
				if (transform.position.WorldPointToNode() == currentTraversal[0].WorldPointToNode())
					currentTraversal.RemoveAt(0);
				
				// If there is still at least 1 place to go, move there
				if (currentTraversal.Count > 0)
					rb2d.velocity = maxSpeed * Vector3.Normalize(currentTraversal[0] - transform.position);
			}
			else {
				// If there is no path, idle
				rb2d.velocity = Vector2.zero;
			}

			playerPreviousNode = playerCurrentNode;
		}
		else {
			// If the player has disappeared, idle
			rb2d.velocity = Vector2.zero;
		}


		if (debugMode && currentTraversal != null) {
			for (int i = 1; i < currentTraversal.Count; i++)
				Debug.DrawLine(currentTraversal[i-1], currentTraversal[i]);
		}
	}

	void FindPath() {
		currentTraversal = path.Find(transform.position, player.position);						
	}
}
