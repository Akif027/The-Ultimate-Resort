using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void RoomRequestEventHandler(GameObject customer);
    private static event RoomRequestEventHandler onRoomRequested;

    public static void Subscribe(RoomRequestEventHandler handler)
    {
        onRoomRequested += handler;
    }

    public static void Unsubscribe(RoomRequestEventHandler handler)
    {
        onRoomRequested -= handler;
    }

    public static void InvokeRoomRequested(GameObject customer)
    {
        onRoomRequested?.Invoke(customer);
    }
}