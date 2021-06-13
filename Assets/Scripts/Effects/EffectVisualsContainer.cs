using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectVisualsContainer : ScriptableObject
{
	public EffectVisuals regularEffect;
	public EffectVisuals fireEffect;
	public EffectVisuals criminalEffect;
	public EffectVisuals damagedEffect;
	public EffectVisuals destroyedEffect;
	
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
			State.Regular => regularEffect,
			State.OnFire => fireEffect,
			State.Criminal => criminalEffect,
			State.Damaged => damagedEffect,
			State.Destroyed => destroyedEffect,
			_ => regularEffect
		};
	}
}
