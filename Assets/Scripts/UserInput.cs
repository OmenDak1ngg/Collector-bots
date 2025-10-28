using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Flag _flag;

    private readonly KeyCode ScanKey = KeyCode.LeftAlt;
    private readonly KeyCode ClickKey = KeyCode.Mouse0;
    private readonly KeyCode CancelKey = KeyCode.Escape;

    private float _rayDistance = 500;
    private bool _clickedOnBase;
    public Base ClickedBase { get; private set; }

    public event Action Scanned;
    public event Action DisplayedFlag;
    public event Action HidedFlag;
    public event Action<Flag> PlacedFlag;

    private void Awake()
    { 
        _clickedOnBase = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(ScanKey))
        {
            Scanned?.Invoke();
        }

        if (Input.GetKeyDown(ClickKey) && GetMouseCollider() != null && GetMouseCollider().TryGetComponent<Base>(out Base clickedBase))
        {
            Debug.Log("i am waiting a place to flag");
            ClickedBase = clickedBase;
            _clickedOnBase = true;
            DisplayedFlag?.Invoke();
        }

        if(_clickedOnBase && Input.GetKeyDown(ClickKey) && GetMouseCollider() != null && GetMouseCollider().TryGetComponent<Base>(out _) == false)
        {
            _clickedOnBase = false;
            PlacedFlag?.Invoke(_flag);
            Debug.Log($"flag placed");
        }

        if(_clickedOnBase && Input.GetKeyDown(CancelKey))
        {
            _clickedOnBase = false;
            HidedFlag?.Invoke();
        }
    }

    private Collider GetMouseCollider()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, _rayDistance))
        {
            return hit.collider;
        }

        return null;
    }
}
