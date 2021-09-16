using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;


public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private Player player;
    [SerializeField] private DataManager dataManager;

    private void Awake()
    {
        //player = GetComponent<Player>();
        dataManager = GetComponent<DataManager>();

        if (JoinManager.Instance)
        {
            JoinManager.Instance.GetPlayer(out player.auth);
        }
    }
}

