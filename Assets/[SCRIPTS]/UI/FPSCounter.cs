using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TMP_Text UIFPSCountText;
    private float deltaTime = 0.0f;
    private float FPS;
    private GUIStyle style = new GUIStyle();

    private void Awake()
    {
        //UIFPSCountText = GameObject.Find("UI_FPSCount").GetComponent<TextMeshPro>();
    }
    private void Start()
    {
        style.fontSize = 20;
        style.normal.textColor = Color.white;
    }

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        FPS = fps();

        if (FPS < 60)
        {
            UIFPSCountText.color = Color.red;
        }
        else { UIFPSCountText.color = Color.white; }

        UIFPSCountText.text = fps().ToString();
    }

    private void OnGUI()
    {
        int fps = Mathf.RoundToInt(1.0f / deltaTime);
        if (fps < 60f)
        {
            style.normal.textColor = Color.red;
        }
        else style.normal.textColor = Color.white;

        string text = $"FPS: {fps}";

       // GUI.Label(new Rect(10, 10, 100, 20), text, style);
    }

    private int fps()
    {
        int fps = Mathf.RoundToInt(1.0f / deltaTime);

        return fps;
    }
}