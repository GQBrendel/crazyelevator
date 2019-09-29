﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    [SerializeField] private GameObject _waitPositionsRef;

    public SpawnPosition[] _spawnPositions;
    public WaitPosition[] _waitPositions;
    public SpawnPosition DespawnPosition { get { return _spawnPositions[0]; } }
    private ElevatorController _elevator;
    public int Index { get; set; }

    private float _userToleranceTime = 15f;
    private float _userDieTime = 7f;

    private List<UserBase> _users = new List<UserBase>();

    private Coroutine _userTolerance;

    public void InsertUser(UserBase userToAdd)
    {
        userToAdd.OnReachedDestination += HandleUserReachedDestination;
        _users.Add(userToAdd);
    }

    public int ListSize()
    {
        return _users.Count;
    }

    private User _impatientUser;

    private void Awake()
    {
        _spawnPositions = GetComponentsInChildren<SpawnPosition>();
        _waitPositions = _waitPositionsRef.GetComponentsInChildren<WaitPosition>();
        _elevator = FindObjectOfType<ElevatorController>();
        _elevator.OnElevatorCleared += HandleClearFloor;
    }

    private void HandleClearFloor()
    {
        _users.Clear();
        for (int i = 0; i < _waitPositions.Length; i++)
        {
            _waitPositions[i].IsFree = true;
            _spawnPositions[i].IsFree = true;
        } 
    }

    private IEnumerator UserToleranceCoroutine()
    {
        yield return new WaitForSeconds(_userToleranceTime);
        if (_impatientUser == null)
        {
            yield break;
        }
        _impatientUser.StartImpatientState();
        yield return new WaitForSeconds(_userDieTime);
        if (_impatientUser == null)
        {
            yield break;
        }
        _impatientUser.RunToElevator();
        _users.Remove(_users[0]);
        StartCoroutine(DelayUntilMove());
        _impatientUser = null;

    }

    private void HandleUserReachedDestination(UserBase user)
    {
        if (user == _users[0])
        {
            _impatientUser = user as User;
            _userTolerance = StartCoroutine(UserToleranceCoroutine());
        }
        if (!_elevator.HasRoom)
        {
            return;
        }
        if (_elevator.CurrentFlootIndex != Index)
        {
            return;
        }
        if (_users.Count == 0)
        {
            return;
        }

        if (user == _users[0])
        {
            ElevatorStoped();
        }
    }

    public void ElevatorStoped()
    {
        StartCoroutine(ElevatorStopedCoroutine());
    }
    private IEnumerator ElevatorStopedCoroutine()
    {
        yield return new WaitForSeconds(1f);

        if (!_elevator.HasRoom)
        {
            yield break;
        }
        if (!_elevator.IsStopedOnTheFloor(Index))
        {
            yield break;
        }
        if (_users.Count == 0)
        {
            yield break;
        }

        _impatientUser = null;
        if (_userTolerance != null)
        {
            StopCoroutine(_userTolerance);
        }

        User firstInLine = _users[0] as User;
      
        if (!firstInLine.ReadyToEnterInElevator())
        {
            yield break;
        }
        firstInLine.MoveToElevator();
        _users.Remove(_users[0]);

        StartCoroutine(DelayUntilMove());

    } 

    private IEnumerator DelayUntilMove()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _users.Count; i++)
        {
            Debug.Log("Count " + _users.Count);
            User currentUser = _users[i] as User;
            currentUser.MoveToWaitPos(_waitPositions[i]);
            _waitPositions[i].IsFree = false;
            _spawnPositions[i].IsFree = false;
        }
        _waitPositions[_users.Count].IsFree = true;
        _spawnPositions[_users.Count].IsFree = true;
    }

    public SpawnPosition RunnerSpawnPos
    {
        get
        {
            return _spawnPositions[2];
        }

    }

    public SpawnPosition SpawnPos
    {
        get
        {
            for (int i = 0; i < _spawnPositions.Length; i++)
            {
                if (_spawnPositions[i].IsFree)
                {
                    _spawnPositions[i].IsFree = false;
                    return _spawnPositions[i];
                }
            }
            Debug.LogError("Did not found any free Spaw position");

            return null;
        }
    }
    public WaitPosition WaitPos
    {
        get
        {
            for (int i = 0; i < _waitPositions.Length; i++)
            {
                if (_waitPositions[i].IsFree)
                {
                    _waitPositions[i].IsFree = false;
                    return _waitPositions[i];
                }
            }
            Debug.LogError("Did not found any free Wait position");

            return null;
        }
    }
}