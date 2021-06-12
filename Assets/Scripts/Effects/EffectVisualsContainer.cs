using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectVisualsContainer : ScriptableObject
{
	public EffectVisuals fireEffect;
	public EffectVisuals criminalEffect;
	
	private EffectVisuals temp;

	public void OnStateChange(State state, GameObject gameObject)
	{
		var ev = GetEffectVisuals(state);
		ev.Set(gameObject);
	}

	private EffectVisuals GetEffectVisuals(State state)
	{
		return state switch
		{
			// State.Regular => temp,
			State.OnFire => fireEffect,
			State.Criminal => criminalEffect,
			// State.Damaged => temp,
			// State.Destroyed => temp,
			// _ => temp
		};
	}
}
