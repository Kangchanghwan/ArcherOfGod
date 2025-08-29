using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private int poolSize = 30;

    private Dictionary<GameObject, IObjectPool<GameObject>> _poolDictionary =
        new Dictionary<GameObject, IObjectPool<GameObject>>();


    public void Start()
    {
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (_poolDictionary.ContainsKey(prefab) == false)
        {
            InitializeNewPool(prefab);
        }

        return _poolDictionary[prefab].Get();
    }

    public void ReturnObject(GameObject objectToReturn, float delay = .001f)
    {
        if (!objectToReturn.activeInHierarchy) return; // 이미 비활성화됨

        if (delay > 0)
        {
            StartCoroutine(DelayReturn(objectToReturn, delay));
        }
        else
        {
            ReturnToPool(objectToReturn);
        }
    }

    private IEnumerator DelayReturn(GameObject objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(objectToReturn);
    }

    private void ReturnToPool(GameObject objectToReturn)
    {
        var pooledObject = objectToReturn.GetComponent<PooledObject>();
        if (pooledObject != null && pooledObject.originalPrefab != null &&
            _poolDictionary.ContainsKey(pooledObject.originalPrefab) == true)
        {
            _poolDictionary[pooledObject.originalPrefab].Release(objectToReturn);
        }
        else
        {
            Debug.LogWarning($"Cannot return object to pool: {objectToReturn.name}. " +
                             "PooledObject component or original prefab not found.");
            Destroy(objectToReturn);
        }
    }

    private void InitializeNewPool(GameObject prefab)
    {
        _poolDictionary[prefab] = new ObjectPool<GameObject>(
            createFunc: () => CreateNewObject(prefab),
            actionOnGet: obj => OnGetFromPool(obj),
            actionOnRelease: obj => OnReturnToPool(obj),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: poolSize,
            maxSize: poolSize
        );
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        var pooledObject = newObject.AddComponent<PooledObject>();
        pooledObject.originalPrefab = prefab;
        return newObject;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.parent = null;
    }

    private void OnReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = transform;
    }
}
