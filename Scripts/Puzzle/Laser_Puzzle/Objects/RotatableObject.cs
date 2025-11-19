using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이어가 클릭해서 회전시킬 수 있는 오브젝트 클래스
/// </summary>
public class RotatableObject : MonoBehaviour, LaserPuzzle.IInteractable
{
    protected LaserPuzzleManager manager;

    protected bool canInteract = true;
    protected LaserRaycaster raycaster;

    private void Awake()
    {
        raycaster = GetComponent<LaserRaycaster>();
    }

    private void Start()
    {
        manager = transform.parent.GetComponentInParent<LaserPuzzleManager>();
    }

    public virtual void OnClick()
    {
        if (canInteract)
        {
            // 회전 동기화 문제 있음
            // 현재는 계산을 특정 시간 이후에 진행하여 최대한 자연스럽게 연출
            transform.Rotate(new Vector3(0f, 45f, 0f));
            manager.RecalculateLaser();
        }
    }

}
