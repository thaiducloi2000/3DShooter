using UnityEngine;
using UnityEngine.Pool;
using static RootMotion.FinalIK.HitReaction;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int flySpeed;
    [SerializeField] private LayerMask hitLayer;
    private WeaponInGame weaponUse;
    private float currentFlySpeed = 0f;
    public IObjectPool<Bullet> pool;
    private Vector3 directionFly;
    bool isRelease = false;
    // Start is called before the first frame update

    private void OnEnable()
    {
        currentFlySpeed = 0f;
        Invoke(nameof(AutoRelease), 5f);
    }

    public void Setup(Vector3 direction, WeaponInGame weapon)
    {
        weaponUse = weapon;
        transform.forward = direction.normalized;
        directionFly = direction.normalized;
        currentFlySpeed = flySpeed;
        isRelease = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFlySpeed <= 0f) return;
        transform.position += directionFly * (currentFlySpeed * Time.deltaTime);
        CheckHit();
    }

    private void CheckHit()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f, hitLayer, QueryTriggerInteraction.Ignore))
        {
            switch (hit.collider.gameObject.layer)
            {
                case 9: // Hit Human
                    break;
                case 6: // Hit Ground
                    break;
            }
            weaponUse.SpawnHitVfx(hit.point, hit.normal);

            currentFlySpeed = 0;
            AutoRelease();
        }
    }

    private void AutoRelease()
    {
        if(isRelease) return;
        CancelInvoke(nameof(AutoRelease));
        pool.Release(this);
        isRelease = true;
    }
}
