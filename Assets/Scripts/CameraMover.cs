using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 1f;

    private Vector3 _offset;

    private void Start()
    {
        _offset = _target.position - transform.position;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 desiredPosition = _target.position - _offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
    }
}