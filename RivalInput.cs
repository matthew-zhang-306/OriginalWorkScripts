using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalInput : IEntityInput {

	public InputHolder GetInput() {
		return new InputHolder();
	}

}
