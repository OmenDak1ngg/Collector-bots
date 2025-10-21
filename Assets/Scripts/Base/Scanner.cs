using System;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;

    private MessageViewer _messageViewer;
    private ResourceTracker _resourceTracker;
    private UserInput _userInput;

    private readonly string _scanStartedText = "скан начался";

    public event Action<Resource> ResourceFounded;

    private void OnEnable()
    {
        _userInput.Scanned += FindClosestResource;
    }

    private void OnDisable()
    {
        _userInput.Scanned -= FindClosestResource;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }

    public void FindClosestResource()
    {
        _messageViewer.ShowText(_scanStartedText);

        Collider[] objects = Physics.OverlapSphere(transform.position, _scanRadius);
        Resource closestResource = null;

        bool isResourceFirst = true;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].TryGetComponent<Resource>(out Resource resource) == false || _resourceTracker.IsResourceTaked(resource))
                continue;

            if (isResourceFirst)
            {
                closestResource = resource;
                isResourceFirst = false;

                continue;
            }

            if (Vector3.SqrMagnitude(transform.position - resource.transform.position) <
                Vector3.SqrMagnitude(transform.position - closestResource.transform.position))
            {
                closestResource = resource;
            }
        }

        ResourceFounded?.Invoke(closestResource);
    }

    public void SetupOnCreate(MessageViewer messageVievwer, ResourceTracker resourceTracker, UserInput userInput)
    {
        _messageViewer = messageVievwer;
        _resourceTracker = resourceTracker;
        _userInput = userInput;
    }
}
