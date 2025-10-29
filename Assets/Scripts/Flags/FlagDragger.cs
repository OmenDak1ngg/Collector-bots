using System.Collections;
using UnityEngine;


public class FlagDragger : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private UserInput _userInput;

    private float _raycastLength = 10000;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _userInput.DisplayedFlag += StartDrag;
        _userInput.PlacedFlag += StopDrag;
    }

    private void OnDisable()
    {
        _userInput.DisplayedFlag -= StartDrag;
        _userInput.PlacedFlag += StopDrag;
    }

    private IEnumerator DragFlag(Flag flag)
    {
        while (enabled)
        {
            Vector3 flagPosition = GetMousePosition();
            Vector3 lookPosition = _camera.transform.position - flag.transform.position;

            flag.transform.LookAt(lookPosition);
            flag.transform.position = flagPosition;

            yield return null;
        }
    }

    private Vector3 GetMousePosition()
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

    public void StartDrag(Flag flag)
    {
        if (flag.IsFlagPlaced == true)
            return;

        _coroutine = StartCoroutine(DragFlag(flag));
    }

    public void StopDrag(Flag flag)
    {
        StopCoroutine(_coroutine);
    }
}