using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public int layer;
    public float radius;
    private AdditionalForce additional;

    private void Awake()
    {
        additional = GetComponent<AdditionalForce>();

        layer = LayerMask.NameToLayer("Puzzle");
    }

    private void FixedUpdate()
    {
            additional.enabled = !Physics.CheckSphere(transform.position, radius, 1 << layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
