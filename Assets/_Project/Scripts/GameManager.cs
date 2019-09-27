using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(ScoreManager))]
public class GameManager : MonoBehaviour
{
    public delegate void FloorChangeHandler(int floorIndex);
    public delegate void ElevatorStopedHandler(int floorIndex, Vector3 elevatorPos);
    public FloorChangeHandler OnFloorChanged;
    public ElevatorStopedHandler OnElevatorStoped;

    public delegate void FailedToGetPositionInFloorHandler(int floorIndex, WaitPosition waitPosition);
    public FailedToGetPositionInFloorHandler OnFailedToGetPosition;


    private int _elevatorIndex = 3;

    [SerializeField] private GameObject _userPrefab;
    [SerializeField] private GameObject _userRunnerPrefab;
    [SerializeField] private ElevatorController _elevator;
    [SerializeField] private Button _restartButton;
    [SerializeField] private FloorData[] _floors;
    [SerializeField] private Material[] _charactersMaterials;
    [SerializeField] private Text _victoryText;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameJoltAPI _gameJolt;
    [SerializeField] private WaveController _waveController;
    [SerializeField] private MultipleTargetCamera _multipleTargetCamera;

    ScoreManager _scoreManager;
    private float _randomMin = 3;
    private float _randomMax = 5;

    private List<UserBase> _users;
    private int _transportedUsers;
    private int _lostedUsers;
    private float _runnerChance = 0f;
    private float _timer = 0;
    private float _waitTime = 2;
    private bool _runner = false;
    private bool _delayAndCallNextWaveRunning;

    private void Awake()
    {
        _elevator.OnFloorChanged += HandleElevatorFloorChanged;
        _elevator.OnElevatorStoped += HandleElevatorStoped;
        _restartButton.onClick.AddListener(HandleRestartButtonClicked);
        _scoreManager = GetComponent<ScoreManager>();
        _waveController.OnWaveStarted += HandleWaveStarted;
        _scoreManager.OnUserScored += HandleUserScored;
        _users = new List<UserBase>();
    }
    private void Start()
    {
       // scoreManager = FindObjectOfType<ScoreManager>();
        AudioManager.instance.Play("MainSound");

        for (int i = 0; i < _floors.Length; i++)
        {
            _floors[i].Index = i;
        }
    }



    private void HandleWaveStarted()
    {
        StartCoroutine(SpawnUsersRoutine());
    }

    private IEnumerator SpawnUsersRoutine()
    {
        while (_waveController.SpawnedUsers < _waveController.CurrentWaveCount + 4)
        {
            _timer += Time.deltaTime;

            if (_timer >= _waitTime)
            {
                _timer = 0;
                SpawnUserWithRandomValues(_runner);
                _runner = false;
                _waitTime = Random.Range(_randomMin, _randomMax);

                if (_scoreManager.CurrentScore > 10000)
                {
                    _randomMin = 0.5f;
                    _randomMax = 2f;
                    _runner = true;
                }
                else if (_scoreManager.CurrentScore > 9000)
                {
                    _randomMin = 1f;
                    _randomMax = 2.5f;
                    _runnerChance = Random.Range(0, 100);
                    if (_runnerChance < 90)
                    {
                        _runner = true;
                    }
                }
                else if (_scoreManager.CurrentScore > 7500)
                {
                    _randomMin = 1.5f;
                    _randomMax = 2.5f;
                    _runnerChance = Random.Range(0, 100);
                    if (_runnerChance < 70)
                    {
                        print(_runner);
                        _runner = true;
                    }
                    print(_runner);
                }
                else if (_scoreManager.CurrentScore > 5000)
                {
                    _randomMin = 1f;
                    _randomMax = 3f;
                    _runnerChance = Random.Range(0, 100);
                    if (_runnerChance < 50)
                    {
                        _runner = true;
                    }
                }
                else if (_scoreManager.CurrentScore > 3000)
                {
                    _randomMin = 1.5f;
                    _randomMax = 3.0f;
                    _runnerChance = Random.Range(0, 100);
                    if (_runnerChance < 35)
                    {
                        _runner = true;
                    }
                }
                else if (_scoreManager.CurrentScore > 1000)
                {
                    _randomMin = 2f;
                    _randomMax = 4f;
                    _runnerChance = Random.Range(0, 100);
                    if (_runnerChance < 25)
                    {
                        _runner = true;
                    }
                }
            }
            yield return null;
        }
    }

    private void SpawnUserWithRandomValues(bool runner)
    {
        int spawnFloor = Random.Range(0, 5);
        int desiredfloor;
        do
        {
            desiredfloor = Random.Range(0, 5);
        }
        while (desiredfloor == spawnFloor);

        /*spawnFloor = 1;
        desiredfloor = 2;*/

        SpawnUser(spawnFloor, desiredfloor, runner);
    }
    public void GameOver()
    {
        _gameJolt.AddScore(_scoreManager.CurrentScore);
        StartCoroutine(GameOverRoutine());
    }
    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        _endGamePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void SpawnUser(int spawnedFloorIndex, int desiredFloorIndex, bool runner)
    {
        FloorData spawnedFloor = _floors[spawnedFloorIndex];
        FloorData desiredFloor = _floors[desiredFloorIndex];

        SpawnPosition spawnPosition = runner ? _floors[spawnedFloorIndex].RunnerSpawnPos : _floors[spawnedFloorIndex].SpawnPos;

        if (!spawnPosition)
        {
            Debug.LogError("Failed to get spawn position");
            return;
        }
        if(spawnedFloor.ListSize() == 4)
        {
            return;
        }
        UserBase user = null;
        if (runner)
        {
            var userRef = Instantiate(_userRunnerPrefab, spawnPosition.transform.position, _userRunnerPrefab.transform.rotation);
            user = userRef.GetComponentInChildren<UserBase>();
        }
        else
        {
            var userRef = Instantiate(_userPrefab, spawnPosition.transform.position, _userPrefab.transform.rotation);
            user = userRef.GetComponentInChildren<UserBase>();

            spawnedFloor.InsertUser(user);
        }
        user.OnUserTransported += HandleUserTransported;
        user.OnUserDied += HandleUserLosted;

        user.SetSpawnPosition(spawnPosition);

        _waveController.SpawnedUsers++;
        user.Spawn(spawnedFloor, desiredFloor, _elevator, _charactersMaterials[desiredFloorIndex], this);
        _users.Add(user);
        _multipleTargetCamera.Add(user.transform);
    }

    private void HandleRestartButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleElevatorStoped(int floorIndex, Vector3 elevatorPosition)
    {
        _floors[floorIndex].ElevatorStoped();
        _elevatorIndex = floorIndex;
        OnElevatorStoped?.Invoke(floorIndex, elevatorPosition);
    }

    private void HandleElevatorFloorChanged(int floorIndex)
    {
        OnFloorChanged?.Invoke(floorIndex);
    }
    private void HandleUserTransported(UserBase user)
    {
        user.OnUserTransported -= HandleUserTransported;
        user.OnUserDied -= HandleUserLosted;
        _transportedUsers++;
        CheckEndLevel();
    }
    private void HandleUserLosted()
    {
        SpawnUserWithRandomValues(false);
        _lostedUsers++;
        CheckEndLevel();
    }
    [SerializeField] private Image _deathImage1;
    [SerializeField] private Image _deathImage2;
    [SerializeField] private Image _deathImage3;

    private void HandleUserScored()
    {
        _waveController.UpdateUI(_transportedUsers);
        if (_transportedUsers >= _waveController.CurrentWaveCount)
        {
            if (!_delayAndCallNextWaveRunning)
            {
                StartCoroutine(DelayAndCallNextWave());
            }
        }
    }
    private IEnumerator DelayAndCallNextWave()
    {
        _delayAndCallNextWaveRunning = true;
        DestroyUsers();
        yield return new WaitForSeconds(1f);

        _elevator.ClearElevator();
        _waveController.CallNextWave();
        _transportedUsers = 0;
        _delayAndCallNextWaveRunning = false;
    }

    private void DestroyUsers()
    {
        _elevator.ClearElevator();
        foreach(var user in _users)
        {
            user.Despawn();
        }
        _users.Clear();
    }

    private void CheckEndLevel()
    {
        if (_lostedUsers == 1)
        {
            _deathImage1.color = new Color(_deathImage1.color.r, _deathImage1.color.g, _deathImage1.color.b, 255);
        }
        else if (_lostedUsers == 2)
        {
            _deathImage2.color = new Color(_deathImage2.color.r, _deathImage2.color.g, _deathImage2.color.b, 255);
        }

        else if (_lostedUsers >= 3)
        {
            _deathImage3.color = new Color(_deathImage3.color.r, _deathImage3.color.g, _deathImage3.color.b, 255);
            GameOver();
        }
    }
}
