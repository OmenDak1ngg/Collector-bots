using System.Collections;
using UnityEngine;

public class FlagDisplayer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Flag _prefab;
    [SerializeField] private float _delay;
    [SerializeField] private UserInput _userInput;

    private WaitForSeconds _delayWait;
    private float _raycastLength = 500;
    private Flag _flag;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _userInput.DisplayedFlag += StartDisplayFlag;
        _userInput.HidedFlag += StopDisplayFlag;
    }

    private void OnDisable()
    {
        _userInput.DisplayedFlag -= StartDisplayFlag;
        _userInput.HidedFlag -= StopDisplayFlag;
    }

    private void Awake()
    {
        _flag = Instantiate(_prefab);
        _flag.gameObject.SetActive(false);
        _delayWait = new WaitForSeconds(_delay);
    }

    private void StartDisplayFlag()
    {
        Debug.Log("startedDisplay");
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
            Debug.Log("displaing");
            _flag.transform.position = GetFlagPosition();

            yield return _delayWait;
        }
    }

    private Vector3 GetFlagPosition()
    {
        Debug.Log("getted POs");

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _raycastLength ,_groundLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}