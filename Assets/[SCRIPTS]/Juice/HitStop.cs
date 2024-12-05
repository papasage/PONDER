using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop instance;
    bool waiting;

    private void Awake()
    { 
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Stop(float duration)
    {
        if (waiting) return;
        StartCoroutine(Pause(duration));
    }

    public void StopAndShake(float stopDuration, float shakeDuration)
    {
        if (waiting) return;
        StartCoroutine(Pause(stopDuration));
    }

    IEnumerator Pause(float duration)
    {
        Time.timeScale = 0.0f;
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        waiting = false;
    }

}
