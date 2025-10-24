using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RobotMover))]
[RequireComponent(typeof(ResourceGrabber))]
[RequireComponent(typeof(NavMeshAgent))]

public class Robot : MonoBehaviour
{
    private Coroutine _coroutine;
    private Vector3 _storagePostion;
    private Vector3 _spawnpointPosition;

    private float _distanceErrorToFlag = 300f;

    public RobotMover Mover { get; private set; }   
    public ResourceGrabber ResourceGrabber { get; private set; }
    public bool IsBusy { get; private set; }

    public event Action<Resource> ReachedResource;
    public event Action<Vector3> ReachedStorage;
    public event Action<Vector3> ReachedFlag;

    private void OnEnable()
    {
        ResourceGrabber.ResourceGrabbed += StartMoveToStorage;
        ResourceGrabber.PuttedResource += StartMoveToSpawnpoint;
    }

    private void OnDisable()
    {
        ResourceGrabber.ResourceGrabbed -= StartMoveToStorage;
        ResourceGrabber.PuttedResource -= StartMoveToSpawnpoint;
    }

    private void Awake()
    {
        ResourceGrabber = GetComponent<ResourceGrabber>();
        Mover = GetComponent<RobotMover>();
        IsBusy = false;
    }

    private IEnumerator MoveToResource(Resource resource)
    {
        IsBusy = true;
        _coroutine = StartCoroutine(Mover.Move(resource.transform.position));

        yield return _coroutine;

        ReachedResource?.Invoke(resource);
    }

    private IEnumerator MoveToStorage()
    {
        _coroutine = StartCoroutine(Mover.Move(_storagePostion));

        yield return _coroutine;

        ReachedStorage?.Invoke(_storagePostion);
    }

    private IEnumerator MoveToSpawnpoint()
    {
        _coroutine = StartCoroutine(Mover.Move(_spawnpointPosition));

        yield return _coroutine;

        IsBusy = false;
    }

    private IEnumerator MoveToFlag(Vector3 flagPosition)
    {
        _coroutine = StartCoroutine(Mover.Move(flagPosition,_distanceErrorToFlag));
        IsBusy = true;

        yield return _coroutine;

        ReachedFlag?.Invoke(flagPosition);
        StartMoveToSpawnpoint();
    }

    public void StartMoveToResource(Resource resource)
    {
        StartCoroutine(MoveToResource(resource));
    }

    public void StartMoveToStorage()
    {
        StartCoroutine(MoveToStorage());
    }

    public void StartMoveToSpawnpoint()
    {
        StartCoroutine(MoveToSpawnpoint());
    }

    public void StartMoveToFlag(Vector3 flagPosition)
    {
        StartCoroutine(MoveToFlag(flagPosition));
    }

    public void SetStoragePosition(Vector3 storagePosition)
    {
        _storagePostion = storagePosition;
    }

    public void SetSpawnpointPosition(Vector3 spawnpointPosition)
    {
        _spawnpointPosition = spawnpointPosition;
    }

    public float GetDistanceErrorToFlag()
    {
        return _distanceErrorToFlag;
    }
}