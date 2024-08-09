using System.Collections;
using UnityEngine;

public static class VibrationManager
{
    private static bool _isVibrating;
    private static MonoBehaviour _coroutineRunner;

    // Method to start continuous vibration
    public static void StartVibration(MonoBehaviour coroutineRunner)
    {
        if (_coroutineRunner == null)
        {
            _coroutineRunner = coroutineRunner;
        }

        if (!_isVibrating)
        {
            _isVibrating = true;
            _coroutineRunner.StartCoroutine(VibrateinLoop());
        }
    }

    // Method to stop vibration
    public static void StopVibration()
    {
        _isVibrating = false;
    }
    public static void Vibrateit()
    {
        Handheld.Vibrate();
    }

    private static IEnumerator VibrateinLoop()
    {
        while (_isVibrating)
        {
            Handheld.Vibrate();
            yield return null; // Wait until the next frame to keep looping
        }
    }
}
