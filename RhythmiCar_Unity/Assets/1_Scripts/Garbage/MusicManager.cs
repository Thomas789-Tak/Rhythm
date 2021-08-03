using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager: MonoBehaviour // 노래의 패턴에 대한 정보를 갖고 있는 클래스
{
    private static MusicManager instance = null;
    public Image LP1;
    public Image LP2;
    public Image TimingArea;
    public RectTransform StartPos;
    public Transform NoteParent;
    public Image noteInstance;
    public Image[] Notes;

    // ---------노래 리스트 ---------
    public AudioSource wakeUp;
    public AudioSource ifI;

    //---------효과음 리스트 ---------
    public AudioSource Skid;
    public AudioSource GearChange;
    public AudioSource Error;
    //--------wakeup 변수 -----------
    int wakeupCurrentMainCount;
    int wakeupCurrentBeatCount;
    public float wakeupMusicTime;
    bool isWakeupPlaying;
    public bool isDelayFirstNote;
    float wakeupNoteDelay;
    int wakeupBeatCount;

    //---------ifI 변수--------------
    int ifIBeatCount;
    int ifICurrentBeatCount;
    int ifICurrentMainCount;
    float ifIMusicTime;
    bool isIfIPlaying;



    private void Awake()
    {
        wakeupNoteDelay = 2f;
        wakeupBeatCount = 63;
        ifIBeatCount = 144;
        ifICurrentBeatCount = wakeupBeatCount + 1;
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        for (int i=0; i<300; i++)
        {
            Notes[i]=Instantiate(noteInstance, transform.position, Quaternion.Euler(0,0,90));
            Notes[i].rectTransform.position = StartPos.position;
            Notes[i].transform.SetParent(NoteParent);
            Notes[i].gameObject.SetActive(false);
        }
    }

    public static MusicManager Instance
    {
        get
        {
            if(instance==null)
            {
                return null;
            }
            return instance;
        }
    }

    public void PlayWakeUp()
    {
        
        if(isWakeupPlaying==false)
        {

            LP1.gameObject.SetActive(true);
            TimingArea.enabled = true;
            wakeUp.Play();
            isWakeupPlaying = true;
        }
        if(isWakeupPlaying)
        {
            if (wakeupMusicTime > 0f && isDelayFirstNote == false)
            {
                Notes[0].transform.SetParent(LP1.transform);
                Notes[0].gameObject.SetActive(true);
                isDelayFirstNote = true;
            }
            if (wakeupCurrentBeatCount < 7)
            {
                if (wakeupMusicTime >= 2.665d * (wakeupCurrentMainCount + 1))
                {
                    Notes[wakeupCurrentBeatCount + 1].gameObject.SetActive(true);
                    Notes[wakeupCurrentBeatCount + 1].transform.SetParent(LP1.rectTransform);
                    wakeupCurrentMainCount++;
                    wakeupCurrentBeatCount++;
                }
            }
            else if(wakeupCurrentBeatCount>6 && wakeupCurrentBeatCount<62)
            {
                if (wakeupMusicTime >= 2.665d * (wakeupCurrentMainCount + 1) - 1.33d)
                {
                    Notes[wakeupCurrentBeatCount + 1].gameObject.SetActive(true);
                    Notes[wakeupCurrentBeatCount + 1].transform.SetParent(LP1.rectTransform);
                    if (wakeupMusicTime >= 2.665d * (wakeupCurrentMainCount + 1))
                    {
                        Notes[wakeupCurrentBeatCount + 2].gameObject.SetActive(true);
                        Notes[wakeupCurrentBeatCount + 2].transform.SetParent(LP1.rectTransform);
                        wakeupCurrentBeatCount += 2;
                        wakeupCurrentMainCount++;
                    }
                }
                if(wakeupCurrentBeatCount==31)
                {
                    Notes[31].transform.SetParent(null);
                    Notes[31].gameObject.SetActive(false);
                }
            }
            LP1.rectTransform.rotation = Quaternion.Euler(0, 0, -wakeupMusicTime * 53.658f);
        }
        wakeupMusicTime = wakeUp.time-wakeupNoteDelay;
    }
    public void PauseWakeUp()
    {
        wakeUp.Pause();
        isWakeupPlaying = false;
        LP1.gameObject.SetActive(false);
        TimingArea.enabled = false;
    }

    public void PauseIfI()
    {
        ifI.Pause();
        isIfIPlaying = false;
        LP2.gameObject.SetActive(false);
        TimingArea.enabled = false;
    }

    public void PlayIfI()
    {
        if (isIfIPlaying == false)
        {
            ifI.Play();
            LP2.gameObject.SetActive(true);
            TimingArea.enabled = true;
            isIfIPlaying = true;
        }
        if (isIfIPlaying)
        {
            if (ifICurrentBeatCount < wakeupBeatCount+1+ifIBeatCount)
            {
                if (ifIMusicTime >= 0.555d * (ifICurrentMainCount + 1))
                {
                    Notes[ifICurrentBeatCount].gameObject.SetActive(true);
                    Notes[ifICurrentBeatCount].transform.SetParent(LP2.rectTransform);
                    ifICurrentMainCount++;
                    ifICurrentBeatCount++;
                }
            }
            LP2.rectTransform.rotation = Quaternion.Euler(0, 0, -ifIMusicTime * 64.4144f);//257.6576f);
        }
        ifIMusicTime = ifI.time;
    }

}

