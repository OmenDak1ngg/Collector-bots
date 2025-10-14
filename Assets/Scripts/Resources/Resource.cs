using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    public Collider Collider { get; private set; }  
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
    }
}
