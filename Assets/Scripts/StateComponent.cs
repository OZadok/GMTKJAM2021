using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class StateComponent : MonoBehaviour
{
	[SerializeField] private State currentState;
	[SerializeField] private EffectVisualsContainer effectVisualsContainer;
	public State CurrentState
	{
		get => currentState;
		set
		{
			if (!CanMoveToState(value))
			{
				return;
			}
			OnStateExit.Invoke(currentState);
			currentState = value;
			OnStateChange.Invoke(currentState);
			OnStateEnter.Invoke(currentState);
		}
	}

	public StateEvent OnStateChange;
	public StateEvent OnStateEnter;
	public StateEvent OnStateExit;

	private void Start()
	{
		OnStateChange.AddListener(state =>effectVisualsContainer.OnStateChange(state, gameObject));
	}

	public bool CanMoveToState(State next)
	{
		if (CurrentState == next)
		{
			return false;
		}

		return CurrentState switch
		{
			State.Regular => next == State.Damaged || next == State.OnFire || next == State.Criminal || next == State.Destroyed,
			State.OnFire => next == State.Damaged || next == State.Regular || next == State.Destroyed,
			State.Criminal => next == State.Damaged || next == State.Regular || next == State.OnFire,
			State.Damaged => next == State.Regular || next == State.Destroyed,
			_ => false
		};
	}

	[ContextMenu("ChangeToOnFire")]
	public void ChangeToOnFire()
	{
		CurrentState = State.OnFire;
	}

	public UnityAction<float> OnStartFix;
	public UnityAction OnStopFix;

	public void StartFix(float timeToFix)
	{
		OnStartFix?.Invoke(timeToFix);
	}

	public void StopFix()
	{
		OnStopFix?.Invoke();
	}
}

public enum State
{
	Regular, OnFire, Criminal, Damaged, Destroyed
}

[System.Serializable]
public class StateEvent : UnityEvent<State>
{
	
}
