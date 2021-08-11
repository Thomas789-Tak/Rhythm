using System.Collections;

[System.Serializable]
public class Stage
{
    public string theme;
    public int level;
    public bool isOpend;

    public bool isCleared;
    public float highScore;

    /// <summary>
    /// CSV에서 받아온 데이터를 사용하는 생성자
    /// 이곳에 없는 변수들은 FireBase에서 받아오도록 한다.
    /// </summary>
    /// <param name="theme"></param>
    /// <param name="level"></param>
    /// <param name="isOpend"></param>
    public Stage(string theme, int level, bool isOpend)
    {
        this.theme = theme;
        this.level = level;
        this.isOpend = isOpend;
    }
}