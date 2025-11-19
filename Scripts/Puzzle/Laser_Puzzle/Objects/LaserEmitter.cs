using UnityEngine;

/// <summary>
/// 레이저 발사 장치
/// </summary>
public class LaserEmitter : MonoBehaviour
{
    [SerializeField] private Color laserColor = Color.red;
    [SerializeField] private float maxDistance = Game.Common.Constants.LASER_MAX_DISTANCE;

    public Transform laserEmitMuzzle;

    private LaserRaycaster raycaster;

    private void Awake()
    {
        raycaster = GetComponent<LaserRaycaster>();
    }

    private void Start()
    {
        laserColor.a = 1f;
    }

    public void FireLaser()
    {
        if (raycaster != null)
            raycaster.CastLaser(laserEmitMuzzle.position, laserEmitMuzzle.forward, laserColor, maxDistance);
    }
}
