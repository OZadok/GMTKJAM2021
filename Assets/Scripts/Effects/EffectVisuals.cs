using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectVisuals : ScriptableObject
{
	public Material Material;
	public List<GameObject> gameObjectsToSpawn;

	public bool spawnFire;

	public void Set(GameObject gameObject)
	{
		var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach (var spriteRenderer in spriteRenderers)
		{
			spriteRenderer.sharedMaterial = Material;
		}
		foreach (var gameObjectToSpawn in gameObjectsToSpawn)
		{
			var spriteRenderer = gameObject.transform.GetComponentInChildren<SpriteRenderer>();
			Instantiate(gameObjectToSpawn, spriteRenderer.transform);
		}
	}
}
