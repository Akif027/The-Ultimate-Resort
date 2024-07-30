using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Game : Manager
{
  private static Game instance;

  public static Game Instance
  {
    get
    {
      if (instance == null)
      {
        instance = (Game)GameManager.Instance.GetManager<Game>();
      }

      return instance;
    }
  }

  public GameData gameData;



  public override void Init()
  {
    Settings.LevelMax = 3;
    gameData.CurrentLevel = (Level)Settings.CurrentLevel;

    if (gameData.CurrentLevel == 0) // if the there is no save data 
    {
      gameData.CurrentLevel = Level.Level_1;
    }

  }
  void Start()
  {

    GameManager.Instance.StartGame();


  }

  public void Save()
  {
    Settings.GameData = gameData;
  }

  void OnDisable()
  {
    Settings.CurrentLevel = (int)UpgradeManager.Instance.CurrentLevel();
  }
}


