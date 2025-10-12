using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }

    public Resource GetClosestResource()
    {
        Debug.Log("скан начался");
        Collider[] objects = Physics.OverlapSphere(transform.position, _scanRadius);
        Resource closestResource = null;

        bool isResourceFirst = true;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].TryGetComponent<Resource>(out Resource resource) == false || resource.IsResourceTaked)
                continue;

            if (isResourceFirst)
            {
                closestResource = resource;
                continue;
            }

            isResourceFirst = false;

            if (Vector3.SqrMagnitude(transform.position - resource.transform.position) <
                Vector3.SqrMagnitude(transform.position - closestResource.transform.position))
            {
                closestResource = resource;
            }
        }

        return closestResource;
    }
}
