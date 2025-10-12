using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    public bool IsResourceTaked {  get; private set; }

    public Collider Collider { get; private set; }  
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        IsResourceTaked = false;
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTaked()
    {
        IsResourceTaked = true;
    }

    public void SetUnTaked()
    {
        IsResourceTaked = false;
    }
}
