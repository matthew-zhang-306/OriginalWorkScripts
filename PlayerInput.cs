using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IEntityInput {

	public InputHolder GetInput() {
		return new InputHolder(Input.GetKey(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow), Input.GetAxisRaw("Horizontal"));
	}
	
}
