using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions {

	public static Vector3 Round(this Vector3 vector) {
		return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
	}

	public static Vector3 Abs(this Vector3 vector) {
		return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	}
	public static Vector2Int Abs(this Vector2Int vector) {
		return new Vector2Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
	}

	public static Vector2Int WorldPointToNode(this Vector3 vector) {
		return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
	}
	public static Vector3 NodeToWorldPoint(this Vector2Int vector) {
		return new Vector3(vector.x + 0.5f, vector.y + 0.5f, 0f);
	}

	public static Vector3 ToVector3(this Vector2 vector) {
		return new Vector3(vector.x, vector.y, 0);
	}
	public static Vector2 ToVector2(this Vector3 vector) {
		return new Vector2(vector.x, vector.y);
	}
}
