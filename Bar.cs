using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

	Transform bar;
	float value;
	public float Value { get { return value; }}

	void Start () {
		bar = gameObject.transform;
		SetValue(1);
	}

	public void SetValue (float _value) {
		value = _value;
		bar.localScale = new Vector3(value, 1, 1);
	}

}
