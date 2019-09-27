using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUsersSetup : MonoBehaviour
{
    private FloorData _floorData;

    [SerializeField] private List<GameObject> _userInActivity;
    [SerializeField] private Transform _defaultDespawnPos;

    private Dictionary<Transform, bool> _transfomTakeDictionary;
    private Dictionary<GameObject, bool> _userIsActiveDictionaty;

    private void Awake()
    {
        _floorData = GetComponent<FloorData>();
        _transfomTakeDictionary = new Dictionary<Transform, bool>();
        _userIsActiveDictionaty = new Dictionary<GameObject, bool>();

        foreach (var user in _userInActivity)
        {
            user.SetActive(false);
            _transfomTakeDictionary.Add(user.transform, false);
            _userIsActiveDictionaty.Add(user, false);
        }
    }

    public Transform GetUserPosition()
    {
        foreach (var user in _userInActivity)
        {
            if (!_transfomTakeDictionary[user.transform])
            {
                _transfomTakeDictionary[user.transform] = true;
                return user.transform;
            }
        }

        Debug.Log("Max capacity reached");
        return _defaultDespawnPos;
    }
    public void NewUserInTheFloor()
    {
        foreach (var user in _userInActivity)
        {
            if (!_userIsActiveDictionaty[user])
            {
                _userIsActiveDictionaty[user] = true;
                StartCoroutine(EnableUser(user));
                return;
            }
        }
        return;
    }
    private IEnumerator EnableUser(GameObject user)
    {
        yield return new WaitForSeconds(.5f);
        user.SetActive(true);
    }


}
