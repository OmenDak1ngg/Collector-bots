using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;

    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private UserInput _userInput;

    private void OnEnable()
    {
        _userInput.Scanned += SendRobotForResources;
    }

    private void OnDisable()
    {
        _userInput.Scanned -= SendRobotForResources;
    }

    public void SendRobotForResources()
    {
        List<Robot> robots = _robotSpawner.AvalaibleRobots;

        Resource closestResource = _scanner.GetClosestResource();

        if (closestResource == null)
        {
            Debug.Log("не найдено ни одного ресурса");
            return;
        }

        foreach (Robot robot in robots)
        {
            if(robot.IsBusy)
                continue;

            robot.GetComponent<RobotMover>().StartMoveToResource(closestResource);

            robot.SetBusy();
            return;
        }

        Debug.Log("нет ни одного свободного робота");
    }
}
