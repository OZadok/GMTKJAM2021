using UnityEngine.Audio;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public List<Sound> sounds = new List<Sound>();

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in Resources.LoadAll("Audio", typeof(Sound)))
        {
			sounds.Add(s);
        }

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}

	}

	public void Play(string sound, bool playOnce)
	{
		Sound s = sounds.Find(item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if(!playOnce)
			s.source.Play();
		else 
		if(!s.source.isPlaying)
			s.source.Play();
	}

	public void Play(Sound sound, bool playOnce)
	{
		Sound s = sounds.Find(item => item == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if (!playOnce)
			s.source.Play();
		else
		if (!s.source.isPlaying)
			s.source.Play();
	}

	public void Stop(string sound)
    {
		Sound s = sounds.Find(item => item.name == sound);
		if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

	public void Stop(Sound sound)
	{
		Sound s = sounds.Find(item => item == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void StopAll()
    {
		foreach(Sound s in sounds)
        {
			s.source.Stop();
        }
    }

	/*
	[MenuItem("Tools/New Sound...")]
	static void DoSomething(MenuCommand command)
	{
		Sound s = (Sound)ScriptableObject.CreateInstance(typeof(Sound));
		AssetDatabase.CreateAsset(s, "Assets/Resources/Audio/NewSound.asset");
	}
	*/

}
