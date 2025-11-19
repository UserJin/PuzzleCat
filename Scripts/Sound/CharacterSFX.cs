using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
    public void PlayWalkSFX()
    {
        SoundManager.instance.PlaySFXByName("walk");
    }

    public void PlayRunSFX()
    {
        SoundManager.instance.PlaySFXByName("run");
    }

    public void PlayJumpSFX()
    {
        SoundManager.instance.PlaySFXByName("jump");
    }
}
