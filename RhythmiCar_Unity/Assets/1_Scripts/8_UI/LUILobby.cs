﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LUILobby : Singleton<LUILobby>
{
    public RectTransform PanelParentRect;
    private float sizeX;

    public Image ImageCar;
    public Image ImgaeStage;

    public TextMeshProUGUI TextGold;
    public TextMeshProUGUI TextDiamond;

    [Header("Panel_Car")]
    public Image ImageCarBig;
    public TextMeshProUGUI TextCarLevel;
    public TextMeshProUGUI TextCarName;

    [SerializeField]
    private GameObject CarListContent;
    private string CarDataPath = "Data/Scriptable Object";
    private Car CurrentCar;

    [System.Serializable]
    /// <summary>
    /// 차량 패널에서 차량의 각 스탯을 바로 표현한다.
    /// 바는 Slider로 되어 있으며 Slider 내부에 Text는 구체적인 수치를 나타낸다.
    /// </summary>
    public struct StatBar
    {
        [SerializeField]
        private GameObject Stat;
        [SerializeField]
        private Slider bar;
        [SerializeField]
        private TextMeshProUGUI TextValue;

        public void SetMaxValue(float value)
        {
            bar.maxValue = value;
        }

        public void SetValue(float value)
        {
            bar.value = value;
            TextValue.text = value.ToString();
        }

        public void SetStat()
        {
            bar = Stat.transform.GetChild(1).GetComponent<Slider>();
            TextValue = Stat.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        }
    }

    public StatBar Acceleration;
    public StatBar Booster;
    public StatBar RhythmEnergy;
    public StatBar Handling;

    [Header("Panel_Main")]
    public int a;

    [Header("Panel_Stage")]
    public int c;



    private void Awake()
    {
        sizeX = PanelParentRect.parent.GetComponent<RectTransform>().sizeDelta.x;

        Acceleration.SetStat();
        Booster.SetStat();
        RhythmEnergy.SetStat();
        Handling.SetStat();
    }
    // Use this for initialization
    void Start()
    {
        SetPlayerData();

        // 패널 위치 세팅
        //sizeX = PanelParent.parent.GetComponent<RectTransform>().sizeDelta.x;
        SetPanelPosition();
        HeaderButtonDown(1);

        SetCarButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HeaderButtonDown(int btnNum)
    {
        float duration = 0.5f;
        var ease = Ease.OutExpo;

        //PanelParentRect.localPosition = 
        //    new Vector2(-btnNum * sizeX, PanelParentRect.localPosition.y);

        PanelParentRect.DOLocalMoveX(-btnNum * sizeX, duration).SetEase(ease);
    }

    public void SetPanelPosition()
    {
        for (int childNum = 0; childNum < PanelParentRect.childCount; childNum++)
        {
            var panel = PanelParentRect.GetChild(childNum);
            var rect = panel.GetComponent<RectTransform>();

            rect.localPosition = new Vector2(childNum * sizeX, rect.localPosition.y);
        }
    }

    public void SetPlayerData()
    {
        TextGold.text = 0.ToString();
        TextDiamond.text = 0.ToString();
    }


    #region Panel_Car

    public void SetCarButtons()
    {
        var cars = Resources.LoadAll(CarDataPath);
        var btns = CarListContent.GetComponentsInChildren<Button_Car>(true);

        for (int i = 0; i < cars.Length; i++)
        {
            if (i < btns.Length)
            {
                btns[i].SetCar((CarDataContainer)cars[i]);
            }
            else
            {
                var btn = Instantiate(btns[0], CarListContent.transform);
                btn.SetCar((CarDataContainer)cars[i]);
            }
        }

    }

    public void SelectCar(Car car)
    {
        


    }

    public void SelectCar(CarDataContainer carDataContainer)
    {

        //this.ImageCar.sprite = carDataContainer.ImageCar;
        this.ImageCarBig.sprite = carDataContainer.ImageCar;

        this.TextCarLevel.text = "Lv. " + carDataContainer.level.ToString();
        this.TextCarName.text = carDataContainer.carName;


    }

    


    #endregion Panel_Car


}