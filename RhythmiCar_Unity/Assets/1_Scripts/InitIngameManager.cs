using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

[System.Serializable]
public static class InitIngameManager //���� ���� �� �� Ŭ������ ������ �������� �ʱ�ȭ ����
{
    public static Car selectedCar;
    public static List<Song> selectedSongs;
    public static Stage selectedStage;

    public static FirebaseAuth auth;
    public static Player player;

    /// <summary>
    /// ������ �޾ƿö��� ����
    /// </summary>
    public static void DataExample()
    {
        // Car DATA
        float acceleration = selectedCar.Acceleration;
        float carCurrentLevel = selectedCar.CurrentLevel;
        string name = selectedCar.CarName;

        // Song DATA
        int currentSong = 2;
        var eBgm = selectedSongs[currentSong].EBGM;

        // Stage DATA
        int stageLevel = selectedStage.Level;
        var stageTheme = selectedStage.Theme;


    }
}
