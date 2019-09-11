using System.Collections;
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

    private List<UserBase> _users = new List<UserBase>();

    public void InsertUser(UserBase userToAdd)
    {
        userToAdd.OnReachedDestination += HandleUserReachedDestination;
        _users.Add(userToAdd);
        if(_users.Count == 4)
        {
            StartCoroutine(WarningTime());
        }
    }

    private IEnumerator WarningTime()
    {
        yield return new WaitForSeconds(10f);
        if (_users.Count == 4)
        {
            _users[0].raiva.SetActive(true);
            _users[1].raiva.SetActive(true);

            StartCoroutine(GameOverTime());
        }
    }
    private IEnumerator GameOverTime()
    {
        yield return new WaitForSeconds(5f);
        if (_users.Count == 4)
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    public int ListSize()
    {
        return _users.Count;
    }
    private void HandleUserReachedDestination(UserBase user)
    {
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
//        StartCoroutine(HandleUserReachedDestinationCoroutine(user));
    }
    private IEnumerator HandleUserReachedDestinationCoroutine(User user)
    {
        yield return new WaitForSeconds(1f);

        if (_elevator.CurrentFlootIndex != Index)
        {
            yield break;
        }
        if (_users.Count == 0)
        {
            yield break;
        }

        if (user == _users[0])
        {
            ElevatorStoped();
        }
    }

    private void Awake()
    {
        _spawnPositions = GetComponentsInChildren<SpawnPosition>();
        _waitPositions = _waitPositionsRef.GetComponentsInChildren<WaitPosition>();
        _elevator = FindObjectOfType<ElevatorController>();
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
        User firstInLine = _users[0] as User;
        if (!firstInLine.ReadyToEnterInElevator())
        {
            yield break;
        }
        firstInLine.MoveToElevator();
        firstInLine.raiva.SetActive(false);
        _users.Remove(_users[0]);

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
    public WaitPosition GetRandomWaitPosition()
    {
        int random = 0;
        do
        {
            random = Random.Range(0, _waitPositions.Length);
        }
        while (_waitPositions[random].IsFree);
        return _waitPositions[random];
    }
}
