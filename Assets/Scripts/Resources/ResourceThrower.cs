using UnityEngine;

public class ResourceThrower : MonoBehaviour
{
    [SerializeField] private float _force;

    [SerializeField,Range(-1,1)] private float _minSpreadX;
    [SerializeField, Range(-1, 1)] private float _maxSpreadX;

    [Min(1)]
    [SerializeField] private float _minSpreadY;
    [SerializeField] private float _maxSpreadY;

    [SerializeField, Range(-1, 1)] private float _minSpreadZ;
    [SerializeField, Range(-1, 1)] private float _maxSpreadZ;

    private Vector3 _throwDirection;

    private float _throwDirectionX;
    private float _throwDirectionY;
    private float _throwDirectionZ;

    private void Start()
    {
        _minSpreadX = Mathf.Min(_minSpreadX, _maxSpreadX);
        _maxSpreadX = Mathf.Max(_maxSpreadX, _minSpreadX);

        _minSpreadZ = Mathf.Min(_minSpreadZ, _maxSpreadZ);
        _maxSpreadZ = Mathf.Max(_maxSpreadX, _minSpreadX);
    }

    public void Throw(Resource resource)
    {
        _throwDirectionX = Random.Range(_minSpreadX, _maxSpreadX);
        _throwDirectionY = Random.Range(_minSpreadY, _maxSpreadY);
        _throwDirectionZ = Random.Range(_minSpreadZ, _maxSpreadZ);

        _throwDirection = new Vector3(_throwDirectionX, _throwDirectionY, _throwDirectionZ);

        resource.Rigidbody.AddForce(_throwDirection * _force, ForceMode.Impulse);
    }
}