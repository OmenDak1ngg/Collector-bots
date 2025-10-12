using UnityEngine;

public class ResourceGroundDetector : MonoBehaviour
{
    [SerializeField] private Resource _resource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Ground>(out _))
        {
            _resource.Rigidbody.isKinematic = true;
        }
    }
}