using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RobotMover))]
[RequireComponent(typeof(ResourceGrabber))]
[RequireComponent(typeof(NavMeshAgent))]
public class Robot : MonoBehaviour
{
    public RobotMover Mover { get; private set; }   
    public ResourceGrabber ResourceGrabber { get; private set; }
    public bool IsBusy { get; private set; }

    private void Awake()
    {
        ResourceGrabber = GetComponent<ResourceGrabber>();
        Mover = GetComponent<RobotMover>();
        IsBusy = false;
    }

    public void MarkBusy()
    {
        IsBusy = true;
    }

    public void UnMarkUnbusy()
    {
        IsBusy = false;
    }

    public void MoveToResource(Resource resource)
    {
        Mover.StartMoveToResource(resource);
    }
}