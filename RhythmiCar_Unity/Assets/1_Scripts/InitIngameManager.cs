using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;


public static class InitIngameManager //���� ���� �� �� Ŭ������ ������ �������� �ʱ�ȭ ����
{
    public static Car selectedCar;
    public static List<Song> selectedSongs;
    public static Stage selectedStage;
    public static FirebaseAuth auth;

    /// <summary>
    /// ������ �޾ƿö��� ����
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
