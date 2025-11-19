using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorController : MonoBehaviour
{
    public int moveSpeed;
    public bool canMove = true;
    private Quaternion startRot;
    private PlayerInput input;
    private InputAction ballSpawn;
    //public float maxNUm;
    //public float minNUm;

    private Vector2 curMovementInput;
    private Vector3 changeZValue;

    private void Awake()
    {
        startRot = transform.localRotation;

        input = GetComponent<PlayerInput>();

        ballSpawn = input.actions["UseKey"];

        ballSpawn.performed += BallSpawner.Instance.OnSpawnBall;
    }

    private void FixedUpdate()
    {
        if (!MainPuzzle_UIManager.Instance.gameClearUI.activeSelf)      //게임 클리어 하면 움직이지 않게 //TODO : 매니저 혹은 EndPoint bool값 isClear를 통해 관리 해 줄 예정입니다

            MoveFloor();
    }

    public void OnSetMoveValue(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        if (context.phase == InputActionPhase.Performed )
        {
            curMovementInput = context.ReadValue<Vector2>();
            changeZValue = new Vector3(-curMovementInput.x, 0, -curMovementInput.y);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            changeZValue = Vector3.zero;
        }
    }

    public void MoveFloor()
    {
        if (!canMove) return;

        //transform.Rotate(changeZValue, moveSpeed * Time.deltaTime);       //기존 이동 로직

        //Vector3 affter = transform.rotation.eulerAngles + changeZValue * moveSpeed * Time.deltaTime;

        transform.localRotation *= Quaternion.Euler(changeZValue * moveSpeed * Time.deltaTime);
    }

    public void RotateReSet()
    {
        Debug.Log("?");
        transform.localRotation = startRot;
    }

    //IEnumerator CantMove()
    //{
    //    canMove = false;
    //    yield return new WaitForSeconds(1f);
    //    canMove = true;
    //}
}
