using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNode : IComparable {

	Vector2Int value;
	public Vector2Int Value { get { return value; }}

	float gcost;
	public float GCost { get { return gcost; } set { gcost = value; }}
	float hcost;
	public float HCost { get { return hcost; }}
	public float FCost { get { return gcost + hcost; }}

	WallNode parent;
	public WallNode Parent { get { return parent; } set { parent = value; }}

	public WallNode(Vector2Int _value, Vector2Int target, WallNode _parent) {
		value = _value;
		parent = _parent;
		gcost = GetDistance(parent);
		hcost = GetDistance(target);
	}

	float GetDistance(Vector2Int target) {
		if (this.value == new Vector2Int(-1, -1))
			Debug.Log(value + " " + target + " " + (14 * Mathf.Min((target - value).Abs().x, (target - value).Abs().y) + 10 * Mathf.Abs((target - value).Abs().x - (target - value).Abs().y)));

		Vector2Int offset = (target - value).Abs();
		return 14 * Mathf.Min(offset.x, offset.y) + 10 * Mathf.Abs(offset.x - offset.y);
	}
	public float GetDistance(WallNode node) {
		if (node != null)
			return GetDistance(node.Value);
		else
			return 0;
	}

	public int CompareTo(object n) {
		return (int)(this.FCost - ((WallNode)n).FCost);
	}

	public override string ToString() {
		return "WallNode - " + this.Value + ": " + this.GCost + ", " + this.HCost + " = " + this.FCost;
	}
}