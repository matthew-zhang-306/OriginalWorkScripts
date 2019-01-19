using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrid : MonoBehaviour {

	public GameObject wall;
	public int width;
	public int height;
	public float borderPadding;
	
	HashSet<Vector2Int> wallTiles;

	Dictionary<Vector2Int, WallNode> debugTraversal;
	
	void Start () {
		wallTiles = new HashSet<Vector2Int>();

		SpawnArenaBorders();

		var walls = GetComponentsInChildren<Transform>();
		foreach (var wall in walls) {
			if (transform == wall) continue;
			AlignTransformToGrid(wall);
			InitializeWallTiles(wall);
		}
	}

	void Update () {
		// UnitTester();
	}

	public bool GetCollision(Vector2Int node) {
		if (Mathf.Abs(node.x) > width || Mathf.Abs(node.y) > height)
			return true;
		return wallTiles.Contains(node);
	}

	public List<Vector2Int> GetNeighboringPositions(Vector2Int center) {
		var output = new List<Vector2Int>();
		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++) {
				Vector2Int offset = new Vector2Int(x, y);

				switch (Mathf.Abs(x) + Mathf.Abs(y)) {
					case 0:	// if center, skip
						continue;
					case 1:	// if edge, check for in-bounds and collision
						if (GetCollision(center + offset))
							continue;
						break;
					case 2: // if corner, check all edges and corner
						if (GetCollision(center + offset) || GetCollision(new Vector2Int(center.x + x, center.y)) || GetCollision(new Vector2Int(center.x, center.y + y)))
							continue;
						break;
				}
				output.Add(center + offset);
			}
		return output;
	}

	void SpawnArenaBorders() {
		InstantiateBorder(new Vector3(-width - borderPadding, 0, 0),  new Vector3(borderPadding * 2, height + 4 * borderPadding, 0));
		InstantiateBorder(new Vector3(width + borderPadding, 0, 0),   new Vector3(borderPadding * 2, height + 4 * borderPadding, 0));
		InstantiateBorder(new Vector3(0, height + borderPadding, 0),  new Vector3(width + 4 * borderPadding, borderPadding * 2,  0));
		InstantiateBorder(new Vector3(0, -height - borderPadding, 0), new Vector3(width + 4 * borderPadding, borderPadding * 2,  0));
	}

	void InstantiateBorder(Vector3 location, Vector3 scale) {
		Transform border = Instantiate(wall, location, Quaternion.identity, transform).transform;
		border.localScale = scale;
	}

	void AlignTransformToGrid(Transform wall) {
		// Get opposite corners of the wall and round them to the nearest lattice point
		Vector3 topLeft = (wall.localPosition - wall.localScale / 2).Round(),
				bottomRight = (wall.localPosition + wall.localScale / 2).Round();
		Vector3 tempScale = (topLeft - bottomRight).Abs();

		// Move the wall into the desired position
		wall.localPosition = (topLeft + bottomRight) / 2;
		wall.localScale = new Vector3(tempScale.x, tempScale.y, 1);
	}

	void InitializeWallTiles(Transform wall) {
		// Get opposite corners of the wall
		Vector3 topLeft = wall.position - wall.localScale / 2;
		Vector3 bottomRight = wall.position + wall.localScale / 2;
		
		// Iterate over every 1x1 square on the wall and add that tile to the collision mapping
		for (int x = (int)(topLeft.x); x < bottomRight.x; x++)
			for (int y = (int)(topLeft.y); y < bottomRight.y; y++)
				wallTiles.Add(new Vector2Int(x, y));
	}

	void UnitTester() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			var worldCoord = Camera.main.transform.position;
			Debug.Log("TEST FOR WALLGRID");
			Debug.Log(worldCoord.WorldPointToNode());
			Debug.Log(wallTiles.Contains(worldCoord.WorldPointToNode()));
		}
	}

	public void SetTraversal(Dictionary<Vector2Int, WallNode> allNodes) {
		debugTraversal = allNodes;
	}

	void OnDrawGizmos() {
		if (debugTraversal != null) {
			foreach (var node in debugTraversal.Values) {
				Gizmos.color = new Color(0,0,255,0.3f);
				Gizmos.DrawCube(node.Value.NodeToWorldPoint(), new Vector3(node.GCost / 110, node.HCost / 110, 1));
			}
		}
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(width * 2, height * 2, 1));
	}
}
