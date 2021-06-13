using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFixer : MonoBehaviour
{
	struct StateComponentTime
	{
		public Collider2D Collider;
		public StateComponent StateComponent;
		public float startTime;
	}
	
	[Header("Parameters")]
	[SerializeField] public State stateToFix;
	[SerializeField] public float timeToFix = 1f;

	private List<StateComponentTime> stateComponentTimes;

	private void Awake()
	{
		stateComponentTimes = new List<StateComponentTime>();
	}

	private void FixedUpdate()
	{
		if (stateComponentTimes.Count == 0)
		{
			return;
		}
		var timeElapse = Time.time - stateComponentTimes[0].startTime;
		if (timeElapse >= timeToFix)
		{

			stateComponentTimes[0].StateComponent.CurrentState = State.Regular;
			stateComponentTimes[0].StateComponent.StopFix();
			stateComponentTimes.RemoveAt(0);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		OnColliderDetect(other);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		OnColliderDetect(other);
	}

	private void OnColliderDetect(Collider2D other)
	{
		var index = stateComponentTimes.FindIndex(x => x.Collider == other);
		if (index != -1)
		{
			return;
		}
		
		var stateComponent = other.GetComponent<StateComponent>();
		if (!stateComponent) return;
		if (stateComponent.CurrentState != stateToFix) return;
		
		var stateComponentTime = new StateComponentTime
		{
			Collider = other, StateComponent = stateComponent, startTime = Time.time
		};
		stateComponentTimes.Add(stateComponentTime);
		stateComponent.StartFix(timeToFix);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		var index = stateComponentTimes.FindIndex(x => x.Collider == other);
		if (index != -1)
		{
			stateComponentTimes[index].StateComponent.StopFix();
			stateComponentTimes.RemoveAt(index);
		}
	}
}
