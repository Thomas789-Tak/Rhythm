using System.Collections;
using UnityEngine;

[System.Serializable]
public class Stage
{

    [SerializeField] private string theme;
    [SerializeField] private int level;
    [SerializeField] private bool isOpend;

    [SerializeField] private bool isCleared;
    [SerializeField] private float highScore;

    public Stage(string theme, int level, bool isOpend, bool isCleared = false, float highScore = 0)
    {
        this.theme = theme;
        this.level = level;
        this.isOpend = isOpend;
        this.isCleared = isCleared;
        this.highScore = highScore;
    }

    public string Theme { get => theme; private set => theme = value; }
    public int Level { get => level; private set => level = value; }
    public bool IsOpend { get => isOpend; private set => isOpend = value; }
    public bool IsCleared { get => isCleared; private set => isCleared = value; }
    public float HighScore { get => highScore; private set => highScore = value; }
}