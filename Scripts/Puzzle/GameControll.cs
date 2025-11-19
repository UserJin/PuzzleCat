using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControll : InteractableObject
{
    public MiniGame games;
    public Transform rewardSpawnPoint;

    private void Start()
    {
        InteractionText = "Press [F]";
    }

    public void CheckClear()
    {
        if (PuzzleDataManager.Instance.puzzleClearData.ContainsKey(games.GameID) &&
            PuzzleDataManager.Instance.puzzleClearData[games.GameID])
        {
            if (games.reward != null && rewardSpawnPoint != null)
            {
                Instantiate(games.reward, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
            }
            Setactive();
        }
    }

    public void Setactive()
    {
        gameObject.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();

        PuzzleManager.Instance.PuzzleIn(games, rewardSpawnPoint);
        PuzzleDataManager.Instance.Clear += Setactive;
    }

    private void OnDestroy()
    {
        PuzzleDataManager.Instance.Clear -= Setactive;
    }
}