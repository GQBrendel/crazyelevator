using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FloorData))]
public class FloorUsersSetup : MonoBehaviour
{
    private FloorData _floorData;

    [SerializeField] private List<GameObject> _userInActivity;

    private void Awake()
    {
        _floorData = GetComponent<FloorData>();
    }


}
