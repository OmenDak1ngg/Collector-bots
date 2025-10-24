using System;
using System.Collections;
using UnityEngine;

public class FlagDisplayer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Flag _prefab;
    [SerializeField] private UserInput _userInput;
    [SerializeField] private Flag _flag;

    private bool _isFlagPlaced;

    private float _raycastLength = 500;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _userInput.DisplayedFlag += StartDisplayFlag;
        _userInput.HidedFlag += StopDisplayFlag;
        _userInput.PlacedFlag += _ => PlaceFlag();
        _flag.RobotArrived += OnRobotArrived;
    }

    private void OnDisable()
    {
        _userInput.DisplayedFlag -= StartDisplayFlag;
        _userInput.HidedFlag -= StopDisplayFlag;
        _userInput.PlacedFlag -= _ => PlaceFlag();
        _flag.RobotArrived -= OnRobotArrived;
    }

    private void Awake()
    {
        _isFlagPlaced = false;
    }

    private void OnRobotArrived()
    {
        if (_isFlagPlaced == false)
            return;

        _isFlagPlaced = false;
        StopDisplayFlag();
    }

    private void StartDisplayFlag()
    {
        if (_isFlagPlaced)
            return;

        _flag.gameObject.SetActive(true);
        _coroutine = StartCoroutine(DisplayFlag());
    }

    private void StopDisplayFlag()
    {
        if (_coroutine == null)
            return;

        _flag.gameObject.SetActive(false);
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator DisplayFlag()
    {
        while (enabled)
        {
            if(GetFlagPosition() != Vector3.zero)
            {
                Vector3 lookPosition = _camera.transform.position - _flag.transform.position;

                _flag.transform.LookAt(lookPosition);
                _flag.transform.position = GetFlagPosition();
            }

            yield return null;
        }
    }

    private Vector3 GetFlagPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _raycastLength, _groundLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void PlaceFlag()
    {
        _isFlagPlaced = true;
        StopCoroutine(_coroutine);
    }
}