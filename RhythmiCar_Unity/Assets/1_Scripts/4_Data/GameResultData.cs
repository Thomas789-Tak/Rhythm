using System.Collections;
using UnityEngine;

public static class GameResult
{
    public struct GameResultData
    {
        public bool isFirst;
        public float time;
        public float score;
        public int stage;
        public int gold;
        public int iNote;
        public int star;
        public float achievement;
    }

    private static GameResultData gameResultData;


    public static void SetResult(GameResultData newGameResultData)
    {
        gameResultData = newGameResultData;
    }
    
    public static GameResultData GetResult()
    {
        return gameResultData;
    }

}