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
    AudioSource sfx;


    public enum EBGM
    {
        INTRO,
        INTRO_ANGRY,
        TITLE,
        IN_GAME,
        GAME_OVER_BELOW_SCORE,
        GAME_OVER_OVER_SCORE,
    }

    public enum ESFX
    {
        NONE,
        SPACE_BAR,
        WATER_SPLASH,
        SELECT,
        COUNT_DOWN_INTRO,
        COUNT_DOWN_ONE,
        COUNT_DOWN_TWO,
        COUNT_DOWN_THREE,
    }

    [Header("BGM")]
    [SerializeField]
    AudioClip bgm_introClip;

    [SerializeField]
    AudioClip bgm_introAngryClip;

    [SerializeField]
    AudioClip bgm_titleClip;

    [SerializeField]
    AudioClip bgm_inGameClip;

    [SerializeField]
    AudioClip bgm_gameOverBelowScoreClip;

    [SerializeField]
    AudioClip bgm_gameOverOverScoreClip;

    [Header("SFX")]
    [SerializeField]
    AudioClip sfx_spaceBar;

    [SerializeField]
    AudioClip sfx_water;

    [SerializeField]
    AudioClip sfx_select;

    [SerializeField]
    AudioClip sfx_countdown_intro;

    [SerializeField]
    AudioClip sfx_countdown_one;

    [SerializeField]
    AudioClip sfx_countdown_two;

    [SerializeField]
    AudioClip sfx_countdown_three;

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
            case EBGM.INTRO:
                bgm.clip = bgm_introClip;
                break;
            case EBGM.INTRO_ANGRY:
                bgm.clip = bgm_introAngryClip;
                break;
            case EBGM.TITLE:
                bgm.clip = bgm_titleClip;
                break;
            case EBGM.IN_GAME:
                bgm.clip = bgm_inGameClip;
                break;
            case EBGM.GAME_OVER_BELOW_SCORE:
                bgm.clip = bgm_gameOverBelowScoreClip;
                break;
            case EBGM.GAME_OVER_OVER_SCORE:
                bgm.clip = bgm_gameOverOverScoreClip;
                break;
        }
    }

    AudioClip SetSFX(ESFX value)
    {
        switch (value)
        {
            case ESFX.SPACE_BAR:
                return sfx_spaceBar;
            case ESFX.WATER_SPLASH:
                return sfx_water;
            case ESFX.SELECT:
                return sfx_select;
            case ESFX.COUNT_DOWN_INTRO:
                return sfx_countdown_intro;
            case ESFX.COUNT_DOWN_ONE:
                return sfx_countdown_one;
            case ESFX.COUNT_DOWN_TWO:
                return sfx_countdown_two;
            case ESFX.COUNT_DOWN_THREE:
                return sfx_countdown_three;
            default:
                return null;
        }
    }
}
