using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

public class ObjectPool : Singleton<ObjectPool>
{

    [SerializeField] private int poolSize = 10;

    [SerializeField]
    private readonly Dictionary<GameObject, Queue<GameObject>> _poolDictionary = new ();
    

    public GameObject GetObject(GameObject prefab,Transform target)
    {
        if (_poolDictionary.ContainsKey(prefab) == false)
        {
            InitializeNewPool(prefab);
        }

        if (_poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab); // if all objects of this type are in uise, create a new one.

        GameObject objectToGet = _poolDictionary[prefab].Dequeue();

        objectToGet.transform.position = target.position;

        objectToGet.SetActive(true);

        return objectToGet;
    }

    public async UniTask ReturnObject(GameObject objectToReturn, float delay = .001f)
    {
        await DelayReturn(delay, objectToReturn);
    }

    private async UniTask DelayReturn(float delay,GameObject objectToReturn)
    {
        await UniTask.WaitForSeconds(delay);
        
        ReturnToPool(objectToReturn);
    }

    private void ReturnToPool(GameObject objectToReturn)
    {
        GameObject originalPrefab = objectToReturn.GetComponent<PooledObject>().originalPrefab;

        objectToReturn.SetActive(false);
        
        _poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }

    private void InitializeNewPool(GameObject prefab)
    {
        _poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        newObject.SetActive(false);

        _poolDictionary[prefab].Enqueue(newObject);
    }
}
