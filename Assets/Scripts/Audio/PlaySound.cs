using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public string audioName;
    public Sound audioFile;
    [Tooltip("Enables or disables overlap, basically if the sound can play more than once at a time.")]
    public bool playOnce = false;

    private void Start()
    {
        if (audioName != null && audioName != "")
            AudioManager.instance.Play(audioName, playOnce);
        else if (audioFile != null)
            AudioManager.instance.Play(audioFile, playOnce);
        else
            Debug.LogError("Tried to play a sound but couldn't find it!");
    }

    private void OnDestroy()
    {
        if (AudioManager.instance == null) return;

        if (audioName != null && audioName != "")
            AudioManager.instance.Stop(audioName);
        else if (audioFile != null)
            AudioManager.instance.Stop(audioFile);
        else
            Debug.LogError("Tried to stop a sound on destroy but couldn't find it!");
    }
}
