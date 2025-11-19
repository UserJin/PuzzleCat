using Game.Common;
using UnityEngine;

/// <summary>
/// 레이저를 감지하면 장애물을 제거하는 함수
/// 기존 계획은 레이저가 감지될 때만 제거하지만, 이 경우 무한반복이 발생하여 한 번 감지하면 유지되는 것으로 변경
/// </summary>
public class Sensor : MonoBehaviour, LaserPuzzle.ILaserInteractable
{
    public Color targetColor;

    public Material material;
    public GameObject obstacle;

    public bool isActive = false;

    private LaserPuzzleManager manager;

    private void Start()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
        material.color = targetColor;

        obstacle = transform.parent.GetChild(1).gameObject;

        manager = transform.parent.GetComponentInParent<LaserPuzzleManager>();
    }

    public void OnLaserHit(LaserHitInfo laserHitInfo)
    {
        if (isActive) return;
        if(targetColor == laserHitInfo.laserColor)
        {
            obstacle.transform.localPosition = new Vector3(obstacle.transform.localPosition.x, -0.9f, obstacle.transform.localPosition.z);
            isActive = true;
            manager.RecalculateLaser();
        }
    }
}
