using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSensor : MonoBehaviour
{
    private GameObject ball;
    public Collider topCollider;

    private void Update()
    {
        if (ball == null) topCollider.isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ball != null) return;

        ball = collision.gameObject;
        topCollider.isTrigger = false;
    }
}
