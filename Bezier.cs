using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier {

	public List<Vector2> NodePoints;
	public List<Vector2> ControlPointsOffset;
	private List<Vector2> curvePoints;
	public List<Vector2> CurvePoints { get { return curvePoints; }}
	public int CurvePointCount { get { return curvePoints == null ? 0 : curvePoints.Count(); }}

	public float resolution;

	public Bezier(float reso) {
		Init(reso);
	}

	public void Init(float reso) {
		NodePoints = new List<Vector2>();
		ControlPointsOffset = new List<Vector2>();
		resolution = reso;
	}

	public void Render() {
		curvePoints = new List<Vector2>();
		for (int s = 0; s < NodePoints.Count - 1; s++)
			for (float t = 0; t <= 1; t += 1.0f / resolution)
				curvePoints.Add(GetBezier(s, t));
	}

	Vector2 GetBezier(int s, float t) {
		List<Vector2> points = new List<Vector2>(){
			NodePoints[s],
			NodePoints[s]   + ControlPointsOffset[s],
			NodePoints[s+1] - ControlPointsOffset[s+1],
			NodePoints[s+1]
		};
		Vector2 accum = Vector2.zero;
		
		var n = points.Count() - 1;
		for (int i = 0; i <= n; i++)
			accum += (float)(Binom(n, i) * Math.Pow(1 - t, n - i) * Math.Pow(t, i)) * points[i];
		return accum;
	}

	long Binom(int n, int i) {
		long result = 1;
		for (int k = 1; k <= Math.Min(i, n - i); k++) {
			result *= n--;
			result /= k;
		}
		return result;
	}

	public override string ToString() {
		return "BEZIER: [" + String.Join(",", NodePoints.Select(v => v.ToString()).ToArray()) + "], RESO: " + resolution;
	}

}
