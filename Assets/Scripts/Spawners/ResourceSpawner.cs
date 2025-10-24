using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private ResourceThrower _thrower;

    [SerializeField] private int _maxResourcesCount = 3;
    [SerializeField] private float _delay;

    private int _currentResourcesCount;
    private WaitForSeconds _waitTime;

    protected override void Awake()
    {
        base.Awake();

        _currentResourcesCount = 0;
        _waitTime = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (enabled)
        {
            if (_currentResourcesCount < _maxResourcesCount)
                Pool.Get();

            yield return _waitTime;
        }
    }

    protected override Resource OnInstantiate()
    {
        Resource resource = base.OnInstantiate();
        resource.ReachedStorage += Release;

        return resource;
    }

    protected override void OnGet(Resource pooledObject)
    {
        _currentResourcesCount += 1;
        pooledObject.transform.position = transform.position;
        _thrower.Throw(pooledObject);
        base.OnGet(pooledObject);
    }

    protected override void OnRelease(Resource pooledObject)
    {
        base.OnRelease(pooledObject);
        pooledObject.Rigidbody.isKinematic = false;
        _currentResourcesCount -= 1;
    }

    protected override void OnDestory(Resource pooledObject)
    {
        pooledObject.ReachedStorage -= Release; 

        base.OnDestory(pooledObject);
    }
}
