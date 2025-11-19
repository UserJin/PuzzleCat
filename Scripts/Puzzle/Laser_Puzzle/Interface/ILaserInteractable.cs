using UnityEngine;

namespace LaserPuzzle
{
    /// <summary>
    /// 레이저와 상호작용이 가능하게 하는 인터페이스
    /// </summary>
    public interface ILaserInteractable
    {
        void OnLaserHit(Game.Common.LaserHitInfo laserHitInfo);
    }
}