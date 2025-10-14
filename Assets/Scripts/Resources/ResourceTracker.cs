using System.Collections.Generic;
using UnityEngine;

public class ResourceTracker : MonoBehaviour
{
    private List<Resource> _takedResource;

    private void Awake()
    {
        _takedResource = new List<Resource>();
    }

    public bool IsResourceTaked(Resource resource)
    {
        if (_takedResource.Contains(resource))
            return true;

        return false;
    }

    public void MarkTaked(Resource resours)
    {
        _takedResource.Add(resours);
    }

    public void UnMarkTaked(Resource resource)
    {
        _takedResource.Remove(resource);
    }
}