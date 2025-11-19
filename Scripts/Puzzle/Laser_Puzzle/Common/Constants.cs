using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    /// <summary>
    /// 게임에 사용되는 상수 값을 담은 클래스
    /// </summary>
    public static class Constants
    {
        public const float LASER_MAX_DISTANCE = 30f;
        public const float LASER_MAX_WIDTH = 0.2f;
    }

    /// <summary>
    /// 레이저 충돌 정보를 담는 구조체
    /// </summary>
    public struct LaserHitInfo
    {
        public Vector3 hitPoint;
        public Vector3 incomingDirection;
        public Vector3 hitNormal;
        public Color laserColor;

        public LaserHitInfo(Vector3 point, Vector3 dir, Vector3 normal, Color color)
        {
            hitPoint = point;
            incomingDirection = dir;
            hitNormal = normal;
            laserColor = color;
        }
    }

    /// <summary>
    /// 발사하는 레이저 정보를 담은 구조체
    /// </summary>
    [System.Serializable]
    public struct LaserRaycastInfo
    {
        public Vector3 originPos;
        public Vector3 raycastDirection;
        public Color laserColor;
        public float maxDistance;

        public LaserRaycastInfo(Vector3 pos, Vector3 dir, Color color, float dist)
        {
            originPos = pos;
            raycastDirection = dir;
            laserColor = color;
            maxDistance = dist;
        }
    }
}