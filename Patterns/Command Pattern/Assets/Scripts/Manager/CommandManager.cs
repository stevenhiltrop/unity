using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandManager : MonoBehaviour
{
    private static CommandManager _instance;
    private List<ICommand> _commandBuffer = new List<ICommand>();

    public static CommandManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The CommandManager is NULL.");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void AddCommand(ICommand command)
    {
        _commandBuffer.Add(command);
    }

    #region Play
    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        Debug.Log("Playing..");

        foreach (var command in _commandBuffer)
        {
            command.Execute();
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Finished");
    }
    #endregion

    #region Rewind
    public void Rewind()
    {
        StartCoroutine(RewindRoutine());
    }

    IEnumerator RewindRoutine()
    {
        Debug.Log("Rewinding...");

        foreach (var command in Enumerable.Reverse(_commandBuffer))
        {
            command.Undo();
        }
        yield return new WaitForSeconds(1.0f);
    }
    #endregion

    #region Done
    public void Done()
    {
        var cubes = GameObject.FindGameObjectsWithTag("Cube");

        foreach (var cube in cubes)
        {
            cube.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
    #endregion

    #region Reset
    public void Reset()
    {
        _commandBuffer.Clear();
    }
    #endregion
}
