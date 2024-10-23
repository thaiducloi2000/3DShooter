using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class CameraZoneSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    private void SwitchToCamera(string targetCameraName) 
    {
        foreach (var camera in virtualCameras)
        {
            camera.enabled = camera.tag == targetCameraName;
        }
    }
 
    [Button("SecondaryVirtualCamera")]
    public void SecondaryVirtualCamera()
    {
          SwitchToCamera("SecondaryVirtualCamera");
    }

    [Button("PrimaryVirtualCamera")]
    public void PrimaryVirtualCamera()
    {
          SwitchToCamera("PrimaryVirtualCamera");
    }
}
