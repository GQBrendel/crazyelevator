using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int _usersOnTheLevel = 10;
    [SerializeField] private Text _victoryText;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameJoltAPI _gameJolt;

    ScoreManager _scoreManager;
    private float _randomMin = 3;
    private float _randomMax = 5;

    private ScoreManager scoreManager;

    private int _spawnedUsers;
    private int _transportedUsers;
    private int _lostedUsers;

    private void Awake()
    {
        _elevator.OnFloorChanged += HandleElevatorFloorChanged;
        _elevator.OnElevatorStoped += HandleElevatorStoped;
        _restartButton.onClick.AddListener(HandleRestartButtonClicked);
        _scoreManager = GetComponent<ScoreManager>();
    }
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        AudioManager.instance.Play("MainSound");

        for (int i = 0; i < _floors.Length; i++)
        {
            _floors[i].Index = i;
        }    
    }

    bool runner = false;
    float runnerChance = 0f;

    float timer = 0;
    float waitTime = 2;


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            timer = 0;
            SpawnUserWithRandomValues(runner);
            runner = false;
            waitTime = Random.Range(_randomMin, _randomMax);

            if (scoreManager.CurrentScore > 10000)
            {
                _randomMin = 0.5f;
                _randomMax = 2f;
                runner = true;
            }
            else if (scoreManager.CurrentScore > 9000)
            {
                _randomMin = 1f;
                _randomMax = 2.5f;
                runnerChance = Random.Range(0, 100);
                if (runnerChance < 90)
                {
                    runner = true;
                }
            }
            else if (scoreManager.CurrentScore > 7500)
            {
                _randomMin = 1.5f;
                _randomMax = 2.5f;
                runnerChance = Random.Range(0, 100);
                if (runnerChance < 70)
                {
                    print(runner);
                    runner = true;
                }
                print(runner);
            }
            else if (scoreManager.CurrentScore > 5000)
            {
                _randomMin = 1f;
                _randomMax = 3f;
                runnerChance = Random.Range(0, 100);
                if (runnerChance < 50)
                {
                    runner = true;
                }
            }
            else if (scoreManager.CurrentScore > 3000)
            {
                _randomMin = 1.5f;
                _randomMax = 3.0f;
                runnerChance = Random.Range(0, 100);
                if (runnerChance < 35)
                {
                    runner = true;
                }
            }
            else if (scoreManager.CurrentScore > 1000)
            {
                _randomMin = 2f;
                _randomMax = 4f;
                runnerChance = Random.Range(0, 100);
                if (runnerChance < 25)
                {
                    runner = true;
                }
            }
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

        _spawnedUsers++;
        user.Spawn(spawnedFloor, desiredFloor, _elevator, _charactersMaterials[desiredFloorIndex], this);
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
        _lostedUsers++;
        CheckEndLevel();
    }
    [SerializeField] private Image _deathImage1;
    [SerializeField] private Image _deathImage2;
    [SerializeField] private Image _deathImage3;

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
