using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class WeaponInGame : MonoBehaviour, IWeaponInGame
{
    [SerializeField] private GameObject muzzleFlashVFX;
    [SerializeField] private Transform targetPoint;
    public Transform TargetPoint => targetPoint;

    [Header("Weapon Setting")]
    [SerializeField] private Vector3 recoilDirection;
    public Vector3 RecoilDirection => recoilDirection;
    [SerializeField] private float recoilDelta;
    public float RecoilDelta => recoilDelta;
    [SerializeField] private int maxAmmoPerAmplifier;
    [SerializeField] private float reloadSpeed;

    [Header("Object To Pool")]
    [SerializeField] private Bullet bullet;
    private IObjectPool<Bullet> bulletPool;
    [SerializeField] private GameObject hitVfx;
    private IObjectPool<PoolElement> hitPool;

    private int currentAmmo;
    public int CurrentAmmo
    {
        get => currentAmmo;
        private set
        {
            currentAmmo = value;
            OnAmmoChangeCallBack?.Invoke(currentAmmo);
        }
    }

    private ThirdPersonController playerUse;
    private UnityAction outOfAmmoCallBack;
    private UnityAction<int> OnAmmoChangeCallBack;


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

    public void AddAmmoChangeListener(UnityAction<int> listener)
    {
        OnAmmoChangeCallBack += listener;
    }

    public void OnShoot()
    {
        if (currentAmmo <= 0) return;
        muzzleFlashVFX.gameObject.SetActive(false);
        muzzleFlashVFX.gameObject.SetActive(true);
        CurrentAmmo--;
        if (currentAmmo <= 0)
        {
            outOfAmmoCallBack?.Invoke();
            playerUse.Reload();
        }
    }

    public void Reload()
    {
        CurrentAmmo = maxAmmoPerAmplifier;
        muzzleFlashVFX.gameObject.SetActive(false);
        outOfAmmoCallBack?.Invoke();
    }
    #region Object Pool 
    //---- Hit Vfx
    private IObjectPool<PoolElement> HitPool
    {
        get
        {
            if (hitPool == null)
            {
                hitPool = new ObjectPool<PoolElement>(CreatePooledItemHit, OnTakeFromPool<PoolElement>, OnReturnedToPool<PoolElement>, OnDestroyPoolObject<PoolElement>, maxSize: 100);
            }
            return hitPool;
        }
    }

    public void SpawnHitVfx(Vector3 spawnPoint, Vector3 normal)
    {
        PoolElement hit = HitPool.Get();
        hit.transform.position = spawnPoint;
        hit.transform.up = normal;
    }

    //----- End Of Hit Vfx
    //----- Bullet
    private IObjectPool<Bullet> BulletPool
    {
        get
        {
            if (bulletPool == null)
            {
                bulletPool = new ObjectPool<Bullet>(CreatePooledItemBullet, OnTakeFromPool<Bullet>, OnReturnedToPool<Bullet>, OnDestroyPoolObject<Bullet>, maxSize: 100);
            }
            return bulletPool;
        }
    }
    public void SpawnBullet()
    {
        Bullet obj = BulletPool.Get();
        obj.transform.position = TargetPoint.position;
        obj.transform.forward = TargetPoint.forward.normalized;
        obj.Setup(this);
    }
    // ----- End Of Bullet
    // ----- private State
    private Bullet CreatePooledItemBullet()
    {
        Bullet obj = Instantiate(bullet);
        obj.pool = bulletPool;
        return obj;
    }

    private PoolElement CreatePooledItemHit()
    {
        GameObject hit = Instantiate(hitVfx);
        var fx = hit.AddComponent<PoolElement>();
        fx.pool = hitPool;
        return fx;
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool<T>(T obj) where T : Component
    {
        obj.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool<T>(T obj) where T : Component
    {
        obj.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject<T>(T obj) where T : Component
    {
        Destroy(obj.gameObject);
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzleFlashVFX.transform.position, muzzleFlashVFX.transform.position + muzzleFlashVFX.transform.forward.normalized * 30f);
    }
#endif
}
