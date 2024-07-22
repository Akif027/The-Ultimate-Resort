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



  // public override void Init()
  // {
  //   if (Settings.GameData == null)
  //   {
  //     Settings.GameData = gameData;
  //   }
  //   gameData = Settings.GameData;
  // }
  void Start()
  {

    GameManager.Instance.StartGame();

  }

  public void Save()
  {
    Settings.GameData = gameData;
  }


}


