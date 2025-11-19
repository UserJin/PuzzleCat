using KNJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleDataManager : Singleton<PuzzleDataManager>
{
    public Dictionary<string, bool> puzzleClearData = new Dictionary<string, bool>();

    public List<MiniGame> mainPuzzleCheck;

    public Action Clear;

    public void isGameCleared(MiniGame data)
    {
        string id = data.GameID;

        if (puzzleClearData.ContainsKey(id))
        {
            puzzleClearData[id] = true;
        }
        else
        {
            puzzleClearData.Add(id, true);
        }

        CheckGameClear();
        Clear?.Invoke();

        DataManager.Instance.SaveData();
    }

    public void Init()
    {
        puzzleClearData = DataManager.Instance.GetGameClearData();

        GameControll[] allPuzzles = FindObjectsOfType<GameControll>();

        foreach (GameControll puzzle in allPuzzles)
        {
            puzzle.CheckClear();
        }
    }

    private void CheckGameClear()
    {
        foreach (MiniGame mainPuzzle in mainPuzzleCheck)
        {
            string id = mainPuzzle.GameID;

            if (!puzzleClearData.ContainsKey(id) || !puzzleClearData[id])
            {
                return;
            }
        }

        Debug.Log("엔딩씬");
        GameManager.Instance.GameEnd();
    }

}