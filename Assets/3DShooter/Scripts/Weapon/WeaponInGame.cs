using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class WeaponInGame : MonoBehaviour, IWeaponInGame
{
    [SerializeField] private GameObject muzzleFlashVFX;
    [SerializeField] private Vector3 recoilDirection;
    public Vector3 RecoilDirection => recoilDirection;
    [SerializeField] private float recoilDelta;
    public float RecoilDelta => recoilDelta;
    [SerializeField] private int maxAmmoPerAmplifier;
    [SerializeField] private float reloadSpeed;
    private int currentAmmo;
    public int CurrentAmmo => currentAmmo;

    private ThirdPersonController playerUse;
    private UnityAction outOfAmmoCallBack;

    public void Equip(ThirdPersonController player, Transform handlePoint, UnityAction OnOutOfAmmoCallBack = null)
    {
        playerUse = player;
        transform.parent = handlePoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        muzzleFlashVFX.gameObject.SetActive(false);
        currentAmmo = maxAmmoPerAmplifier;
        outOfAmmoCallBack = OnOutOfAmmoCallBack;
    }

    public void OnShoot()
    {
        if (currentAmmo <= 0) return;
        muzzleFlashVFX.gameObject.SetActive(false);
        muzzleFlashVFX.gameObject.SetActive(true);
        currentAmmo--;
        if (currentAmmo <= 0)
        {
            outOfAmmoCallBack?.Invoke();
            Invoke(nameof(Reload), reloadSpeed);
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmoPerAmplifier;
        muzzleFlashVFX.gameObject.SetActive(false);
        outOfAmmoCallBack?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzleFlashVFX.transform.position, muzzleFlashVFX.transform.position + muzzleFlashVFX.transform.forward.normalized * 30f);
    }
#endif
}
