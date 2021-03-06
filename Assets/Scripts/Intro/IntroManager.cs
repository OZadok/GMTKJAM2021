using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("Story")]
    public IntroLine[] lines;
    public Sprite[] slides;
    public bool forceUppercase = true;

    [Header("Functionality")]
    public float typewriterDelay = 0.1f;
    public Text TextUI;
    public Image slideshowImage;
    [Tooltip("Loads in a scene (preferably the actual game's) after the intro is finished. If left blank, this will do nothing and the application will stay on this scene.")]
    public string nextScene = "";

    [Header("Audio")]
    public Sound keyboardClick;
    public Sound whiteNoise;
    public Sound newsFlash;

    // PRIVATES
    private string tempTxt;
    private int lineIndex = 0;
    private bool introEnded = false;

    void Start()
    {
        TextUI.text = "";

        StartCoroutine(StartIntro());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !introEnded)
        {
            AudioManager.instance.Play(keyboardClick, false);

            StopAllCoroutines();
            tempTxt = "";
            lineIndex++;

            // if exceeded lines
            if (lineIndex >= lines.Length)
                StartCoroutine(QuitIntro());
            else
                ShowSlide();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(QuitIntro());
        }
    }

    void ShowSlide()
    {
        if(lineIndex >= lines.Length)
        {
            Debug.LogError("LINE INDEX EXCEEDS LINES.LENGTH!!!");
            return;
        }

        if (forceUppercase)
            lines[lineIndex].dialogueLine = lines[lineIndex].dialogueLine.ToUpper();

        StartCoroutine(Typewriter());
        slideshowImage.sprite = slides[lines[lineIndex].slideIndex];
    }

    IEnumerator Typewriter()
    {
        foreach(char c in lines[lineIndex].dialogueLine)
        {
            tempTxt += c;
            TextUI.text = tempTxt;
            yield return new WaitForSeconds(typewriterDelay);
        }
    }

    IEnumerator QuitIntro()
    {
        TextUI.text = "";
        introEnded = true;
        AudioManager.instance.Play(whiteNoise, false);

        slideshowImage.sprite = slides[5];
        yield return new WaitForSeconds(1f);
        AudioManager.instance.StopAll();
        // THIS ENDS THE INTRO
        AudioManager.instance.StopAll();

        if (nextScene != "")
            SceneManager.LoadScene(nextScene);
        else
            Debug.Log("Intro ended sucessfully, requires Next Scene to be filled to move on");
    }

    IEnumerator StartIntro()
    {
        introEnded = true;
        TextUI.text = "";
        AudioManager.instance.Play(whiteNoise, false);

        slideshowImage.sprite = slides[5];
        yield return new WaitForSeconds(.5f);
        AudioManager.instance.StopAll();
        AudioManager.instance.Play(newsFlash, false);
        introEnded = false;
        ShowSlide();
    }
}
