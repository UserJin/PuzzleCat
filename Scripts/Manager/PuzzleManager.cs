using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : Singleton<PuzzleManager>
{
    public GameObject obj;
    public MiniGame miniGame;
    public Action Reward;
    public event Action<MiniGame> OnPuzzleZoneEnter;
    public event Action OnpuzzleZoneExit;
    public Transform currentRwdTrs;

    public void PuzzleIn(MiniGame data, Transform rwdTrs)
    {
        miniGame = null;
        currentRwdTrs = null;
        OnPuzzleZoneEnter?.Invoke(data);

        if (obj != null)
        {
            DestroyObj();
        }

        miniGame = data;
        currentRwdTrs = rwdTrs;
        obj = Instantiate(data.levels, transform.position, transform.rotation, transform);

        // 미니게임 BGM 시작 (페이드 효과)
        if (SoundManager.instance != null)
        {
            // 미니게임 데이터의 BGM 정보를 사용
            SoundManager.instance.FadeToBGM(
                data.bgmClip,
                data.bgmVolume,
                data.fadeOutTime,
                data.fadeInTime
            );
        }
    }

    public void PuzzleExit()
    {
        OnpuzzleZoneExit?.Invoke();

        // 기본 BGM으로 복귀 (페이드 효과)
        if (SoundManager.instance != null)
        {
            // miniGame 데이터가 없을 경우를 대비한 방어 코드
            float fadeOutTime = (miniGame != null) ? miniGame.fadeOutTime : 1.0f;

            SoundManager.instance.FadeToBGM(
                SoundManager.instance.defaultBGM,
                SoundManager.instance.defaultBGMVolume,
                fadeOutTime,
                1.0f
            );
        }

        DestroyObj();
    }

    public void PuzzleClear()
    {
        if (miniGame.reward != null && currentRwdTrs != null)
        {
            SpawnReward();
            Debug.Log("Spawn!");
        }
        PuzzleDataManager.Instance.isGameCleared(miniGame);
        PuzzleExit();
        DestroyObj();
    }

    public void SpawnReward()
    {
        Instantiate(miniGame.reward, currentRwdTrs.position, currentRwdTrs.rotation);
    }

    public void DestroyObj()
    {
        Destroy(obj);
    }
}