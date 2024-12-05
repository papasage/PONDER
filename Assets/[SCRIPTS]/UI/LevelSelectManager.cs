using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager instance;
    private AudioSource menuAudio;

    [SerializeField] TMP_Text description;

    public GameObject mapPointer;
    public float lerpSpeed = 1f; // Speed of interpolation
    private Transform targetPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        mapPointer = GameObject.Find("MapPointer");
    }

    public void Start()
    {
        Cursor.visible = false;
        menuAudio = GetComponent<AudioSource>();

    }
    private void Update()
    {
        if (targetPosition != null)
        {
            // Lerp the position gradually over time
            mapPointer.transform.position = Vector3.Lerp(mapPointer.transform.position, targetPosition.position, lerpSpeed * Time.deltaTime);
        }
    }

    public void UpdateDescription(string text)
    {
        description.text = text;
    }

    public void UpdateMapMarker(Transform position)
    {
        targetPosition = position;
    }

    public void LoadFaldridge()
    {
        StartCoroutine(SceneTransition("Faldridge"));
    }
    public void LoadAldrac()
    {
        AudioManager.instance.SelectInvalid();
    }
    public void LoadMinnic()
    {
        AudioManager.instance.SelectInvalid();
    }
    public void LoadFrostholm()
    {
        AudioManager.instance.SelectInvalid();
    }
    public void LoadDelstan()
    {
        AudioManager.instance.SelectInvalid();
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator SceneTransition(string levelName)
    {
        AudioManager.instance.MenuStart();
        GetComponent<PlayableDirector>().Play();
        yield return new WaitForSecondsRealtime(2.2f);
        SceneManager.LoadScene(levelName);
    }
}
