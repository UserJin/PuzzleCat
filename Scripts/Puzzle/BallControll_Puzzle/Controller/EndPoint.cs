using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private GameObject gameClearUI;

    private bool isClear = false;   //공이 튀겼을 때 UI 반복 띄우는 현상 방지할 bool값 //이게 게임매니저에서 받아가야 하고 이걸로 관리를 해야함

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball") || isClear) return;

        PuzzleManager.Instance.PuzzleClear();
        //gameClearUI.SetActive(true);
        //Debug.Log("게임 클리어 UI띄우기");
    }
}
