using System.Collections;
using UnityEngine;

// 게임의 사운드를 관리하는 싱글톤 매니저. BGM + SFX 관리
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float bgmVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Header("Default BGM")]
    public AudioClip defaultBGM;
    public float defaultBGMVolume = 0.5f;

    [Header("SFX Clips")]
    public AudioClip walkSFX;
    public AudioClip runSFX;
    public AudioClip jumpSFX;

    // AudioSource 참조 대신 직접 컴포넌트 보유
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // BGM AudioSource 생성
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = bgmVolume;
            bgmSource.name = "BGM_AudioSource";

            // SFX AudioSource 생성 (완전히 분리)
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = sfxVolume;
            sfxSource.name = "SFX_AudioSource";

            UnityEngine.Debug.Log($"AudioSources 생성 완료: BGM={bgmSource.GetInstanceID()}, SFX={sfxSource.GetInstanceID()}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 볼륨 초기 설정 적용
        ApplyVolumeSettings();
        PlayDefaultBGM();
    }

    /// 현재 볼륨 설정을 AudioSource에 적용
    private void ApplyVolumeSettings()
    {
        // BGM 볼륨 
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }

        // SFX 볼륨 - BGM과 완전히 분리
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    /// BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
    }

    /// SFX 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    /// BGM 재생 (BGM 전용 AudioSource 사용)
    public void PlayBGM(AudioClip clip, float volumeScale = 0.7f, bool loop = true)
    {
        if (clip == null || bgmSource == null) return;

        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume * volumeScale;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlayDefaultBGM(float volumeScale = -1f)
    {
        if (bgmSource == null) return;
        if (bgmSource.clip == defaultBGM && bgmSource.isPlaying) return;

        float targetVolumeScale = (volumeScale >= 0) ? volumeScale : defaultBGMVolume;
        PlayBGM(defaultBGM, targetVolumeScale);
    }

    // SFX 관련 메서드 - SFX 전용 AudioSource 사용
    /// SFX 재생 (SFX 전용 AudioSource 사용)
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null || sfxSource == null) return;

        float finalVolume = sfxVolume * volumeScale;
        sfxSource.PlayOneShot(clip, finalVolume);
    }

    /// 미리 지정한 효과음 이름으로 호출
    public void PlaySFXByName(string name)
    {
        switch (name)
        {
            case "walk": PlaySFX(walkSFX); break;
            case "run": PlaySFX(runSFX); break;
            case "jump": PlaySFX(jumpSFX); break;
            default:
                UnityEngine.Debug.LogWarning($"SFX '{name}' not found!");
                break;
        }
    }

    /// 특정 볼륨으로 SFX 재생
    public void PlaySFXByName(string name, float volumeScale)
    {
        switch (name)
        {
            case "walk": PlaySFX(walkSFX, volumeScale); break;
            case "run": PlaySFX(runSFX, volumeScale); break;
            case "jump": PlaySFX(jumpSFX, volumeScale); break;
            default:
                UnityEngine.Debug.LogWarning($"SFX '{name}' not found!");
                break;
        }
    }

    // 나머지 메서드들...
    public Coroutine StopBGMWithFadeOut(float fadeTime)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
        return currentFadeCoroutine;
    }

    public Coroutine FadeToBGM(AudioClip clip, float targetVolumeScale, float fadeOutTime, float fadeInTime)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(FadeToBGMCoroutine(clip, targetVolumeScale, fadeOutTime, fadeInTime));
        return currentFadeCoroutine;
    }

    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    public void PauseBGM()
    {
        if (bgmSource != null)
            bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        if (bgmSource != null)
            bgmSource.UnPause();
    }

    private IEnumerator FadeOutBGM(float fadeTime)
    {
        if (bgmSource == null) yield break;

        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            if (bgmSource != null)
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }
        if (bgmSource != null)
        {
            bgmSource.volume = 0f;
            bgmSource.Stop();
        }
    }

    private IEnumerator FadeInBGM(AudioClip clip, float targetVolumeScale, float fadeTime)
    {
        if (clip == null || bgmSource == null) yield break;

        bgmSource.clip = clip;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float finalTargetVolume = bgmVolume * targetVolumeScale;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            if (bgmSource != null)
                bgmSource.volume = Mathf.Lerp(0f, finalTargetVolume, t / fadeTime);
            yield return null;
        }
        if (bgmSource != null)
            bgmSource.volume = finalTargetVolume;
    }

    private IEnumerator FadeToBGMCoroutine(AudioClip clip, float targetVolumeScale, float fadeOutTime, float fadeInTime)
    {
        if (bgmSource != null && bgmSource.isPlaying)
            yield return FadeOutBGM(fadeOutTime);

        yield return FadeInBGM(clip, targetVolumeScale, fadeInTime);
        currentFadeCoroutine = null;
    }

    /// 모든 사운드 일시정지
    public void PauseAll()
    {
        PauseBGM();
        if (sfxSource != null)
            sfxSource.Pause();
    }

    /// 모든 사운드 재개
    public void ResumeAll()
    {
        ResumeBGM();
        if (sfxSource != null)
            sfxSource.UnPause();
    }

    /// 모든 사운드 정지
    public void StopAll()
    {
        StopBGM();
        if (sfxSource != null)
            sfxSource.Stop();
    }

    /// SFX 일시정지
    public void PauseSFX()
    {
        if (sfxSource != null)
            sfxSource.Pause();
    }

    /// SFX 재개
    public void ResumeSFX()
    {
        if (sfxSource != null)
            sfxSource.UnPause();
    }

    /// SFX 정지
    public void StopSFX()
    {
        if (sfxSource != null)
            sfxSource.Stop();
    }

    // 디버깅용 메서드
    public void PrintAudioSourceInfo()
    {
        UnityEngine.Debug.Log($"BGM Source: {bgmSource.GetInstanceID()}, Volume: {bgmSource.volume}");
        UnityEngine.Debug.Log($"SFX Source: {sfxSource.GetInstanceID()}, Volume: {sfxSource.volume}");
    }
}