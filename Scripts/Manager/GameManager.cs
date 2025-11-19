using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KNJ;
using System;
public class GameManager : Singleton<GameManager>
{
    public Action End;

    private void Start()
    {
        LoadData();

        PuzzleManager.Instance.OnPuzzleZoneEnter += HandlePuzzleIn;
        PuzzleManager.Instance.OnpuzzleZoneExit += HandlePuzzleExit;
    }
   
    public void LoadData()
    {
        if (DataManager.Instance.LoadData())
        {
            CharacterManager.Instance.Player.transform.position = DataManager.Instance.GetPlayerPositionData();

            PuzzleDataManager.Instance.Init();
        }

    }

    private void HandlePuzzleIn(MiniGame data)
    {
        if (data.isGravityUse)
        {
            Physics.gravity = data.GravityScale;
        }

        PlayerInput input = CharacterManager.Instance.Player.GetComponent<PlayerInput>();

        if (input != null)
        {
            input.SwitchCurrentActionMap("BallPuzzle");
        }

        Cursor.lockState = CursorLockMode.None;
    }

    private void HandlePuzzleExit()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);

        PlayerInput input = CharacterManager.Instance.Player.GetComponent<PlayerInput>();

        if (input != null)
        {
            input.SwitchCurrentActionMap("KittyAction");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveData();
    }

    public void GameEnd()
    {
        PlayerInput input = CharacterManager.Instance.Player.GetComponent<PlayerInput>();

        if (input != null)
        {
            input.SwitchCurrentActionMap("BallPuzzle");
        }

        Cursor.lockState = CursorLockMode.None;

        End?.Invoke();
    }
}
