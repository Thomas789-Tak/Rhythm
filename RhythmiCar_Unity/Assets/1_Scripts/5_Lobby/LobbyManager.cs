using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;


public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private DataManager dataManager;

    private void Awake()
    {
        //player = GetComponent<Player>();
        //dataManager = GetComponent<DataManager>();

    }


    public void GameStart()
    {
        LoadingSceneManager.LoadScene("Map1");
    }


}

