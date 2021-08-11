using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitIngameManager //씬에 들어올 때 이 클래스의 정보를 바탕으로 초기화 해줌
{
    // 플레이어 스탯
    public static List<SoundManager.EBGM> songEquipList = new List<SoundManager.EBGM>(); // 노래 장착 정보 Ex) 0번이 첫번째 곡, 해당하는 숫자가 enum에 해당하는 숫자  /// SoundManager에 해당하는 내용 참조할 것
    public static float acceleration { get; set; } // 가속도
    public static float boosterMaxGauge { get; set; } //최대 부스터 게이지(게이지 총량)
    public static float boosterSpeed { get; set; } // 부스터 가속 속도
    public static float turnStrength { get; set; }
    public static float brakeForce { get; set; }
    public static float friction { get; set; }
    public static float rhythmPower { get; set; }

    // 스테이지 정보
    public static int stagelevel { get; set; }

    // 자동차 종류
    public static int carType { get; set; }
}
