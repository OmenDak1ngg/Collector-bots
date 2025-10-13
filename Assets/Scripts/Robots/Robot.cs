using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RobotMover))]
[RequireComponent(typeof(ResourceGrabber))]
[RequireComponent(typeof(NavMeshAgent))]
public class Robot : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
    }

    public void SetBusy()
    {
        IsBusy = true;
    }

    public void SetUnbusy()
    {
        IsBusy = false;
    }
}