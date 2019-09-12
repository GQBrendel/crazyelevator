using DG.Tweening;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class User : UserBase
{
    public UICcolorsChanger _uiColorChanger;

    [SerializeField] private float _timeToEnterTheElevator = 1;
    [SerializeField] private float _timeToGoToWaitPos = 4;
    [SerializeField] private SkinnedMeshRenderer _meshRendererRagDoll;
    [SerializeField] private GameObject _ragDollObject;
    [SerializeField] private Transform _transformRoot;

    private bool _isAngry;

    private UserAnimator _animator;

    public bool _ragDoll;

    private Tween _moveToElevatorTween;
    private Tween _moveToWaitPositionTween;
    private FloorData _currentFloor;

    public float movementSpeed = 1f;

    public WaitPosition _myWaitPosition;
    public SpawnPosition _mySpawnPosition;

    private ElevatorController _elevator;

    private Rigidbody _rigidbody;

    public bool _isInRiskArea;
    public bool _insideTheElevator;
    private bool _reachedWaitPos;
    private bool _kamikaze;
    private bool _moveForward;
    private bool _moveToElevator;
    private bool _runToElevator;

    private bool movingForward;

    private void Start()
    {
        raiva.SetActive(false);
    }

    public override void Spawn(FloorData currentFloor, FloorData desiredFloor, ElevatorController elevator, Material material, GameManager gm)
    {
        gm.OnElevatorStoped += ElevatorStoped;
        gm.OnFloorChanged += ElevatorMoved;
        gm.OnFailedToGetPosition += HandleCrowdedFloor;

        _elevator = elevator;
        _currentFloor = currentFloor;
        DesiredFloor = desiredFloor;
        MeshRenderer.material = material;
        _meshRendererRagDoll.material = material;
        _animator = GetComponent<UserAnimator>();
        _rigidbody = GetComponent<Rigidbody>();
        _uiColorChanger = FindObjectOfType<UICcolorsChanger>();

        _myWaitPosition = currentFloor.WaitPos;
        _animator.Walk();
        MoveToWaitPos(_myWaitPosition);
    }

    public void MoveToWaitPos(WaitPosition destination)
    {
        _reachedWaitPos = false;
        _myWaitPosition = destination;
        _moveForward = true;

    }
    public bool ReadyToEnterInElevator()
    {
        if (movingForward)
        {
            return false;
        }
        return _reachedWaitPos;
    }

    public void ElevatorMoved(int floorIndex)
    {
        if(floorIndex != _currentFloor.Index)
        {
            StopMovement();
        }
    }

    public void ElevatorStoped(int floorIndex, Vector3 elevatorPos)
    {
        if (_insideTheElevator)
        {
            if(floorIndex == DesiredFloor.Index)
            {
                _transformRoot.SetParent(null);
                OnUserTransported?.Invoke(this);
                _uiColorChanger.UserExitedElevator(this);
                _elevator.CarriedUsers--;
                _elevator.UserLeavedElevator(this);
                _moveToElevator = false;
                _runToElevator = false;
                gameObject.layer = 10;
                _insideTheElevator = false;
            }
        }
    }

    public void MoveToElevator()
    {
        if (_isAngry)
        {
            _animator.EndAngry();
            _transformRoot.DORotate(new Vector3(0, 90, 0), 1.19f, RotateMode.Fast).OnComplete(() => {
                _animator.Walk();
                if (!_reachedWaitPos)
                {
                    return;
                }
                _moveToElevator = true;
            });
        }
        else
        {
            _animator.Walk();
            if (!_reachedWaitPos)
            {
                return;
            }
            _moveToElevator = true;
        }
    }
    public void RunToElevator()
    {
        _animator.EndAngry();

        _transformRoot.DORotate(new Vector3(0, 90, 0), 1.19f, RotateMode.Fast).OnComplete(() => {
            _animator.Run();
            if (!_reachedWaitPos)
            {
                return;
            }
            StartCoroutine(DelayUntilRun());
        });
    }
    private IEnumerator DelayUntilRun()
    {
        yield return new WaitForSeconds(0.2f);
        _runToElevator = true;
    }

    public override void SetSpawnPosition(SpawnPosition spawnPosition)
    {
        _mySpawnPosition = spawnPosition;
    }

    private void HandleInsideElevator()
    {
        _elevator.CarriedUsers++;
        _elevator.UserEnteredElevator(this);
        _insideTheElevator = true;
        _uiColorChanger.UserEnteredTheElevator(this);
        _transformRoot.SetParent(_elevator.transform);
    }

    public void StartImpatientState()
    {
        _transformRoot.DORotate(new Vector3(0, 180, 0), 1.19f, RotateMode.Fast);
        _isAngry = true;
        _animator.Angry();
        raiva.SetActive(true);
    }

    private void StopMovement()
    {
        _moveToElevatorTween.Kill();
    }
    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("ElevatorDoor") )
        {
            if (!_insideTheElevator)
            {
                if(_elevator.IsStopedOnTheFloor(_currentFloor.Index) && _elevator.HasRoom)
                {
                    HandleInsideElevator();
                    StopMovement();
                }
                else
                {
                    _ragDoll = true;
                    AudioManager.instance.Play("Fall");
                    OnUserDied?.Invoke();
                }
            }
        }
    }
    public float yOffset = -0.51f;

    private void Update()
    {
        if (_ragDoll)
        {
            _ragDollObject.SetActive(true);
            gameObject.SetActive(false);
        }

        if (_transformRoot.parent == _elevator.transform)
        {
            Vector3 Position = new Vector3(0, yOffset, 0);
            _transformRoot.localPosition = Position;
        }
        if (_moveForward)
        {
            Vector3 destinationPos = new Vector3(_myWaitPosition.transform.position.x, _transformRoot.position.y, _transformRoot.position.z);

            if (Vector3.Distance(_transformRoot.position, destinationPos) < 0.5f)
            {
                _animator.Idle();
                _reachedWaitPos = true;
                OnReachedDestination?.Invoke(this);
                _moveForward = false;
            }
            if (!_reachedWaitPos)
            {
                _animator.Walk();
                _transformRoot.position += Vector3.right * Time.deltaTime * movementSpeed;
            }
        }
        if (_moveToElevator)
        {
            _animator.Walk();
            Vector3 destinationPos = new Vector3(_elevator.transform.position.x, _transformRoot.position.y, _transformRoot.position.z);

            if (!_insideTheElevator)
            {
                _transformRoot.position += Vector3.right * Time.deltaTime * movementSpeed;
            }
        }
        if (_runToElevator)
        {
            _animator.Run();
            Vector3 destinationPos = new Vector3(_elevator.transform.position.x, _transformRoot.position.y, _transformRoot.position.z);

            if (!_insideTheElevator)
            {
                _transformRoot.position += Vector3.right * Time.deltaTime * movementSpeed*2;
            }
        }
    }
    private void HandleCrowdedFloor(int floorIndex, WaitPosition waitPosition)
    {
        if(floorIndex != _currentFloor.Index)
        {
            return;
        }
        if(_myWaitPosition != waitPosition)
        {
            return;
        }
        PerformUltimateSacrifice();
    }
    private void PerformUltimateSacrifice()
    {
        _kamikaze = true;
        _animator.Run();
        Vector3 finalDestination = new Vector3(_elevator.transform.position.x, _transformRoot.position.y, _elevator.transform.position.z);
    }
}
