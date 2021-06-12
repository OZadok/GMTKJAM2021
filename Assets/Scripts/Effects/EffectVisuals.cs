using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectVisuals : ScriptableObject
{
	public Material Material;
	public List<GameObject> gameObjectsToSpawn;

	public void Set(GameObject gameObject)
	{
		var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach (var spriteRenderer in spriteRenderers)
		{
			spriteRenderer.sharedMaterial = Material;
		}

		foreach (var gameObjectToSpawn in gameObjectsToSpawn)
		{
			Instantiate(gameObjectToSpawn, gameObject.transform);
		}
	}
}
