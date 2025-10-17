using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _prefab;
    

    public void CreateBase(Vector3 flagPosition)
    {
        Base newBase = Instantiate(_prefab);
        
        newBase.transform.position = flagPosition;
    }
}
