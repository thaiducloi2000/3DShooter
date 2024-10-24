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
    bool isRelease = false;
    // Start is called before the first frame update

    private void OnEnable()
    {
        currentFlySpeed = 0f;
        Invoke(nameof(AutoRelease), 5f);
    }

    public void Setup(WeaponInGame weapon)
    {
        weaponUse = weapon;
        currentFlySpeed = flySpeed;
        isRelease = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentFlySpeed <= 0f) return;
        transform.position += transform.forward * currentFlySpeed * Time.deltaTime;
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
