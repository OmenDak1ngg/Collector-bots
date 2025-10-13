using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private ResourceThrower _thrower;

    [SerializeField] private int _maxResourcesCount = 3;
    [SerializeField] private float _delay;

    [SerializeField] private Storage _storage;

    private int _currentResourcesCount;
    private WaitForSeconds _waitTime;

    private void OnEnable()
    {
        _storage.ResourceAdded += OnRelease;
    }

    private void OnDisable()
    {
        _storage.ResourceAdded -= OnRelease;
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

    protected override void Awake()
    {
        base.Awake();

        _currentResourcesCount = 0;
        _waitTime = new WaitForSeconds(_delay);
    }

    protected override void OnGet(Resource pooledObject)
    {
        base.OnGet(pooledObject);

        _currentResourcesCount += 1;
        pooledObject.transform.position = transform.position;
        _thrower.Throw(pooledObject);
    }

    protected override void OnRelease(Resource pooledObject)
    {
        base.OnRelease(pooledObject);
        pooledObject.SetUnTaked();
        pooledObject.Rigidbody.isKinematic = false;
        _currentResourcesCount -= 1;
    }
}
