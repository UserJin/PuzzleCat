using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레이저가 도달해야하는 목표 지점
/// 레이저의 색이 targetColor와 같아야 활성화됨
/// </summary>
public class LaserReceiver : MonoBehaviour, LaserPuzzle.ILaserInteractable
{
    public Color targetColor;
    [SerializeField] private bool isActive = false;

    private LaserPuzzleManager manager;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Material modelMaterial;

    private void Awake()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        targetMaterial = renderers[0].material;
        modelMaterial = renderers[1].material;
    }

    private void Start()
    {
        manager = transform.parent.GetComponentInParent<LaserPuzzleManager>();
        manager.OnObjectChange += ResetState;

        targetColor.a = 1;
        targetMaterial.color = targetColor;
    }

    public void Activate()
    {
        isActive = true;
        modelMaterial.color = targetColor;
        manager.CheckCompletion();
    }

    public void OnLaserHit(Game.Common.LaserHitInfo laserHitInfo)
    {
        if(laserHitInfo.laserColor == targetColor)
        {
            Activate();
        }
    }

    private void ResetState()
    {
        isActive = false;
        modelMaterial.color = Color.white;
    }

    public bool CheckActive() => isActive; 
}
