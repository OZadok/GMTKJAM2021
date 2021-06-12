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
    public AudioClip keyboardClick;
    public AudioClip whiteNoise;

    // PRIVATES
    private string tempTxt;
    private int lineIndex = 0;
    private bool introEnded = false;
    private AudioSource source;

    void Start()
    {
        TextUI.text = "";
        source = GetComponent<AudioSource>();

        StartCoroutine(StartIntro());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !introEnded)
        {
            source.PlayOneShot(keyboardClick);

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
        source.PlayOneShot(whiteNoise);

        slideshowImage.sprite = slides[5];
        yield return new WaitForSeconds(1f);

        // THIS ENDS THE INTRO
        if (nextScene != "")
            SceneManager.LoadScene(nextScene);
        else
            Debug.Log("Intro ended sucessfully, requires Next Scene to be filled to move on");
    }

    IEnumerator StartIntro()
    {
        introEnded = true;
        TextUI.text = "";
        source.PlayOneShot(whiteNoise);

        slideshowImage.sprite = slides[5];
        yield return new WaitForSeconds(.5f);
        introEnded = false;
        source.Stop();
        ShowSlide();
    }
}
