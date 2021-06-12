using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectVisualsContainer : ScriptableObject
{
	public EffectVisuals fireEffect;

	public void OnStateChange(State state, GameObject gameObject)
	{
		switch (state)
		{
			case State.Regular:
				break;
			case State.OnFire:
				fireEffect.Set(gameObject);
				break;
			case State.Criminal:
				break;
			case State.Damaged:
				break;
			case State.Destroyed:
				break;
			default:
				break;
		}
	}
}
