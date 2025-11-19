using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 레이저 퍼즐을 전체적으로 관리하는 매니저
/// 시작시 모든 레이저 발사기에 발사를 명령함
/// 레이저 경로를 다시 계산하는 호출이 발생하면 이벤트에 등록된 함수를 작동시키고 경로를 재계산
/// 퍼즐이 클리어되면 클리어 처리를 진행하고 퍼즐 제거
/// </summary>
public class LaserPuzzleManager : MonoBehaviour
{
    [SerializeField] private LaserEmitter[] emitters;
    [SerializeField] private LaserReceiver[] receivers;

    // 오브젝트 상태가 변했을 때 발생시킬 이벤트
    public event Action OnObjectChange;

    private Transform puzzle;
    private Camera cam;
    public GameObject clearUI;

    public LaserPuzzleInputManager inputManager;

    private void Start()
    {
        Init();

        emitters = GetComponentsInChildren<LaserEmitter>();
        receivers = GetComponentsInChildren<LaserReceiver>();

        FireAllLaser();
    }

    private void FireAllLaser()
    {
        foreach (LaserEmitter emitter in emitters)
        {
            emitter.FireLaser();
        }
    }

    public void RecalculateLaser()
    {
        StartCoroutine(DelayRecalculate());
    }

    private IEnumerator DelayRecalculate()
    {
        yield return new WaitForSeconds(0.02f);
        OnObjectChange.Invoke();
        FireAllLaser();
    }

    public void CheckCompletion()
    {
        foreach(LaserReceiver receiver in receivers)
        {
            if(!receiver.CheckActive())
            {
                //하나라도 활성화 X
                return;
            }
        }

        Debug.Log("Puzzle Clear");
        // 퍼즐 클리어
        // 클리어 처리는 여기서
        // 클리어 했다는 UI를 띄어야 하나?
        ShowClearUI();
    }

    public void ShowClearUI()
    {
        clearUI.SetActive(true);
        // 인풋 비활성화
        inputManager.DeactivateInput();
    }

    private void Init()
    {
        // 부모 퍼즐 찾기
        puzzle = transform.parent;

        // 카메라 오소그래픽
        cam = Camera.main;
        cam.orthographic = true;
        Cursor.lockState = CursorLockMode.None;
        // 인풋 매니저 등록
        inputManager = puzzle.GetComponentInChildren<LaserPuzzleInputManager>();
    }

    public void OnClearBtn()
    {
        // 해당 퍼즐 클리어 했다는 거 알림 / 데이터 저장
        clearUI.SetActive(false);
        // 카메라 오소그래픽 해제
        cam.orthographic = false;
        Cursor.lockState = CursorLockMode.Locked;
        // 퍼즐 매니저에 퍼즐 클리어 알림
        PuzzleManager.Instance.PuzzleClear();
    }
}
