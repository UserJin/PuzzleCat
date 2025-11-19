using KNJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpawn : MonoBehaviour
{
    public GameObject puzzle1;
    public GameObject puzzle2;
    public GameObject puzzle3;
    public GameObject puzzle4;

    public Transform spawPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(puzzle1, spawPosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(puzzle2, spawPosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Instantiate(puzzle3, spawPosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Instantiate(puzzle4, spawPosition);
        }
    }
}
