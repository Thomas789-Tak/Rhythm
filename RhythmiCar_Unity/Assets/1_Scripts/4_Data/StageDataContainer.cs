using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StageDataContainer", menuName = "Scriptable Object/Stage Data Container")]
[System.Serializable]
public class StageDataContainer : ScriptableObject
{
    public int stageNum;
    public string stageName;
    public Sprite ImageStage;
    //public int level;
    public float time;
    public int score;
}