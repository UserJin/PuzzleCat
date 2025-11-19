using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    public void Show(string text)
    {
        label.text = text;
        label.gameObject.SetActive(true);
    }

    public void Hide()
    {
        label.gameObject.SetActive(false);
    }
}
