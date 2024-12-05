using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private AudioSource menuAudio;
    public void Start()
    {
        Cursor.visible = false;
        menuAudio = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            Play();
        }
    }

    public void Play()
    {
        StartCoroutine(SceneTransition());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator SceneTransition()
    {
        AudioManager.instance.MenuStart();
        GetComponent<PlayableDirector>().Play();
        yield return new WaitForSecondsRealtime(2.2f);
        SceneManager.LoadScene("LevelSelect");
    }
}
