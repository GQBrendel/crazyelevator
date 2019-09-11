using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPosition : MonoBehaviour
{
    public bool IsFree { get; set; }

    private void Awake()
    {
        IsFree = true;
    }
}
