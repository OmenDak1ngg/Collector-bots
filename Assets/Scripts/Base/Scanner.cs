using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private MessageViewer _messageViewer;

    private readonly string _scanStartedText = "скан начался";

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }

    public Resource GetClosestResource()
    {
        _messageViewer.ShowText(_scanStartedText);

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
