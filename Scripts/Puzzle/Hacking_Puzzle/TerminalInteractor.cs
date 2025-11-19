using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalInteractor : MonoBehaviour
{
    [Header("Assign the UI Prefab (Canvas 포함 프리팹)")]
    public GameObject hackingUIPrefab;

    private GameObject currentHackingUI = null;
    private HackingMiniManager miniManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentHackingUI == null) StartHacking();
            else StopHacking();
        }
    }

    void StartHacking()
    {
        if (!hackingUIPrefab)
        {
            Debug.LogError("[TerminalInteractor] hackingUIPrefab 미할당!");
            return;
        }

        currentHackingUI = Instantiate(hackingUIPrefab);

        // 루트/자식 어디에 붙어있든 안전하게 찾기
        miniManager = currentHackingUI.GetComponentInChildren<HackingMiniManager>(true);
        if (miniManager == null)
        {
            Debug.LogError("[TerminalInteractor] 프리팹에서 HackingMiniManager를 찾지 못했습니다. 프리팹 구조를 확인하세요.");
            return;
        }

        // 게임 종료(성공/타임아웃) 이벤트 구독
        miniManager.OnGameFinished += HandleGameFinished;

        // EventSystem 없으면 UI 입력이 안 들어옴
        EnsureEventSystem();
    }

    void StopHacking()
    {
        if (miniManager != null)
        {
            miniManager.OnGameFinished -= HandleGameFinished;
            miniManager = null;
        }

        if (currentHackingUI != null)
        {
            Destroy(currentHackingUI);
            currentHackingUI = null;
        }
    }

    private void HandleGameFinished(bool success)
    {
        if (success) Debug.Log("해킹 성공! 보상을 지급합니다.");
        else Debug.Log("해킹 실패. 터미널이 잠깁니다.");

        StartCoroutine(CloseUIAfterDelay(1.5f));
    }

    private IEnumerator CloseUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopHacking();
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(go);
        }
    }
}
