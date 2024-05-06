using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomData", menuName = "Room Data", order = 1)]
public class RoomDataContainer : ScriptableObject
{
    public List<Room> rooms = new List<Room>();
}
