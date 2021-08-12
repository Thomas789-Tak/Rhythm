 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    List<SoundManager.EBGM> mySongEquipList = new List<SoundManager.EBGM>();
    void Start()
    {
        mySongEquipList.Add(SoundManager.EBGM._180BPM_Turn);
    }

    void Update()
    {
        
    }
}
