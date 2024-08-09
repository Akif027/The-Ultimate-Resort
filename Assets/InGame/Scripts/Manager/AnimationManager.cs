using System;
using System.Collections.Generic;

using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{

    [SerializeField] GameObject Player;

    private static AnimationManager _instance;

    private RoomManager roomManager;




    private AnimationManager() { }


    void Start()
    {

        roomManager = GetComponent<RoomManager>();
        if (_instance == null)
        {
            _instance = this;
            //   DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

    }


    public void RoomPlayAllCleanAnimation(int roomNumber)
    {

        roomManager.roomData[roomNumber - 1].PlayAllCleanAnimation();
    }


    public void RoomPlayAllDirtyAnimation(int roomNumber)
    {

        roomManager.roomData[roomNumber - 1].PlayAllDirtyAnimation();
    }


}

