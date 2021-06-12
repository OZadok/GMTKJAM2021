using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHumanController : CarController
{
	private void Update()
	{
		vertical = Input.GetAxis("Vertical");
		horizontal = Input.GetAxis("Horizontal");
	}
}
