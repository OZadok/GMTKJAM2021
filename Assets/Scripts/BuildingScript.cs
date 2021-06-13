using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
	[SerializeField] public StateComponent StateComponent;
	[SerializeField] public Collider2D Collider;
	private void Start()
	{
		GameManager.Instance.Register(this);
		StateComponent.OnStateChange.AddListener(OnStateChanged);
	}
	
	private void OnStateChanged(State state)
	{
		if (state != State.Destroyed)
		{
			return;
		}

		Collider.enabled = false;
	}
}
