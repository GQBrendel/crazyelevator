using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute : MonoBehaviour
{
    [SerializeField] AudioListener cam;

    public void ismute()
    {
        if (cam.enabled)
        {
            cam.enabled = false;
        }
        else
            cam.enabled = true;
    }
}
