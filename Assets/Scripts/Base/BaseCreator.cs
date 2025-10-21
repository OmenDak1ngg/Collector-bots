using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _prefab;
    [SerializeField] private UserInput _userInput;
    [SerializeField] private ErrorViewer _errorViewer;
    [SerializeField] private ResourceTracker _resourceTracker;

    [SerializeField] private MessageViewer _messageViewer;

    [SerializeField] private Camera _camera;

    [SerializeField] private NavMeshSurface _navMeshSurface;

    private Vector3 _startBasePos;

    private List<Base> _bases;

    private void OnDisable()
    {
        foreach(Base robotBase in _bases)
        {
            robotBase.RobotReachedFlag -= CreateBase;
        }
    }

    private void Awake()
    {
        _bases = new List<Base>();
        _startBasePos = Vector3.zero;
        CreateBase(_startBasePos);
    }

    public void CreateBase(Vector3 flagPosition)
    {
        Base newBase = Instantiate(_prefab);

        newBase.SetupOnCreate(_userInput, _errorViewer, _resourceTracker);
        newBase.Scanner.SetupOnCreate(_messageViewer, _resourceTracker, _userInput);
        
        newBase.GetComponentInChildren<SpawnZone>().SetupOnCreate(_errorViewer);
        newBase.GetComponentInChildren<RobotSpawner>().SetupOnCreate(_errorViewer);
        newBase.GetComponentInChildren<ResourceViewer>().SetupOnCreate(_camera);
        newBase.GetComponentInChildren<Storage>().SetupOnCreate(_errorViewer);  

        newBase.RobotReachedFlag += CreateBase;

        newBase.transform.position = flagPosition;
        newBase.transform.parent = this.transform;
      
        _navMeshSurface.BuildNavMesh();

        _bases.Add(newBase);
    }
}
