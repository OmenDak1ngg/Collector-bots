using System;
using System.Collections;
using UnityEngine;

public class ResourceGrabber : MonoBehaviour
{
    [SerializeField] private ResourceCarryPoint _carryPoint;
    [SerializeField] private float _grabSpeed;
    
    private Resource _carriedResource;
    private Robot _robot;

    public event Action ResourceGrabbed;
    public event Action PuttedResource;

    private void OnEnable()
    {
        _robot.ReachedResource += StartGrabResource;
        _robot.ReachedStorage += StartPutResource;
    }

    private void OnDisable()
    {
        _robot.ReachedResource -= StartGrabResource;
        _robot.ReachedStorage -= StartPutResource;
    }

    private void Awake()
    {
        _robot = GetComponent<Robot>();
    }

    private void StartGrabResource(Resource resource)
    {
        _carriedResource = resource;
        StartCoroutine(GrabResource(resource.transform));
        
        resource.transform.parent = transform;
        resource.Rigidbody.isKinematic = true;
        resource.Collider.isTrigger = true;
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
    }

    private IEnumerator MoveResourceToPoint(Vector3 pointPosition, Transform resourceTransform)
    {
        _robot.Mover.SetZeroVelocity();

        Vector3 resourcePosition = resourceTransform.position;

        while (resourcePosition != pointPosition)
        {
            if (_carriedResource.Collected)
                break;

            resourcePosition = Vector3.MoveTowards(resourcePosition, pointPosition, _grabSpeed * Time.deltaTime);
            resourceTransform.position = resourcePosition;

            yield return null;
        }

        _robot.Mover.ResetVelocity();
    }
}
