using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class SMITH_PS_W10 : MonoBehaviour
{
    /*
     * Create an object pool implementation.
     *
     * Requirements:
     *     - When prefab is added, create multiples and set them inactive
     *     - When Spawn() is called, either turn an inactive object on, or if there isn't one, spawn another
     *     - When Despawn() is called on an object, return it to the object pool
     *
     * Extra Ideas:
     *     Make a class that inherits from monobehavior to replace Start and Destroy
     *         - Initialization to replace "Start()"
     *         - Despawn to replace "Destroy()"
     *     Add position and rotation to Spawn()
     *     Allow there to be multiple kinds of prefabs tracked in the pool
     *     Make Despawn track what type of object was spawned/despawned and return to the right pool
     */

    public GameObject prefabToPool;
    public void Start()
    {
        ObjectPool.Add(prefabToPool);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectPool.Spawn(prefabToPool);
        }
    }
}

public static class ObjectPool
{
    private static List<PooledObject> pool;
    private static int numPooledObjects;

    public static void Add(GameObject toPool, int numberToPreload = 20)
    {
        // Add type of gameobject to pool and spawn
        pool = new List<PooledObject>(); //new PooledObject[numberToPreload];
        numPooledObjects = numberToPreload;

        for (int i = 0; i < numPooledObjects; i++)
        {

            GameObject obj = InstantiateObj(toPool);
            obj.SetActive(false);
        }
    }

    private static GameObject InstantiateObj(GameObject toPool)
    {
        GameObject obj = UnityEngine.Object.Instantiate(toPool);
        PooledObject pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.Initialize();
        pool.Add(pooledObj);
        return obj;
    }

    public static GameObject Spawn(GameObject pooledObject) => Spawn(pooledObject, null, Vector3.zero, Quaternion.identity);
    public static GameObject Spawn(GameObject pooledObject, Vector3 position) => Spawn(pooledObject, null, position, Quaternion.identity);

    public static GameObject Spawn(GameObject pooledObject, Transform parent, Vector3 position, Quaternion rotation)
    {
        // Get gameobject from pool
        PooledObject retrievedObj = FindInactiveObj();
        GameObject obj;
        if (ReferenceEquals(retrievedObj, null))
        {
            numPooledObjects++;
            obj = InstantiateObj(pooledObject);
        }
        else
        {
            obj = retrievedObj.gameObject;
            obj.SetActive(true);
        }

        obj.transform.parent = parent;
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    private static PooledObject FindInactiveObj()
    {
        for (int i = 0; i < numPooledObjects; i++)
        {
            Debug.Log($"Looking at i = {i} which is {pool[i].gameObject.name}");
            PooledObject obj = pool[i];
            if (!obj.gameObject.activeSelf)
                return obj;
        }

        return null;
    }

    public static void Despawn(GameObject toDespawn)
    {
        // Return gameobject to pool
        for (int i = 0; i < numPooledObjects; i++)
        {
            PooledObject obj = pool[i];
            if (ReferenceEquals(obj.gameObject, toDespawn))
            {
                obj.gameObject.SetActive(false);
                return;
            }
        }
    }
}

public class PooledObject : MonoBehaviour {
    public virtual void Initialize()
    {
        
    }

    public virtual void CleanUp()
    {
        
    }
}