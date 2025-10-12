using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private int _resourceCount;

    public event Action<int> ResourceUpdated;

    private void Start()
    {
        _resourceCount = 0;
    }

    public void IncreaseResources()
    {
        _resourceCount++;
        ResourceUpdated?.Invoke(_resourceCount);
    }

    public void DecreaseResources(int decreaseCount)
    {
        if(decreaseCount > _resourceCount)
        {
            Debug.Log("у вас не хватает ресурсов");
            return;
        }

        _resourceCount -= decreaseCount;
        ResourceUpdated?.Invoke(_resourceCount);
    }
}