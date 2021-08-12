using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitIngameManager //씬에 들어올 때 이 클래스의 정보를 바탕으로 초기화 해줌
{
    public static Car selectedCar;
    public static List<Song> selectedSongs;
    public static Stage selectedStage;

    /// <summary>
    /// 정보를 받아올때의 예시
    /// </summary>
    public static void DataExample()
    {
        // Car DATA
        float acceleration = selectedCar.Acceleration;
        float carCurrentLevel = selectedCar.CurrentLevel;
        string name = selectedCar.Name;

        // Song DATA
        int currentSong = 2;
        var eBgm = selectedSongs[currentSong].EBGM;

        // Stage DATA
        int stageLevel = selectedStage.Level;
        var stageTheme = selectedStage.Theme;

    }
}
