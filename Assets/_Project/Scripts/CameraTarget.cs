using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Transform targetFoco;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, GameObject.Find("Elevator").transform.position.y,transform.position.z);
        StartCoroutine("lookatFoco");
    }


    IEnumerator lookatFoco()
    {
        yield return new WaitForSeconds(0);
        transform.LookAt(targetFoco);
        StartCoroutine("lookatFoco");
    }
}
