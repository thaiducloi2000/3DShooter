using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class CameraZoneSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras = new CinemachineVirtualCamera[2];
    private CinemachineVirtualCamera currentCamera;

    private void OnEnable()
    {
        foreach (var cam in virtualCameras)
        {
            cam.gameObject.SetActive(cam.Follow != null ? true : false);
        }
    }

    public void SwitchToCamera(bool isMainCamera)
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 0;
        }

        int nextIndex = isMainCamera ? 1 : 0;

        currentCamera = virtualCameras[nextIndex];

        currentCamera.Priority = 10;
    }

    public void SetupCamera(Transform followTransform)
    {
        foreach (var cam in virtualCameras)
        {
            cam.Follow = followTransform;
            cam.gameObject.SetActive(true);
        }
    }

#if UNITY_EDITOR
    #region Test Feature
    [Button("SecondaryVirtualCamera")]
    public void SecondaryVirtualCamera()
    {
        SwitchToCamera(false);
    }

    [Button("PrimaryVirtualCamera")]
    public void PrimaryVirtualCamera()
    {
        SwitchToCamera(true);
    }
    #endregion
#endif
}
