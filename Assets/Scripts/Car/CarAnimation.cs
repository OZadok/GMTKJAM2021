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

	[Header("Parameters")] 
	[SerializeField] private int framesAmount = 8;

	public int FramesAmount => framesAmount;


	private void Awake()
	{
		if (rigidbody2D == null)
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
		}
		
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
		
		animator.speed = 0;
	}

	private void Update()
	{
		// animator.SetFloat(Rotation, rigidbody2D.rotation);
		var rotation = rigidbody2D.rotation;
		if (rotation < 360f) rotation += 360f;
		rotation /= 360f;
		rotation *= FramesAmount;
		// rotation += 0.5f;
		var theFrame = Mathf.Round(rotation);

		animator.Play(0, 0, 1 - theFrame / FramesAmount);
	}
}
