using System.Collections;

[System.Serializable]
public class Song
{
    public string name;
    
    public bool isOpend;
    public int openCost;
    
    public int bpm;
    public int maxSpeed;

    /// <summary>
    /// CSV에서 받아온 데이터를 사용하는 생성자
    /// 이곳에 없는 변수들은 FireBase에서 받아오도록 한다.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isOpend"></param>
    /// <param name="openCost"></param>
    /// <param name="bpm"></param>
    /// <param name="maxSpeed"></param>
    public Song(string name, bool isOpend, int openCost, int bpm, int maxSpeed)
    {
        this.name = name;
        this.isOpend = isOpend;
        this.openCost = openCost;
        this.bpm = bpm;
        this.maxSpeed = maxSpeed;
    }
}