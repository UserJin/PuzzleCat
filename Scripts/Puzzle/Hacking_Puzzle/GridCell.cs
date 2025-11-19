using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum CellState { Normal, Highlighted, Selected, Disabled }

public class GridCell : MonoBehaviour
{
    public int cellRow;
    public int cellCol;

    public Button button;
    public Image image;
    public TextMeshProUGUI codeText;

    public CellState currentState { get; private set; }

    void Awake()
    {
        if (!button) button = GetComponent<Button>();
        if (!image) image = GetComponent<Image>();
        if (!codeText) codeText = GetComponentInChildren<TextMeshProUGUI>();
        currentState = CellState.Normal;
    }

    // 🚨 코드와 함께 스프라이트를 받아와서 이미지에 적용합니다.
    public void Setup(string code, int r, int c, Sprite sprite, UnityAction onClickAction)
    {
        codeText.text = code;
        cellRow = r;
        cellCol = c;

        if (image != null)
        {
            image.sprite = sprite;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClickAction);
    }

    public void SetState(CellState state, Color color)
    {
        currentState = state;

        // 🚨 이미지의 색상을 변경합니다.
        if (image != null)
        {
            image.color = color;
        }

        button.interactable = !(state == CellState.Selected || state == CellState.Disabled);
    }
}