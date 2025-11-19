using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<GameObject> levels;

    private void Awake()
    {
        Spawn(0);
    }

    void Spawn(int level)
    {
        Instantiate(levels[level], transform.position, transform.rotation, transform);
    }
}
