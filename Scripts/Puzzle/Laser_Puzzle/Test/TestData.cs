using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            KNJ.DataManager.Instance.SaveData();
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            KNJ.DataManager.Instance.LoadData();
        }
    }
}
