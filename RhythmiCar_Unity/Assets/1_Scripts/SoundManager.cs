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

    [Header("[[AudioSource]]")]
    [SerializeField]
    AudioSource bgm;

    public AudioSource sfx;

    public AudioSource sfx_Loop;


    public enum EBGM
    {
        _90BPM_WakeUp,
        _108BPM_IfI,
        _180BPM_Turn,
        _196BPM_SuddenShower,

    }

    public enum ESFX
    {
        EngineStartSound,
    }

    public enum ESFX_Loop
    {
        CarDriftSound,
    }

    [Header("[BGM]")]
    [SerializeField]
    AudioClip bgm_WakeUp;

    [SerializeField]
    AudioClip bgm_IfI;

    [SerializeField]
    AudioClip bgm_Turn;

    [SerializeField]
    AudioClip bgm_SuddenShower;

    [Header("[SFX]")]

    [SerializeField]
    AudioClip sfx_EngineStartSound;

    [Header("[SFX_Loop]")]

    [SerializeField]
    AudioClip sfx_Loop_DriftSound;

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

    public void PlayLoopSFX(ESFX_Loop value)
    {
        AudioClip bclip = SetSFX_Loop(value);
        if (bclip&&sfx.isPlaying==false)
            sfx.PlayOneShot(bclip);
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
            case EBGM._196BPM_SuddenShower:
                bgm.clip = bgm_SuddenShower;
                break;
        }
    }

    AudioClip SetSFX(ESFX value)
    {
        switch (value)
        {
            case ESFX.EngineStartSound:
                return sfx_EngineStartSound;
            default:
                return null;
        }
    }

    AudioClip SetSFX_Loop(ESFX_Loop value)
    {
        switch (value)
        {
            case ESFX_Loop.CarDriftSound:
                return sfx_Loop_DriftSound;
            default:
                return null;
        }
    }
}
