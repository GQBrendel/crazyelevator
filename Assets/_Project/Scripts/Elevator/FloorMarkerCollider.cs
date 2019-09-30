using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMarkerCollider : MonoBehaviour
{
    private ElevatorController _elevatorController;

    //system Light
    [SerializeField] private Light[] lights;
    private bool isClicked;

    private void Start()
    {
        _elevatorController = GetComponentInParent<ElevatorController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lights[0].enabled = false;
            lights[1].enabled = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lights[0].enabled = true;
            lights[1].enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var floorMarker = other.transform.GetComponent<FloorMarker>();
        if (floorMarker)
        {
            _elevatorController.FloorChanged(floorMarker);
        }

        if (other.gameObject.name == "Floor0")
        {
            lights[0].color = Color.green;
        }else if (other.gameObject.name == "Floor1")
        {
            lights[0].color = Color.magenta;
        }
        else if (other.gameObject.name == "Floor2")
        {
            lights[0].color = Color.yellow;
        }
        else if (other.gameObject.name == "Floor3")
        {
            lights[0].color = Color.red;
        }
        else if (other.gameObject.name == "Floor4")
        {
            lights[0].color = Color.blue;
        }
    }
}
