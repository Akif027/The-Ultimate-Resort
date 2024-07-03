using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Game : Manager
{

  public GameData gameData;
  void Start()
  {

    GameManager.Instance.StartGame();
  }
}


