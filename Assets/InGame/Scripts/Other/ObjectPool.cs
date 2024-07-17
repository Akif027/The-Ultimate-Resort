using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public class PooledObject
    {
        public string name;
        public GameObject prefab;
        public Transform parent;
        public int initialSize = 10;
        public int maxSize = 20;
    }

    public List<PooledObject> pooledObjects;
    private Dictionary<string, Queue<GameObject>> availableObjects = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, HashSet<GameObject>> reservedObjects = new Dictionary<string, HashSet<GameObject>>();

    private void Awake()
    {
        foreach (PooledObject pooledObject in pooledObjects)
        {
            availableObjects[pooledObject.name] = new Queue<GameObject>();
            reservedObjects[pooledObject.name] = new HashSet<GameObject>();

            for (int i = 0; i < pooledObject.initialSize; i++)
            {
                GameObject obj = Instantiate(pooledObject.prefab);

                if (pooledObject.parent != null)
                {
                    obj.transform.SetParent(pooledObject.parent);
                }

                obj.SetActive(false);
                availableObjects[pooledObject.name].Enqueue(obj);
            }
        }
    }
    // public void AddPooledObject(string name, GameObject prefab, Transform parent = null, int initialSize = 10, int maxSize = 20)
    // {
    //     // Create a new PooledObject instance with the provided parameters
    //     PooledObject newPooledObject = new PooledObject
    //     {
    //         name = name,
    //         prefab = prefab,
    //         parent = parent,
    //         initialSize = initialSize,
    //         maxSize = maxSize
    //     };

    //     // Add the new PooledObject to the pooledObjects list
    //     pooledObjects.Add(newPooledObject);

    //     // Initialize the new PooledObject
    //     availableObjects[name] = new Queue<GameObject>();
    //     reservedObjects[name] = new HashSet<GameObject>();

    //     // Instantiate the initial number of objects for the new pooled object
    //     for (int i = 0; i < initialSize; i++)
    //     {
    //         GameObject obj = Instantiate(prefab);

    //         if (parent != null)
    //         {
    //             obj.transform.SetParent(parent);
    //         }

    //         obj.SetActive(false);
    //         availableObjects[name].Enqueue(obj);
    //     }
    // }
    public GameObject GetPooledObject(string name)
    {
        if (availableObjects.ContainsKey(name))
        {
            Debug.Log($"Requesting object from pool '{name}'. Available count: {availableObjects[name].Count}");
            if (availableObjects[name].Count > 0)
            {
                GameObject obj = availableObjects[name].Dequeue();
                reservedObjects[name].Add(obj);
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.Log($"No available objects in pool '{name}'. Checking if new objects can be instantiated.");
                foreach (PooledObject pooledObject in pooledObjects)
                {
                    if (pooledObject.name == name && reservedObjects[name].Count < pooledObject.maxSize)
                    {
                        GameObject obj = Instantiate(pooledObject.prefab);

                        if (pooledObject.parent != null)
                        {
                            obj.transform.SetParent(pooledObject.parent);
                        }

                        reservedObjects[name].Add(obj);
                        obj.SetActive(true);
                        return obj;
                    }
                }
            }
        }
        Debug.LogError($"Max size reached for {name} or object not found.");
        return null;
    }

    public void ReturnObjectToPool(GameObject obj, string name)
    {
        // Debug.Log($"Returning object to pool '{name}'. Current reserved count: {reservedObjects[name].Count}");

        obj.SetActive(false);
        reservedObjects[name].Remove(obj);
        availableObjects[name].Enqueue(obj);

    }

    protected void OnDestroy()
    {
        foreach (var pair in reservedObjects)
        {
            HashSet<GameObject> set = pair.Value;
            foreach (var obj in set)
            {
                Destroy(obj);
            }
        }
    }
}
