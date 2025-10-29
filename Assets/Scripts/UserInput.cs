using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _baseLayerMask;

    private readonly KeyCode ScanKey = KeyCode.LeftAlt;
    private readonly KeyCode ClickKey = KeyCode.Mouse0;
    private readonly KeyCode CancelKey = KeyCode.Escape;

    private Flag _baseFlag;
    private float _rayDistance = 500;
    private bool _clickedOnBase;

    public Base ClickedBase { get; private set; }

    public event Action Scanned;
    public event Action<Flag> DisplayedFlag;
    public event Action<Flag> HidedFlag;
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

        if (Input.GetKeyDown(ClickKey) &&  GetMouseCollider() != null && GetMouseCollider().TryGetComponent<Base>(out Base clickedBase))
        {
            ClickedBase = clickedBase;
            _baseFlag = clickedBase.Flag;

            _clickedOnBase = true;
            DisplayedFlag?.Invoke(_baseFlag);
        }

        if(_clickedOnBase && Input.GetKeyDown(ClickKey) && GetMouseCollider() == null)
        {
            _baseFlag.MarkPlaced();
            _clickedOnBase = false;
            PlacedFlag?.Invoke(_baseFlag);
        }

        if(_clickedOnBase && Input.GetKeyDown(CancelKey))
        {
            ClickedBase.Flag.UnMarkFlagPlaced();
            _clickedOnBase = false;
            HidedFlag?.Invoke(_baseFlag);
        }
    }

    private Collider GetMouseCollider()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, _rayDistance, _baseLayerMask))
        {
            return hit.collider;
        }

        return null;
    }
}
