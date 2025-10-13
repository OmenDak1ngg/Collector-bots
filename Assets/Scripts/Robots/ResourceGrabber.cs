using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGrabber : MonoBehaviour
{
    [SerializeField] private ResourceCarryPoint _carryPoint;

    [SerializeField] private float _grabSpeed;

    private RobotMover _mover;

    private Resource _carriedResource;

    public event Action ResourceGrabbed;
    public event Action PuttedResource;

    private void OnEnable()
    {
        _mover.ReachedResource += StartGrabResource;
        _mover.ReachedStorage += StartPutResource;
    }

    private void OnDisable()
    {
        _mover.ReachedResource -= StartGrabResource;
        _mover.ReachedStorage -= StartPutResource;
    }

    private void Awake()
    {
        _mover = GetComponent<RobotMover>();
    }

    private void StartGrabResource(Resource resource)
    {
       StartCoroutine(GrabResource(resource));
    }

    private void StartPutResource(Storage storage)
    {
        StartCoroutine(PutResourceToStorage(storage));
    }

    private IEnumerator GrabResource(Resource resource)
    {
        resource.transform.parent = transform;
        resource.Rigidbody.isKinematic = true;
        resource.Collider.isTrigger = true;
        
        Coroutine coroutine = StartCoroutine(MoveResourceToPoint(_carryPoint.GameObject(), resource));

        yield return coroutine;

        _carriedResource = resource;
        ResourceGrabbed?.Invoke();
    }

    private IEnumerator PutResourceToStorage(Storage storage)
    {
        Coroutine coroutine = StartCoroutine(MoveResourceToPoint(storage.GameObject(), _carriedResource));

        yield return coroutine;

        _carriedResource.transform.parent = null;
        storage.AddResource(_carriedResource);
        PuttedResource?.Invoke();
    }

    private IEnumerator MoveResourceToPoint(GameObject point, Resource resource)
    {
        Vector3 resourcePosition = resource.transform.position;

        while (resourcePosition != point.transform.position)
        {
            resourcePosition = Vector3.MoveTowards(resourcePosition, point.transform.position, _grabSpeed * Time.deltaTime);
            resource.transform.position = resourcePosition;

            yield return null;
        }
    }
}
