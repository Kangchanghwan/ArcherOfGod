using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{

    private static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindFirstObjectByType(typeof(T));
                if (_instance == null)
                {
                    SetupInstance();
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        RemoveDuplicates();
    }
    private static void SetupInstance()
    {
        _instance = (T)FindFirstObjectByType(typeof(T));
        if (_instance == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = typeof(T).Name;
            gameObject.AddComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void RemoveDuplicates()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
