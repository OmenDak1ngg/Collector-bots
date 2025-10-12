using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    protected ObjectPool<T> Pool;

    protected virtual void Awake()
    {
        Pool = new ObjectPool<T>(
            createFunc:() => OnInstantiate(),
            actionOnGet:(T pooledObject) => OnGet(pooledObject),
            actionOnRelease:(T pooledObject) => OnRelease(pooledObject),
            actionOnDestroy:(T pooledObject) => OnDestory(pooledObject)
            );
    }

    protected virtual T OnInstantiate()
    {
        return Instantiate(_prefab);
    }

    protected virtual void OnGet(T pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    protected virtual void OnRelease(T pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    protected virtual void OnDestory(T pooledObject)
    {
        Destroy(pooledObject);
    }

    protected virtual void Release(T pooledObject)
    {
        Pool.Release(pooledObject);
    }

    protected virtual void Get()
    {
        Pool.Get();
    }
}
