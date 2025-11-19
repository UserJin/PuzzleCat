using Game.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레이저 굴절 장치
/// </summary>
public class LaserRefractor : MonoBehaviour, LaserPuzzle.ILaserInteractable
{
    private LaserRaycaster raycaster;

    private void Awake()
    {
        raycaster = GetComponent<LaserRaycaster>();
    }

    // 레이저가 들어오면 이를 굴절시켜서 발사함
    public void OnLaserHit(LaserHitInfo laserHitInfo)
    {
        Vector3 refractVec = Quaternion.Euler(0f, 45f, 0f) * laserHitInfo.incomingDirection;

        if (raycaster != null)
        {
            LaserRaycastInfo raycastInfo = new LaserRaycastInfo(transform.position, refractVec, laserHitInfo.laserColor, Constants.LASER_MAX_DISTANCE);
            raycaster.AddLaserInfo(raycastInfo);
            raycaster.CastAllLaser();
        }
    }
}
