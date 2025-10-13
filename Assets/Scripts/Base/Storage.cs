using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private ErrorViewer _errorViewer;

    private readonly string _errorText = "у вас не хватает ресурсов";

    private int _resourceCount;

    public event Action<int> ResourceUpdated;
    public event Action<Resource> ResourceAdded;

    private void Awake()
    {
        _resourceCount = 0;
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
    }
}