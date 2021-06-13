using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public Sound audioFile;
    [Tooltip("Enables or disables overlap, basically if the sound can play more than once at a time.")]
    public bool playOnce = false;
    [SerializeField] float chance;

    private void Start()
    {
        if (audioFile != null)
        {
            if (chance > Random.Range(0f, 1f))
            {
                AudioManager.instance.Play(audioFile, playOnce);
            }
        }
        else
        {
            Debug.LogError("Tried to play a sound but couldn't find it!");
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.instance == null) return;
        
        if (audioFile != null)
            AudioManager.instance.Stop(audioFile);
        else
            Debug.LogError("Tried to stop a sound on destroy but couldn't find it!");
    }
}