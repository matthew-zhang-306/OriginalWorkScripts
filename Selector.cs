using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour {

	public Vector3 desiredPosition;
	public Vector3 offset;

	void Update () {
		transform.position = Vector3.Lerp(transform.position, desiredPosition + offset, 0.3f);
	}

	public void MoveToButton (PointerEventData eventData) {
		desiredPosition = eventData.pointerEnter.transform.position;
	}

}
