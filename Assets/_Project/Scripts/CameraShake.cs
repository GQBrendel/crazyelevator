using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(MultipleTargetCamera))]
public class CameraShake : MonoBehaviour
{
    Vector3 cameraInitialPosition;
    [SerializeField] private float _shakeMagnitude = 0.05f;
    [SerializeField] private float _shakeTime = 0.2f;
    private Camera _camera;
    private MultipleTargetCamera _multipleTargetCamera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _multipleTargetCamera = GetComponent<MultipleTargetCamera>();
    }

    public void ShakeIt()
    {
        cameraInitialPosition = _camera.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", _shakeTime);
    }

    private void StartCameraShaking()
    {
        _multipleTargetCamera.SetUpdateStatus(false);
        float cameraShakingOffsetX = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        //float cameraShakingOffsetY = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        Vector3 cameraIntermadiatePosition = _camera.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
    //    cameraIntermadiatePosition.y += cameraShakingOffsetY;
        _camera.transform.position = cameraIntermadiatePosition;
    }

    private void StopCameraShaking()
    {
        _multipleTargetCamera.SetUpdateStatus(true);
        CancelInvoke("StartCameraShaking");
        _camera.transform.position = cameraInitialPosition;
    }

}
