using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    Debug.LogError("SoundManager singelton Error");
                }
            }
            return instance;
        }
    }

    [Header("AudioSource")]
    [SerializeField]
    AudioSource bgm;

    [SerializeField]
    public AudioSource sfx;


    public enum EBGM
    {
        _90BPM_WakeUp,
        _108BPM_IfI,
        _180BPM_Turn,
        _196BPM_Bluedy,

    }

    public enum ESFX
    {
        EngineStartSound,
        CarDriftSound,
    }

    [Header("BGM")]
    [SerializeField]
    AudioClip bgm_WakeUp;

    [SerializeField]
    AudioClip bgm_IfI;

    [SerializeField]
    AudioClip bgm_Turn;

    [SerializeField]
    AudioClip bgm_Bluedy;

    [Header("SFX")]

    [SerializeField]
    AudioClip sfx_EngineStartSound;

    [SerializeField]
    AudioClip sfx_DriftSound;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayBGM(EBGM value, bool isLoop = false)
    {
        SetBGM(value);
        bgm.loop = isLoop;
        bgm.Play();
    }

    public void StopBGM(EBGM value)
    {
        SetBGM(value);
        bgm.Stop();
    }


    public void PlayOneShotSFX(ESFX value)
    {
        AudioClip aclip = SetSFX(value);

        if (aclip)
            sfx.PlayOneShot(aclip);
    }

    void SetBGM(EBGM value)
    {
        switch (value)
        {
            case EBGM._90BPM_WakeUp:
                bgm.clip = bgm_WakeUp;
                break;
            case EBGM._108BPM_IfI:
                bgm.clip = bgm_IfI;
                break;
            case EBGM._180BPM_Turn:
                bgm.clip = bgm_Turn;
                break;
            case EBGM._196BPM_Bluedy:
                bgm.clip = bgm_Bluedy;
                break;
        }
    }

    AudioClip SetSFX(ESFX value)
    {
        switch (value)
        {
            case ESFX.EngineStartSound:
                return sfx_EngineStartSound;
            case ESFX.CarDriftSound:
                return sfx_DriftSound;
            default:
                return null;
        }
    }
}
