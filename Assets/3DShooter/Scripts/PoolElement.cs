using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolElement : MonoBehaviour
{
    public IObjectPool<PoolElement> pool;
    [SerializeField] private float timeToRelease = 3f;

    private void OnEnable()
    {
        Invoke(nameof(ReleaseToPool), timeToRelease);
    }

    private void ReleaseToPool()
    {
        pool.Release(this);
    }
}
