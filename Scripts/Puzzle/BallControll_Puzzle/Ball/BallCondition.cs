using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCondition : MonoBehaviour
{
    public int deadPositionY;

    private void Update()
    {
        if (transform.position.y > deadPositionY) return;

        Destroy(this.gameObject);
    }
}
