using Cinemachine;
using Sirenix.OdinInspector;
using StarterAssets;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraZoneSwitcher switcher;

    [SerializeField] private ThirdPersonController localPlayer; // Switch To Local Player later
    [SerializeField] private Transform aimPoint;
    [SerializeField] private LayerMask shootLayer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private Vector3 recoilDirection;
    private float recoilDelta;

    private Vector3 GetRandomShake => new Vector3(Random.Range(-recoilDirection.x, recoilDirection.x), Random.Range(-recoilDirection.y, recoilDirection.y), Random.Range(-recoilDirection.z, recoilDirection.z));
    public Transform AimPoint => aimPoint;

    private PlayerInputHandler playerInputHandler;

    #region Setup Camera
    public void SetupCamera(ThirdPersonController player)
    {
        localPlayer = player;
        switcher.SetupCamera(localPlayer.CinemachineCameraTarget.transform);
        playerInputHandler = localPlayer.PlayerInputHandler;
        playerInputHandler.AssignOnAimCallBack(switcher.SwitchToCamera);
        playerInputHandler.AssignOnShootCallBack(IsShooting);
        recoilDirection = player.CurrentWeapon.RecoilDirection;
        recoilDelta = player.CurrentWeapon.RecoilDelta;
    }

    #endregion

    #region Interactive Camera
    private void IsShooting(bool isShoot)
    {
        if (isShoot)
        {
            StartShoot();
            return;
        }
        CancelInvoke(nameof(Shoot));
    }

    public void StartShoot()
    {
        if( playerInputHandler.IsShoot && localPlayer.CurrentWeapon.CurrentAmmo > 0)
        {
            InvokeRepeating(nameof(Shoot), 0f, .1f);
            return;
        }
        CancelInvoke(nameof(Shoot));
    }

    [Button("Shoot")]
    private void Shoot()
    {
        if (localPlayer.CurrentWeapon.CurrentAmmo <= 0)
        {
            CancelInvoke(nameof(Shoot));
            return;
        }

        TriggerShake();
        localPlayer.CurrentWeapon.OnShoot();

        // if target is in range shoot, deal damage else spawn bullet
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, aimPoint.localPosition.z, shootLayer, QueryTriggerInteraction.Ignore))
        {
            switch (hit.collider.gameObject.layer)
            {
                case 9: // Hit Human
                    break;
                case 6: // Hit Ground
                    break;
            }
            localPlayer.CurrentWeapon.SpawnHitVfx(hit.point,hit.normal);
        }
        else
        {
            localPlayer.CurrentWeapon.SpawnBullet();
        }
    }

    private void TriggerShake()
    {
        impulseSource.m_DefaultVelocity = GetRandomShake;
        impulseSource.GenerateImpulseWithForce(recoilDelta);
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
