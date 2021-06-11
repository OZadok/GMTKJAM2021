using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHumanController : CarController
{
	private void Update()
	{
		moveForward = Input.GetAxis("Vertical") > 0.4f;
		horizontal = Input.GetAxis("Horizontal");
	}
}
