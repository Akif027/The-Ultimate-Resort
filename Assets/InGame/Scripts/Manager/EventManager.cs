using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void RoomRequestEventHandler(GameObject customer);
    private static event RoomRequestEventHandler onRoomRequested;

    public delegate void ToiletRequestEventHandler(customer customer);
    private static event ToiletRequestEventHandler onToiletRequested;

    public delegate void SwimmingPoolRequestEventHandler(customer customer);
    private static event SwimmingPoolRequestEventHandler onSwimmingPoolRequested;

    public static void SubscribeRoomRequest(RoomRequestEventHandler handler)
    {
        onRoomRequested += handler;
    }

    public static void UnsubscribeRoomRequest(RoomRequestEventHandler handler)
    {
        onRoomRequested -= handler;
    }

    public static void InvokeRoomRequested(GameObject customer)
    {
        onRoomRequested?.Invoke(customer);
    }

    public static void SubscribeToiletRequest(ToiletRequestEventHandler handler)
    {
        onToiletRequested += handler;
    }

    public static void UnsubscribeToiletRequest(ToiletRequestEventHandler handler)
    {
        onToiletRequested -= handler;
    }

    public static void InvokeToiletRequested(customer customer)
    {
        onToiletRequested?.Invoke(customer);
    }



    public static void SubscribeSwimmingPoolRequest(SwimmingPoolRequestEventHandler handler)
    {
        onSwimmingPoolRequested += handler;
    }

    public static void UnsubscribeSwimmingPoolRequest(SwimmingPoolRequestEventHandler handler)
    {
        onSwimmingPoolRequested -= handler;
    }

    public static void InvokeSwimmingPoolRequested(customer customer)
    {
        onSwimmingPoolRequested?.Invoke(customer);
    }

}