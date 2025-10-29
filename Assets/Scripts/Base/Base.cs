using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private Storage _storage;
    [SerializeField] private Flag _flag;

    private readonly string _noResourceText = "не найдено ни одного ресурса";
    private readonly string _noRobotsText = "нет ни одного свободного робота";
    private readonly string _notEnoughRobotsText = "не хватает роботов для постройки базы";

    private UserInput _userInput;
    private ResourceTracker _resourceTracker;
    private ErrorViewer _errorViewer;
    private Base _thatBase;
    private bool _canBuildRobot;
    private List<Robot> _robots;
    
    private int _resourcesToBuild = 5;
    private int _resourcesToCreateRobot = 3;
    private int _startRobots = 3;
    private float _sleepTime = 0.5f;
    private WaitForSeconds _sleep;

    public Flag Flag => _flag;
    public Scanner Scanner => _scanner;

    public event Action<Vector3> RobotReachedFlag;
    
    private void OnEnable()
    {
        _scanner.ResourceFounded += SendRobotForResources;
        _robotSpawner.RobotCreated += SetupRobotForStorage;
        _storage.ResourceAdded += _resourceTracker.UnMarkTaked;
        _storage.CollectedToCreateRobot += CreateRobot;
        _userInput.PlacedFlag += StartSendRobotToFlag;
    }

    private void OnDisable()
    {
        _scanner.ResourceFounded -= SendRobotForResources;
        _robotSpawner.RobotCreated -= SetupRobotForStorage;
        _storage.ResourceAdded -= _resourceTracker.UnMarkTaked;
        _storage.CollectedToCreateRobot -= CreateRobot;
        _userInput.PlacedFlag -= StartSendRobotToFlag;

        foreach(Robot robot in _robots)
        {
            robot.ReachedFlag -= OnRobotReachedFlag;
        }
    }

    private void Awake()
    {
        _canBuildRobot = true;
        _sleep = new WaitForSeconds(_sleepTime);
        _thatBase = GetComponent<Base>();
        _robots = new List<Robot>();
    }

    private void Start()
    {
        for(int i = 0;i < _startRobots; i++)
        {
            _robotSpawner.Get();
        }
    }

    private void SetupRobotForStorage(Robot robot)
    {
        robot.SetStoragePosition(_storage.transform.position);

        _robots.Add(robot);
        robot.ReachedFlag += OnRobotReachedFlag;
    }

    private void StartSendRobotToFlag(Flag flag)
    {
        if (_userInput.ClickedBase != _thatBase)
            return;

        if (_robots.Count <= 1)
        {
            _errorViewer.ShowText(_notEnoughRobotsText);
            return;
        }

        _canBuildRobot = false;

        StartCoroutine(SendRobotToFlag(flag));
    }

    private IEnumerator SendRobotToFlag(Flag flag)
    {
        bool isChoosedRobot = false;

        while(_storage.ResourceCount < _resourcesToBuild)
        {
            yield return _sleep;
        }

        while(isChoosedRobot == false)
        {
            foreach (Robot robot in _robots)
            {
                if (robot.IsBusy)
                    continue;

                isChoosedRobot = true;
                _storage.DecreaseResources(_resourcesToBuild);
                flag.SetColliderSize(robot.GetDistanceErrorToFlag());
                robot.StartMoveToFlag(flag.transform.position);
                robot.SetFlag(flag);
                break;
            }

            yield return _sleep;
        }
    }

    private void CreateRobot()
    {
        if (_canBuildRobot == false)
            return;

        _storage.DecreaseResources(_resourcesToCreateRobot);
        _robotSpawner.Get();
    }

    private void OnRobotReachedFlag(Vector3 flagPosition)
    {
        _canBuildRobot = true;
        RobotReachedFlag?.Invoke(flagPosition);
    }

    public void SendRobotForResources(Resource resource)
    {
        if (resource == null)
        {
            _errorViewer.ShowText(_noResourceText);
            return;
        }

        foreach (Robot robot in _robots)
        {
            if(robot.IsBusy)
                continue;

            _resourceTracker.MarkTaked(resource);
            robot.StartMoveToResource(resource);

            return;
        }

        _errorViewer.ShowText(_noRobotsText);
    }

    public void SetupOnCreate(UserInput userInput, ErrorViewer errorViewer, ResourceTracker resourceTracker)
    {
        _userInput = userInput;
        _errorViewer = errorViewer;
        _resourceTracker = resourceTracker;
    }
}
