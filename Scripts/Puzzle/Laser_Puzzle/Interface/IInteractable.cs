using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaserPuzzle
{
    /// <summary>
    /// 플레이어의 마우스 입력과 상호작용을 가능하게 하는 인터페이스
    /// </summary>
    public interface IInteractable
    {
        public void OnClick();
    }
}
