using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public delegate void FloorChangeHandler(int floorIndex);
    public delegate void ElevatorStopedHandler(int floorIndex, Vector3 elevatorPos);
    public delegate void ElevatorCleared();
    public ElevatorCleared OnElevatorCleared;
    public FloorChangeHandler OnFloorChanged;
    public ElevatorStopedHandler OnElevatorStoped;

    [SerializeField] private GameObject _destroyUserEffectPrefab;
    [SerializeField] private Transform _currentFloorPosition;
    [SerializeField] private int m_currentFloorIndex;
    [SerializeField] private Arrow _arrow;
    [SerializeField] private MultipleTargetCamera _multipleTargetCamera;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private List<GameObject> _usersInElevator;
    [SerializeField] private WaveController _waveController;

    private Dictionary<UserBase, GameObject> _userToElevatorDictionary;
    private float _maxY = 31.8541f;
    private float _minY = 3.100102f;

    private Vector3 _mouseOffset;
    private float _mouseZCoord;
    private bool _isStoped = true;

    public int CarriedUsers;
    public int MaxCapacity = 4;

    public int CurrentFlootIndex { get { return m_currentFloorIndex; } }
    public bool HasRoom { get { return CarriedUsers < MaxCapacity; } }

    private void Start()
    {
        _userToElevatorDictionary = new Dictionary<UserBase, GameObject>();
        _waveController.OnWaveEnded += HandleWaveEnded;
        foreach (var user in _usersInElevator)
        {
            user.SetActive(false);
        }
    }

    public void UserEnteredElevator(UserBase user)
    {
        foreach (var userInElevator in _usersInElevator)
        {
            if (!userInElevator.activeInHierarchy)
            {
                _userToElevatorDictionary.Add(user, userInElevator);
                userInElevator.SetActive(true);
                userInElevator.GetComponentInChildren<SkinnedMeshRenderer>().material = user.MeshRenderer.material;
                user.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void UserLeavedElevator(UserBase user)
    {
        GameObject leavingUser = _userToElevatorDictionary[user];
        _userToElevatorDictionary.Remove(user);
        leavingUser.gameObject.SetActive(false);
        user.gameObject.SetActive(true);
        _multipleTargetCamera.Remove(user.transform);
    }          

    public bool IsStopedOnTheFloor(int floorIndex)
    {
        return (m_currentFloorIndex == floorIndex && _isStoped);
    }

    public void FloorChanged(FloorMarker floorMarker)
    {
        if (_isStoped)
        {
            return;
        }
        if (floorMarker)
        {
            m_currentFloorIndex = floorMarker.Index;
            _currentFloorPosition = floorMarker.transform;
            OnFloorChanged?.Invoke(m_currentFloorIndex);
        }
    }
    private void OnMouseDown()
    {
        _multipleTargetCamera.SetUpdateStatus(false);
        _isStoped = false;
        _mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        _mouseOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        if (_arrow.gameObject.activeInHierarchy)
        {
            _arrow.gameObject.SetActive(false);
        }
    }
    private void OnMouseUp()
    {
        _cameraShake.ShakeIt();
//        _multipleTargetCamera.SetUpdateStatus(true);
        AudioManager.instance.Play("Elevator");
        if (!_currentFloorPosition)
        {
            return;
        }
        _isStoped = true;
        Vector3 movePosition = new Vector3(transform.position.x, _currentFloorPosition.position.y, transform.position.z);
        transform.position = movePosition;
        OnElevatorStoped?.Invoke(m_currentFloorIndex, transform.position);
    }
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _mouseZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    void OnMouseDrag()
    {
        float yPos = (GetMouseAsWorldPoint() + _mouseOffset).y;

        if(yPos > _maxY)
        {
            yPos = _maxY;
        }
        if(yPos < _minY)
        {
            yPos = _minY;
        }
        Vector3 movePosition = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.position = movePosition;
    }

    private void HandleWaveEnded()
    {
        ClearElevator();
    }
    private void ClearElevator()
    {
        for (int i = 0; i < _usersInElevator.Count; i++)
        {
            if (_usersInElevator[i].activeInHierarchy)
            {
                _usersInElevator[i].SetActive(false);
                Instantiate(_destroyUserEffectPrefab, _usersInElevator[i].transform.position, _destroyUserEffectPrefab.transform.rotation);
            }
        }
        CarriedUsers = 0;
        _userToElevatorDictionary.Clear();
        OnElevatorCleared?.Invoke();
    }
}