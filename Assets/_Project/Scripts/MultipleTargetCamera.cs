using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
   
    [SerializeField] private List<Transform> _targets;
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private float _smoothTime = 0.5f;
    [SerializeField] private float _minZoom = 40f;
    [SerializeField] private float _maxZoom = 10f;
    [SerializeField] private float _zomLimiter = 50f;

    private bool _shouldUpdate = true;

    private Camera _camera;

    private Vector3 _velocity;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        if (!_shouldUpdate)
        {
            return;
        }
        if(_targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
    }
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _offSet;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(_maxZoom, _minZoom, GetGreatestDistance() / _zomLimiter);

        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] == null)
            {
                Remove(_targets[i]);
            }
            else
            {
                bounds.Encapsulate(_targets[i].position);
            }
        }
        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if(_targets.Count == 1)
        {
            return _targets[0].position;
        }

        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for(int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] == null)
            {
                Remove(_targets[i]);
            }
            else
            {
                bounds.Encapsulate(_targets[i].position);
            }
        }
        return bounds.center;
    }

    public void ToggleUpdateStatus()
    {
        _shouldUpdate = !_shouldUpdate;
    }
    public void Add(Transform target)
    {
        if (!_targets.Contains(target))
        {
            _targets.Add(target);
        }
    }
    public void Remove(Transform target)
    {
        if (_targets.Contains(target))
        {
            _targets.Remove(target);
        }
    }


}
