using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : GameManager
{



    private static AnimationManager _instance;

    private RoomManager roomManager;


    public static AnimationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find the existing instance in the scene
                _instance = FindObjectOfType<AnimationManager>();

                // If not found, create a new instance
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<AnimationManager>();
                }
            }
            return _instance;
        }
    }


    private AnimationManager() { }


    protected override void Initialize()
    {
        roomManager = GetComponent<RoomManager>();
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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

