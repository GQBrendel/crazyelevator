﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public delegate void FloorChangeHandler(int floorIndex);
    public delegate void ElevatorStopedHandler(int floorIndex, Vector3 elevatorPos);
    public FloorChangeHandler OnFloorChanged;
    public ElevatorStopedHandler OnElevatorStoped;

    [SerializeField] private Transform _currentFloorPosition;
    [SerializeField] private int m_currentFloorIndex;
    [SerializeField] private Arrow _arrow;
    [SerializeField] private List<GameObject> _usersInElevator;
    private Dictionary<UserBase, GameObject> _userToElevatorDictionary;

    public int CarriedUsers;
    public int MaxCapacity = 4;
    public bool HasRoom { get { return CarriedUsers < MaxCapacity; } }

    private void Start()
    {
        _userToElevatorDictionary = new Dictionary<UserBase, GameObject>();
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
    }

    private Vector3 _mouseOffset;
    private float _mouseZCoord;
    private bool _isStoped = true;


    public int CurrentFlootIndex { get { return m_currentFloorIndex; } }

   

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
        Vector3 movePosition = new Vector3(transform.position.x, (GetMouseAsWorldPoint() + _mouseOffset).y, transform.position.z);
        transform.position = movePosition;
    }
}