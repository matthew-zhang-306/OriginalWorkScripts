using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class CurveDrawer : MonoBehaviour {

	public Bezier Curve;
	public float resolution;
	public float width;
	public int indexToDelete;

	public string curveJson;

	public void Init() {
		if (Curve == null)
			try {
				Curve = JsonUtility.FromJson<Bezier>(curveJson);
			} catch {

			}
		if (Curve == null) {
			Curve = new Bezier(resolution);
			AddPoint(new Vector2(0, 0));
			AddPoint(new Vector2(10, 10));
		}
	}

	public void AddPoint(Vector2 point) {
		Init();

		Curve.NodePoints.Add(point);
		Curve.ControlPointsOffset.Add(new Vector2(10, 10));
		Generate();
	}

	public void DeletePoint() {
		Curve.NodePoints.RemoveAt(indexToDelete);
		Curve.ControlPointsOffset.RemoveAt(indexToDelete);
		Generate();
	}
	
	public void Generate() {
		Curve.resolution = resolution;
		Curve.Render();
		GetComponent<LineRenderer>().positionCount = Curve.CurvePointCount;
		GetComponent<LineRenderer>().SetPositions(Curve.CurvePoints.Select(v => v.ToVector3()).ToArray());

		Mesh mesh = GetCurveMesh();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<PolygonCollider2D>().points = ToPolygonColliderPoints(mesh.vertices);
	}

	public void Save() {
		curveJson = JsonUtility.ToJson(Curve);
	}

	Mesh GetCurveMesh() {
		Mesh mesh = new Mesh();

		var verts = new List<Vector3>();
		var triangles = new List<int>();
		
		for (int v = 0; v < Curve.CurvePointCount; v++) {
			// Get the vector pointing perpendicularly to the curve at the desired point
			Vector3 tangent = v > 0 ? Curve.CurvePoints[v] - Curve.CurvePoints[v-1] : Curve.ControlPointsOffset[0];
			Vector3 perpendicular = new Vector3(-tangent.y, tangent.x, tangent.z).normalized;
			
			// Extend forwards and backwards by the vector according to the curve width
			verts.Add(Curve.CurvePoints[v].ToVector3() + perpendicular * width);
			verts.Add(Curve.CurvePoints[v].ToVector3() - perpendicular * width);

			// If a line segment has been made, form two clockwise triangles out of it
			if (v > 0) {
				var t = verts.Count - 1; // the -1 allows t to be the index of the last element
				triangles.AddRange(new List<int>(){ t-3, t-1, t-2, t-2, t-1, t });
			}
		}

		Debug.Log(string.Join(",", verts.Select(o => o.ToString()).ToArray()));
		Debug.Log(string.Join(",", triangles.Select(o => "" + o).ToArray()));
		mesh.vertices = verts.ToArray();
		mesh.triangles = triangles.ToArray();
		return mesh;
	}

	Vector2[] ToPolygonColliderPoints(Vector3[] vertices) {
		Vector2[] points = new Vector2[vertices.Length];

		// Formula: every even point in order, then every odd point in reverse
		for (int p = 0; 2 * p < vertices.Length; p++) {
			points[p] = vertices[2 * p];
			points[points.Length - 1 - p] = vertices[2 * p + 1];
		}

		return points;
	}
}


#if UNITY_EDITOR

// A custom editor that allows us to drag the points around in the scene view
[CustomEditor(typeof(CurveDrawer))]
public class CurveDrawerEditor : UnityEditor.Editor {
	void OnSceneGUI() {
		var curveDrawer = (CurveDrawer)target;
		var curve = curveDrawer.Curve;
		
		// Check if the curve exists - if it does not, initialize it with two default points
		if (curve == null || curve.NodePoints == null) {
			curveDrawer.Init();
		}
		// If the curve still doesn't exist, it's probably because the JSON utility is busy doing its thing, so wait
		if (curve == null || curve.NodePoints == null)
			return;
		
		bool changedFlag = false;
		for (int i = 0; i < curve.NodePoints.Count; i++) {
			Handles.color = Color.white;
			
			// Draw the node point handle
			Vector2 nodePos = curve.NodePoints[i];
			Vector2 newNodePos = Handles.FreeMoveHandle(nodePos, Quaternion.identity, 1f, Vector2.zero, Handles.CylinderHandleCap);
			// Check for a change in position
			if (newNodePos != nodePos) {
				curve.NodePoints[i] = newNodePos;
				changedFlag = true;
			}

			Handles.color = Color.yellow;

			// If the point is not the last one, draw the forward facing control point handle
			if (i != curve.NodePoints.Count - 1) {
				Vector2 ctrlPos = curve.NodePoints[i] + curve.ControlPointsOffset[i];
				Vector2 newCtrlPos = Handles.FreeMoveHandle(ctrlPos, Quaternion.identity, 1f, Vector2.zero, Handles.CylinderHandleCap);
				Handles.DrawLine(newNodePos, newCtrlPos);
				// Check for a change in position
				if (newCtrlPos != ctrlPos) {
					curve.ControlPointsOffset[i] = newCtrlPos - newNodePos;
					changedFlag = true;
				}
			}

			// If the point is not the first one, draw the backward facing control point handle
			if (i != 0) {
				Vector2 ctrlPos = curve.NodePoints[i] - curve.ControlPointsOffset[i];
				Vector2 newCtrlPos = Handles.FreeMoveHandle(ctrlPos, Quaternion.identity, 1f, Vector2.zero, Handles.CylinderHandleCap);
				Handles.DrawLine(newNodePos, newCtrlPos);
				// Check for a change in position
				if (newCtrlPos != ctrlPos) {
					curve.ControlPointsOffset[i] = newNodePos - newCtrlPos;
					changedFlag = true;
				}
			}
		}

		// If anything has changed, re-render the line
		if (changedFlag)
			curveDrawer.Generate();
	}

	public override void OnInspectorGUI() {
		GUI.skin.label.wordWrap = true;
		DrawDefaultInspector();

		var curveDrawer = (CurveDrawer)target;

		// Add point button
		if (GUILayout.Button("Add Point"))
			curveDrawer.AddPoint(new Vector2(0, 0));
		
		// Delete point button
		if (GUILayout.Button("Delete Point"))
			curveDrawer.DeletePoint();

		if (GUILayout.Button("Regen"))
			curveDrawer.Generate();
		
		if (GUILayout.Button("Save"))
			curveDrawer.Save();
		GUILayout.Label("Note: after pressing 'Save', when the 'Curve Json' field updates, CUT (Ctrl-X) and PASTE (Ctrl-V) the string back into the box so that Unity will keep it there! It's a weird thing but if you don't do that, Unity will just get rid of all your data when you reload the scene.");
	}
}

#endif