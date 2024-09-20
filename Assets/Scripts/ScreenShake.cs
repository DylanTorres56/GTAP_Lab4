using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera cVC;
    [SerializeField] CinemachineBasicMultiChannelPerlin cBMCP;

    // Awake is called on the first active frame
    void Awake()
    {
        cVC = GetComponent<CinemachineVirtualCamera>();
        cBMCP = cVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // As an object collides, adjust the amplitude of the Virtual Camera by a set amount for a set duration.
    public void CamShake(float shakeIntensity, float shakeLength) 
    {
        cBMCP.m_AmplitudeGain = shakeIntensity;
        Invoke("ShakeEnd", shakeLength);
    }

    // Once the camera has shaken enough, reset it to neutral.
    public void ShakeEnd() 
    {
        cBMCP.m_AmplitudeGain = 0f;
    }

}
