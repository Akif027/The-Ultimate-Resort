using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;


    public bool ApplicationIsQuitting
    {
        get { return _applicationIsQuitting; }
    }
    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] More than one instance of " + typeof(T) +
                            " found. There should never be more than one singleton!");
                        DestroyImmediate(_instance.gameObject); // Ensure only one instance remains
                        return null;
                    }
                }

                return _instance;
            }
        }
    }

    // protected virtual void OnDestroy()
    // {
    //     if (_instance == this)
    //     {
    //         _instance = null;
    //     }
    // }

    // protected virtual void OnApplicationQuit()
    // {
    //     _applicationIsQuitting = true;
    // }

    // protected virtual void OnDisable()
    // {
    //     if (_instance == this)
    //     {
    //         _instance = null;
    //     }
    //     _applicationIsQuitting = true;
    // }
}
