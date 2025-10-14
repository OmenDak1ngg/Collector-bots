using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceError;
    
    private ResourceTracker _resourceTracket;
    private Storage _Storage;
    private ResourceGrabber _grabber;
    private NavMeshAgent _agent;
    private Vector3 _spawnpoint;

    public event Action<Storage> ReachedStorage;
    public event Action<Resource> ReachedResource;

    private void OnEnable()
    {
        _grabber.ResourceGrabbed += StartMoveToStorage;
        _grabber.PuttedResource += StartMoveToSpawn;
    }

    private void OnDisable()
    {
        _grabber.ResourceGrabbed -= StartMoveToStorage;
        _grabber.PuttedResource -= StartMoveToSpawn;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _grabber = GetComponent<ResourceGrabber>();
    }


    private IEnumerator MoveToResource(Resource resource)
    {
        Coroutine coroutine = StartCoroutine(Move(resource.GameObject()));

        yield return coroutine;

        ReachedResource?.Invoke(resource);
    }

    private void StartMoveToStorage()
    {
        StartCoroutine(MoveToStorage());
    }

    private IEnumerator MoveToStorage()
    {
        Coroutine coroutine = StartCoroutine(Move(_Storage.GameObject()));

        yield return coroutine;

        ReachedStorage?.Invoke(_Storage);
    }

    private void StartMoveToSpawn()
    {
        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator MoveToSpawn()
    {
        Coroutine coroutine = StartCoroutine(Move(_spawnpoint));

        yield return coroutine;

        GetComponent<Robot>().UnMarkUnbusy();
    }


    private IEnumerator Move(GameObject gameObject)
    {
        Vector3 targetPosition = gameObject.transform.position;

        targetPosition.y = transform.position.y;

        _agent.SetDestination(targetPosition);

        while (Vector3.SqrMagnitude(transform.position - targetPosition) > _distanceError)
            yield return null;

        _agent.ResetPath();
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);

        while (Vector3.SqrMagnitude(transform.position - targetPosition) > _distanceError)
            yield return null;

        _agent.ResetPath();
    }

    public void SetSpawnpoint(Vector3 spawnpoint)
    {
        _spawnpoint = spawnpoint;
    }

    public void SetResourceTracket(ResourceTracker tracker)
    {
        _resourceTracket = tracker;
    }

    public void StartMoveToResource(Resource resource)
    {
        _resourceTracket.MarkTaked(resource);
        StartCoroutine(MoveToResource(resource));
    }

    public void SetStorage(Storage Storage)
    {
        _Storage = Storage;
    }
}
