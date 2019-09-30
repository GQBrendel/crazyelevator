using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMarkerCollider : MonoBehaviour
{
    private ElevatorController _elevatorController;

    private void Start()
    {
        _elevatorController = GetComponentInParent<ElevatorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var floorMarker = other.transform.GetComponent<FloorMarker>();
        if (floorMarker)
        {
            _elevatorController.FloorChanged(floorMarker);
        }
    }
}
