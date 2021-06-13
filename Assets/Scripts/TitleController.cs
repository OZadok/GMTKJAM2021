using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public string nextSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(nextSceneName);
    }
}
