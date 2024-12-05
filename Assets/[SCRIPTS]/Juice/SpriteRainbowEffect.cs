using UnityEngine;
using UnityEngine.UI;

public class SpriteRainbowEffect : MonoBehaviour
{
    public float lerpSpeed = 1.0f; // Adjust the speed of the color transition
    private float hueValue = 0.0f; // Initial hue value

    private Image sprite;

    void Start()
    {
        // Get the Image component attached to the GameObject
        sprite = GetComponent<Image>();
    }

    void Update()
    {
        // Increment the hue value over time
        hueValue += lerpSpeed * Time.deltaTime;

        // Ensure the hue value stays within the range [0, 1]
        hueValue %= 1.0f;

        // Convert the hue value to a color in the HSB color space
        Color startColor = Color.HSVToRGB(hueValue, 1.0f, 1.0f);
        Color endColor = Color.HSVToRGB((hueValue + 0.1f) % 1.0f, 1.0f, 1.0f);

        // Use Color.Lerp to smoothly transition between colors
        Color lerpedColor = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1));

        // Apply the lerped color to the Image component
        sprite.color = lerpedColor;
    }
}