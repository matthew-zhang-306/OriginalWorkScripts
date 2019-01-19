using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {

	WallGrid grid;
	bool debugMode;

	public Pathfinder(WallGrid _grid, bool _debugMode) {
		grid = _grid;
		debugMode = _debugMode;
	}

	public List<Vector3> Find(Vector3 initial, Vector3 target) {
		return Find(initial.WorldPointToNode(), target.WorldPointToNode());
	}

	// Implementation of A*
	List<Vector3> Find(Vector2Int initial, Vector2Int target) {
		// Points to a node for each traversed location
		var allNodes = new Dictionary<Vector2Int, WallNode>();
		// Stores all nodes that have yet to be traversed, with the lowest f-cost at the top
		var nextOpenNode = new PriorityQueue<WallNode>();
		// Stores all nodes that have already been considered
		var closedNodes = new HashSet<WallNode>();
		// Stores the node currently being addressed
		var currentNode = new WallNode(initial, target, null);

		// First node instantiation
		currentNode.GCost = 0f;
		currentNode.Parent = null;
		allNodes.Add(initial, currentNode);
		nextOpenNode.Enqueue(currentNode);

		while (nextOpenNode.Size > 0) {
			currentNode = nextOpenNode.Dequeue();

			// set node to closed state
			closedNodes.Add(currentNode);

			// path has not been found - invalid node reached
			if (currentNode.FCost >= float.MaxValue)
				break;

			// path has been found
			if (currentNode.Value == target) {
				if (debugMode)
					grid.SetTraversal(allNodes);
				return NodeToList(currentNode);
			}

			// check neighbors
			foreach(Vector2Int neighborLoc in grid.GetNeighboringPositions(currentNode.Value)) {
				WallNode neighborNode;
				
				if (allNodes.ContainsKey(neighborLoc)) {
					// skip any closed nodes
					if (closedNodes.Contains(allNodes[neighborLoc]))
						continue;
					neighborNode = allNodes[neighborLoc];
				} else {
					// create a new node
					neighborNode = new WallNode(neighborLoc, target, currentNode);
					allNodes.Add(neighborLoc, neighborNode);
					nextOpenNode.Enqueue(neighborNode);
				}

				// check if new optimal path is found
				float newGCost = currentNode.GCost + neighborNode.GetDistance(currentNode);
				if (newGCost < neighborNode.GCost) {
					// mark node as invalid by setting its cost to an enormous value
					neighborNode.GCost = float.MaxValue - neighborNode.HCost;

					// enqueue a brand new node with the updated cost
					WallNode newNeighborNode = new WallNode(neighborLoc, target, currentNode);
					allNodes[neighborLoc] = newNeighborNode;
					nextOpenNode.Enqueue(newNeighborNode);
				}
			}
		}

		// no path found
		return null;
	}

	List<Vector3> NodeToList(WallNode final) {
		var output = new List<Vector3>();
		var currentNode = final;
		while (currentNode.Parent != null) {
			output.Insert(0, currentNode.Value.NodeToWorldPoint());
			currentNode = currentNode.Parent;
		}
		return output;
	}
}