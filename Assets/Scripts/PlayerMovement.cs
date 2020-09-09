using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _jumpForce;

    private Rigidbody _rigidbody;
    private SphereCollider _collider;

    private bool _isGrounded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isGrounded)
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position, _collider.radius + 0.01f, 1 << 8);

        if (_rigidbody.velocity.magnitude < _maxSpeed)
            _rigidbody.AddForce(Vector3.right * _moveForce);
    }
}
