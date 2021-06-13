using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Audio Manager Sound")]
public class Sound : ScriptableObject
{
	[Tooltip("If the RandomizedClip array has more than one sound, the game will randomly pick one of them and play it every time the Play() function is called on this sound, ignoring this clip property.")]
	public AudioClip clip;
	public AudioClip[] randomizedClip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = 0f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}
