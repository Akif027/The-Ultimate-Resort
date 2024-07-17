using System.Collections;
using System.Collections.Generic;
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
  void Start()
  {

    GameManager.Instance.StartGame();
  }
}


