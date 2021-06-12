using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
	private static readonly int Rotation = Animator.StringToHash("rotation");
	
	[Header("References")] 
	[SerializeField] private Rigidbody2D rigidbody2D;
	[SerializeField] private Animator animator;
	

	private void Awake()
	{
		if (rigidbody2D == null)
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
		}
	}

	private void Update()
	{
		animator.SetFloat(Rotation, rigidbody2D.rotation);
	}
}
