using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
	[SerializeField] public StateComponent StateComponent;
	private void Start()
	{
		GameManager.Instance.Register(this);
	}
}
