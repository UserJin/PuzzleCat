using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using URandom = UnityEngine.Random;

public enum GameState { FirstClick, Playing, Finished }
public enum DifficultyLevel { Easy, Normal, Hard }

[RequireComponent(typeof(GridLayoutGroup))]
public class HackingMiniManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public GridLayoutGroup gridPanel;
    [SerializeField] public TextMeshProUGUI bufferText;
    [SerializeField] public TextMeshProUGUI logText;
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public TextMeshProUGUI infoText;
    [SerializeField] public GameObject cellPrefab;
    [SerializeField] public GameObject exitButton;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject retryGameUI;
    [SerializeField] private GameObject successUI; 

    [Header("Color Settings")]
    [SerializeField] public Color normalColor = new(0.5f, 0.5f, 0.5f, 1f);
    [SerializeField] public Color selectedColor = new(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] public Color highlightColor = new(0.8f, 0.8f, 0.2f, 1f);
    [SerializeField] public Color disabledColor = new(0.2f, 0.2f, 0.2f, 1f);

    [Header("Game Settings")]
    [SerializeField] private DifficultyLevel difficulty = DifficultyLevel.Normal;
    [HideInInspector][SerializeField] private float gameTime = 60.0f;
    [HideInInspector][SerializeField] private int sequenceLength = 3;
    [SerializeField] private int gridRows = 5;
    [SerializeField] private int gridCols = 5;
    [SerializeField] private bool reshuffleChangesDaemon = true;

    [Header("Penalty Settings")]
    [SerializeField] private bool useMistakePenalty = true;
    [SerializeField] private float mistakePenaltySeconds = 5f;
    [SerializeField] private float minTimerAfterPenalty = 0f;

    [Header("Image Settings")]
    [SerializeField] private List<Sprite> hexSprites;

    private readonly List<string> hexCodes = new() { "낚시대", "쥐", "실타래", "츄르", "캣닢" };

    private GameState currentState;
    private string[,] gridData;
    private GridCell[,] cells;
    private List<string> daemonSequence = new();
    private List<string> matchedSequence = new();
    private int matchIndex = 0;

    private int lastRow = -1, lastCol = -1;
    private float timer;

    public event System.Action<bool> OnGameFinished;

    void Start()
    {
        if (gridPanel == null || cellPrefab == null)
        {
            Debug.LogError("[HackingMiniManager] UI 또는 CellPrefab이 연결되지 않았습니다!");
            return;
        }

        if (hexSprites == null || hexSprites.Count != hexCodes.Count)
        {
            Debug.LogError("Hex Sprites 리스트가 비어있거나, 개수가 올바르지 않습니다. 인스펙터에 이미지를 5개 모두 연결해주세요!");
            return;
        }

        //  게임 시작 전 초기 상태 설정
        ShowUI(startGameUI);
        HideUI(gridPanel.gameObject, bufferText.gameObject, logText.gameObject, timerText.gameObject, infoText.gameObject, exitButton);
        HideUI(retryGameUI, successUI);
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F2");

            if (timer <= 0f)
            {
                TimeOutFail();
            }
        }
    }

    void OnDestroy() { }

    //  게임 시작 버튼에 연결될 함수
    public void StartGame()
    {
        HideUI(startGameUI);
        ShowUI(gridPanel.gameObject, bufferText.gameObject, logText.gameObject, timerText.gameObject, infoText.gameObject);
        InitializeGame(true);

        //  게임 시작과 동시에 타이머가 흐르도록 currentState를 Playing으로 변경
        currentState = GameState.Playing;
    }

    //  재시작 버튼에 연결될 함수
    public void RetryGame()
    {
        HideUI(retryGameUI);
        ShowUI(gridPanel.gameObject, bufferText.gameObject, logText.gameObject, timerText.gameObject, infoText.gameObject);
        InitializeGame(true);

        //  재시작과 동시에 타이머가 흐르도록 currentState를 Playing으로 변경
        currentState = GameState.Playing;
    }

    //  성공 시 게임을 종료하는 함수
    public void ExitGame()
    {
        PuzzleManager.Instance.PuzzleClear ();
    }



    void InitializeGame(bool newDaemon)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                gameTime = 90f;
                sequenceLength = 3;
                break;
            case DifficultyLevel.Normal:
                gameTime = 30f;
                sequenceLength = 4;
                break;
            case DifficultyLevel.Hard:
                gameTime = 15f;
                sequenceLength = 4;
                break;
        }

        gridData = new string[gridRows, gridCols];
        cells = new GridCell[gridRows, gridCols];

        matchedSequence.Clear();
        matchIndex = 0;
        bufferText.text = "";
        logText.text = "";

        //  게임 시작 버튼을 누르기 전에는 FirstClick 상태로 유지
        currentState = GameState.FirstClick;
        lastRow = -1; lastCol = -1;

        ResetGrid();
        GenerateGrid();

        if (newDaemon || daemonSequence.Count == 0)
            daemonSequence = GenerateDaemon();

        infoText.text = "아이템 순서: " + string.Join(" ", daemonSequence);

        timer = gameTime;
        timerText.text = timer.ToString("F2");
    }

    public void SetDifficultyAndRestart(string difficultyStr)
    {
        switch (difficultyStr.ToLower())
        {
            case "easy":
                difficulty = DifficultyLevel.Easy;
                break;
            case "normal":
                difficulty = DifficultyLevel.Normal;
                break;
            case "hard":
                difficulty = DifficultyLevel.Hard;
                break;
            default:
                Debug.LogWarning("Invalid difficulty level. Setting to Normal.");
                difficulty = DifficultyLevel.Normal;
                break;
        }

        InitializeGame(true);
        currentState = GameState.Playing;
    }

    void Reshuffle(bool changeDaemon)
    {
        matchedSequence.Clear();
        matchIndex = 0;
        bufferText.text = "";

        //  리셔플 시에는 게임 플레이 상태를 유지
        currentState = GameState.Playing;
        lastRow = -1; lastCol = -1;

        ResetGrid();
        GenerateGrid();

        if (changeDaemon)
            daemonSequence = GenerateDaemon();

        infoText.text = "아이템 순서: " + string.Join(" ", daemonSequence);
        logText.text += "[다시 섞기]\n";
    }

    void ResetGrid()
    {
        for (int i = gridPanel.transform.childCount - 1; i >= 0; i--)
            Destroy(gridPanel.transform.GetChild(i).gameObject);
    }

    void GenerateGrid()
    {
        gridPanel.constraintCount = gridCols;
        for (int r = 0; r < gridRows; r++)
        {
            for (int c = 0; c < gridCols; c++)
            {
                int hexIndex = URandom.Range(0, hexCodes.Count);
                string code = hexCodes[hexIndex];

                gridData[r, c] = code;

                GameObject cellObj = Instantiate(cellPrefab, gridPanel.transform);
                var cell = cellObj.GetComponent<GridCell>();
                int lr = r, lc = c;

                cell.Setup(code, lr, lc, hexSprites[hexIndex], () => OnCellClick(lr, lc));
                cell.SetState(CellState.Normal, normalColor);
                cells[r, c] = cell;
            }
        }
    }

    List<string> GenerateDaemon()
    {
        var seq = new List<string>();
        for (int i = 0; i < sequenceLength; i++)
            seq.Add(hexCodes[URandom.Range(0, hexCodes.Count)]);
        return seq;
    }

    void OnCellClick(int row, int col)
    {
        if (currentState == GameState.Finished) return;
        string clicked = gridData[row, col];

        //  첫 클릭 로직을 제거하고, 클릭 시 로직만 남김
        bool inCross = (row == lastRow) || (col == lastCol);
        if (matchedSequence.Count > 0 && !inCross)
        {
            ApplyMistakePenaltyIfNeeded();
            Reshuffle(reshuffleChangesDaemon);
            return;
        }
        if (clicked != daemonSequence[matchIndex])
        {
            ApplyMistakePenaltyIfNeeded();
            Reshuffle(reshuffleChangesDaemon);
            return;
        }

        SelectCell(row, col);
        matchedSequence.Add(clicked);
        matchIndex++;
        bufferText.text = string.Join(" ", matchedSequence);

        if (TryComplete()) return;

        UpdateCrossHighlights();
    }

    void SelectCell(int row, int col)
    {
        cells[row, col].SetState(CellState.Selected, selectedColor);
        logText.text += gridData[row, col] + "\n";
        lastRow = row; lastCol = col;
    }

    void UpdateCrossHighlights()
    {
        for (int r = 0; r < gridRows; r++)
        {
            for (int c = 0; c < gridCols; c++)
            {
                if (cells[r, c].currentState == CellState.Selected) continue;
                bool highlight = (r == lastRow) || (c == lastCol);
                cells[r, c].SetState(highlight ? CellState.Highlighted : CellState.Disabled,
                                     highlight ? highlightColor : disabledColor);
            }
        }
    }

    bool TryComplete()
    {
        if (matchIndex >= daemonSequence.Count)
        {
            SuccessGame();
            return true;
        }
        return false;
    }

    void SuccessGame()
    {
        currentState = GameState.Finished;
        infoText.text += "\n<color=green>SUCCESS!</color>";
        foreach (var cell in cells)
            cell.GetComponent<Button>().interactable = false;

        HideUI(gridPanel.gameObject, bufferText.gameObject, logText.gameObject, timerText.gameObject, infoText.gameObject);
        ShowUI(successUI); //  성공 시 successUI를 표시하도록 수정
    }

    void TimeOutFail()
    {
        currentState = GameState.Finished;
        infoText.text += "\n<color=red>ACCESS DENIED</color>\nReason: Time Over... 실패...";
        foreach (var cell in cells)
            cell.GetComponent<Button>().interactable = false;

        HideUI(gridPanel.gameObject, bufferText.gameObject, logText.gameObject, timerText.gameObject, infoText.gameObject);
        ShowUI(retryGameUI); //  실패 시 retryGameUI를 표시하도록 유지
    }

    void ApplyMistakePenaltyIfNeeded()
    {
        if (!useMistakePenalty) return;
        timer -= mistakePenaltySeconds;
        if (timer < minTimerAfterPenalty) timer = minTimerAfterPenalty;
        timerText.text = timer.ToString("F2");
        logText.text += $"[벌칙 -{mistakePenaltySeconds}s]\n";
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //  UI 활성화/비활성화 도우미 함수
    private void ShowUI(params GameObject[] UIs)
    {
        foreach (var ui in UIs)
        {
            if (ui != null)
            {
                ui.SetActive(true);
            }
        }
    }

    private void HideUI(params GameObject[] UIs)
    {
        foreach (var ui in UIs)
        {
            if (ui != null)
            {
                ui.SetActive(false);
            }
        }
    }
}