using Game.Common;
using UnityEngine;

/// <summary>
/// 레이저 반사 장치
/// 들어온 레이저의 반사 벡터가 기존 벡터와 평행하면 처리하지 않음
/// </summary>
public class LaserReflecter : RotatableObject, LaserPuzzle.IInteractable, LaserPuzzle.ILaserInteractable
{
    public void OnLaserHit(LaserHitInfo laserHitInfo)
    {
        // 반사되는 벡터 계산
        Vector3 reflectDir = Vector3.Reflect(laserHitInfo.incomingDirection, laserHitInfo.hitNormal);

        float dot = Vector3.Dot(reflectDir, laserHitInfo.incomingDirection);
        // 반사 벡터가 원 벡터와 평행하면 종료
        if (Mathf.Abs(dot) > 0.999f)
            return;

        if(raycaster != null)
        {
            LaserRaycastInfo raycastInfo = new LaserRaycastInfo(transform.position, reflectDir, laserHitInfo.laserColor, Constants.LASER_MAX_DISTANCE);
            raycaster.AddLaserInfo(raycastInfo);
            raycaster.CastAllLaser();
        }
    }
}
