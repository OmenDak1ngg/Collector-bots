using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Storage : MonoBehaviour
{
    private readonly string _errorText = "у вас не хватает ресурсов";

    private ErrorViewer _errorViewer;
    private int _resourcesToCreateRobot = 3;
    private int _resourceCount;

    public event Action<int> ResourceUpdated;
    public event Action<Resource> ResourceAdded;

    public event Action CollectedThreeResources;

    private void Awake()
    {
        _resourceCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
        {
            resource.InvokeReachedStorage();
            AddResource(resource);
        }
    }

    public void DecreaseResources(int decreaseCount)
    {
        if(decreaseCount > _resourceCount)
        {
            _errorViewer.ShowText(_errorText);
            return;
        }

        _resourceCount -= decreaseCount;
        ResourceUpdated?.Invoke(_resourceCount);
    }

    public void AddResource(Resource resource)
    {
        _resourceCount++;
        ResourceUpdated?.Invoke(_resourceCount);
        ResourceAdded?.Invoke(resource);

        if(_resourceCount >= _resourcesToCreateRobot)
        {
            CollectedThreeResources?.Invoke();
            DecreaseResources(_resourcesToCreateRobot);
        }
    }

    public void SetupOnCreate(ErrorViewer errorViewer)
    {
        _errorViewer = errorViewer;
    }
}