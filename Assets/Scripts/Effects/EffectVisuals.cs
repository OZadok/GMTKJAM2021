using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectVisuals : ScriptableObject
{
	public Material Material;
	public List<GameObject> gameObjectsToSpawn;
	public Conditions textboxCondition;

	public void Set(GameObject gameObject)
	{
		var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
		var particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach (var spriteRenderer in spriteRenderers)
		{
			spriteRenderer.sharedMaterial = Material;
		}
		foreach (var particleSystem in particleSystems)
        {
			Destroy(particleSystem.gameObject);
        }
		foreach (var gameObjectToSpawn in gameObjectsToSpawn)
		{
			var spriteRenderer = gameObject.transform.GetComponentInChildren<SpriteRenderer>();
			Instantiate(gameObjectToSpawn, spriteRenderer.transform);
		}
		Vector3 position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
		FlairTextController.instance.CreateTextbox(textboxCondition, position);
	}
}
