using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
   [SerializeField] private CinemachineImpulseSource impulseSource;
   [SerializeField] private float amplitude = 0.1f;
    // Call this method whenever you want to trigger the camera shake
    [Button("Camera shake")]
   public void TriggerShake()
    {
        impulseSource.GenerateImpulseWithForce(amplitude);
    }
}