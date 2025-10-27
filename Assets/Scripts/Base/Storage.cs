using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Storage : MonoBehaviour
{
    private readonly string _errorText = "у вас не хватает ресурсов";
    
    public int ResourceCount { get; private set; }

    private ErrorViewer _errorViewer;
    private int _resourcesToCreateRobot = 3;

    public event Action<int> ResourceUpdated;
    public event Action<Resource> ResourceAdded;

    public event Action CollectedToCreateRobot;

    private void Awake()
    {
        ResourceCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource.Collected == true)
                return;

            resource.MarkCollected();
            resource.InvokeReachedStorage();
            AddResource(resource);
        }
    }

    public void DecreaseResources(int decreaseCount)
    {
        if(decreaseCount > ResourceCount)
        {
            _errorViewer.ShowText(_errorText);
            return;
        }

        ResourceCount -= decreaseCount;
        ResourceUpdated?.Invoke(ResourceCount);
    }

    private void AddResource(Resource resource)
    {
        ResourceCount++;
        ResourceUpdated?.Invoke(ResourceCount);
        ResourceAdded?.Invoke(resource);

        if(ResourceCount >= _resourcesToCreateRobot)
        {
            CollectedToCreateRobot?.Invoke();
        }
    }

    public void SetupOnCreate(ErrorViewer errorViewer)
    {
        _errorViewer = errorViewer;
    }
}