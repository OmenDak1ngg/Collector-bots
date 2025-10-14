using System;
using System.Collections;
using UnityEngine;

public class ResourceGrabber : MonoBehaviour
{
    [SerializeField] private ResourceCarryPoint _carryPoint;
    [SerializeField] private float _grabSpeed;
    
    private ResourceTracker _resourceTracker;
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
        StartCoroutine(GrabResource(resource.transform));
        
        resource.transform.parent = transform;
        resource.Rigidbody.isKinematic = true;
        resource.Collider.isTrigger = true;
        _carriedResource = resource;
    }

    private void StartPutResource(Vector3 storagePosition)
    {
        StartCoroutine(PutResourceToStorage(storagePosition));
    }

    private IEnumerator GrabResource(Transform resourceTranform)
    {   
        Coroutine coroutine = StartCoroutine(MoveResourceToPoint(_carryPoint.transform.position, resourceTranform));

        yield return coroutine;

        ResourceGrabbed?.Invoke();
    }

    private IEnumerator PutResourceToStorage(Vector3 storagePosition)
    {
        Coroutine coroutine = StartCoroutine(MoveResourceToPoint(storagePosition, _carriedResource.transform));

        yield return coroutine;

        _carriedResource.transform.parent = null;
        PuttedResource?.Invoke();
        _resourceTracker.UnMarkTaked(_carriedResource);
    }

    private IEnumerator MoveResourceToPoint(Vector3 pointPosition, Transform resourceTransform)
    {
        Vector3 resourcePosition = resourceTransform.position;

        while (resourcePosition != pointPosition)
        {
            resourcePosition = Vector3.MoveTowards(resourcePosition, pointPosition, _grabSpeed * Time.deltaTime);
            resourceTransform.position = resourcePosition;

            yield return null;
        }
    }

    public void SetResourceTracker(ResourceTracker tracker)
    {
        _resourceTracker = tracker;
    }
}
