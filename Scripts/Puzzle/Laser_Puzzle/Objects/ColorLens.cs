using Game.Common;
using LaserPuzzle;
using UnityEngine;

/// <summary>
/// 컬러렌즈
/// 레이저가 들어오면 레이저의 진행방향으로 컬러렌즈와 같은 색의 레이저를 발사
/// </summary>
public class ColorLens : MonoBehaviour, ILaserInteractable
{
    [SerializeField] private Color color;

    private LaserRaycaster raycaster;
    private Material material;

    private void Awake()
    {
        raycaster = GetComponent<LaserRaycaster>();
        material = transform.GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
        color.a = 1f;
        material.color = color;
    }

    public void OnLaserHit(LaserHitInfo laserHitInfo)
    {
        if(raycaster != null)
        {
            LaserRaycastInfo raycastInfo = new LaserRaycastInfo(transform.position, laserHitInfo.incomingDirection, color, Constants.LASER_MAX_DISTANCE);
            raycaster.AddLaserInfo(raycastInfo);
            raycaster.CastAllLaser();
        }
    }
}
