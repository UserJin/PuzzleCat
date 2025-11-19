using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalForce : MonoBehaviour
{
    [SerializeField] private float divisor01;
    [SerializeField] private float divisor02;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (transform.position.z > 0f)
        {
            var velocity = _rigidbody.velocity;
            var normalized = velocity.normalized;
            var force = new Vector3(normalized.x * divisor01, normalized.y * divisor02, normalized.z * divisor01);
            _rigidbody.AddForce(force);
        }
    }
}
