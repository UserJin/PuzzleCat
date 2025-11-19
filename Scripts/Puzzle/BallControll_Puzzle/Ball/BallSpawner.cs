using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : Singleton<BallSpawner>
{
    public GameObject ballPrefab;
    private GameObject ballSpawnUI;      //TODO : 매니저 혹은 EndPoint bool값 isClear를 통해 관리 해 줄 예정입니다 
    private GameObject curPrefab;
    private FloorController controller;
    private bool isReset = false;

    private void Awake()
    {
        ballSpawnUI = MainPuzzle_UIManager.Instance.ballSpawnUI.gameObject;
        controller = FindObjectOfType<FloorController>();
    }

    private void Update()
    {
        BallSpawnUISet();

        if (curPrefab != null || isReset) return;
        controller.RotateReSet();
        isReset = true;
    }

    public void OnSpawnBall(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started || curPrefab != null) return;

        curPrefab = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        isReset = false;
        Debug.Log(curPrefab);
    }

    void BallSpawnUISet()
    {
        if (curPrefab != null)        //공이 준비되어 있다면 UI를 꺼라
        {
            controller.canMove = true;
            ballSpawnUI.SetActive(false);
        }
        else if (curPrefab == null)      //공이 준비되어 있지 않다면 UI를 켜라
        {
            controller.canMove = false;
            ballSpawnUI.SetActive(true);
        }
    }
}
