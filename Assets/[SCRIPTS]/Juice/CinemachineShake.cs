using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance;

    private CinemachineVirtualCamera vCamA;
    private CinemachineVirtualCamera vCamB;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        StartCoroutine(ShakeCoroutine(intensity, time));
    }

    private IEnumerator ShakeCoroutine(float intensity, float time)
    {
        vCamA = GameObject.Find("vCam_Player")?.GetComponent<CinemachineVirtualCamera>();
        if (vCamA != null)
        {
            CinemachineBasicMultiChannelPerlin perlinA = vCamA.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlinA != null)
            {
                perlinA.m_FrequencyGain = intensity;
            }
        }

        vCamB = GameObject.Find("vCam_Hook")?.GetComponent<CinemachineVirtualCamera>();
        if (vCamB != null)
        {
            CinemachineBasicMultiChannelPerlin perlinB = vCamB.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlinB != null)
            {
                perlinB.m_FrequencyGain = intensity;
            }
        }

        yield return new WaitForSeconds(time);

        if (vCamA != null)
        {
            CinemachineBasicMultiChannelPerlin perlinA = vCamA.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlinA != null)
            {
                perlinA.m_FrequencyGain = 1f;
            }
        }

        if (vCamB != null)
        {
            CinemachineBasicMultiChannelPerlin perlinB = vCamB.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlinB != null)
            {
                perlinB.m_FrequencyGain = 1f;
            }
        }
    }
}
