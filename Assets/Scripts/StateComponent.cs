using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateComponent : MonoBehaviour
{
	[SerializeField] private State currentState;

	public State CurrentState
	{
		get => currentState;
		set
		{
			if (!CanMoveToState(value))
			{
				return;
			}
			currentState = value;
			OnStateChange.Invoke(currentState);
		}
	}

	public StateEvent OnStateChange;

	public bool CanMoveToState(State next)
	{
		if (CurrentState == next)
		{
			return false;
		}

		return CurrentState switch
		{
			State.Regular => next == State.Damaged || next == State.OnFire || next == State.Criminal,
			State.OnFire => next == State.Damaged || next == State.Regular,
			State.Criminal => next == State.Damaged || next == State.Regular || next == State.OnFire,
			State.Damaged => next == State.Regular || next == State.Destroyed,
			_ => false
		};
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
