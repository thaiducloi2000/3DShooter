using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraZoneSwitcher switcher;

    [SerializeField] private ThirdPersonController localPlayer; // Switch To Local Player later
    [SerializeField] private Transform aimPoint;
    [SerializeField] private LayerMask shootLayer;
    public Transform AimPoint => aimPoint;

    private PlayerInputHandler playerInputHandler;
    // Start is called before the first frame update

    private void Start()
    {
        switcher.SetupCamera(localPlayer.CinemachineCameraTarget.transform);
        playerInputHandler = localPlayer.PlayerInputHandler;
        playerInputHandler.AssignOnAimCallBack(switcher.SwitchToCamera);
        playerInputHandler.AssignOnShootCallBack(IsShooting);
    }

    #region Setup Camera
    public void SetupCamera(ThirdPersonController player)
    {
        localPlayer = player;
        switcher.SetupCamera(localPlayer.CinemachineCameraTarget.transform);
        playerInputHandler = localPlayer.PlayerInputHandler;
        playerInputHandler.AssignOnAimCallBack(switcher.SwitchToCamera);
        playerInputHandler.AssignOnShootCallBack(IsShooting);
    }

    #endregion

    #region Interactive Camera
    private void IsShooting(bool isShoot)
    {
        if (isShoot) 
        {
            InvokeRepeating(nameof(Shoot),0f,1f);
        }
        else
        {
            CancelInvoke(nameof(Shoot));
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position, aimPoint.position);
    }
#endif
}
