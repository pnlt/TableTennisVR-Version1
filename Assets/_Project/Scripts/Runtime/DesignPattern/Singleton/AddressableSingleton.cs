using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    /*Addressables.LoadAssetsAsync<GameObject>("Manager").Completed += objs =>
                    {
                            if (objs.Status == AsyncOperationStatus.Succeeded)
                            {
                                foreach (var obj in objs.Result)
                                {
                                    var go = Instantiate(obj);
                                    go.name = typeof(T).Name;
                                    _instance = go.GetComponent<T>();

                                }
                            }
                            else
                            {
                                _instance = CreateInstance();
                            }
                    };*/

                    var obs = Addressables.LoadAssetsAsync<GameObject>("Manager");
                    obs.WaitForCompletion();

                    if (obs.Status == AsyncOperationStatus.Succeeded)
                    {
                        foreach (var obj in obs.Result)
                        {
                            var go = Instantiate(obj);
                            go.name = typeof(T).Name;
                            _instance = go.GetComponent<T>();
                        }
                    }
                    else
                    {
                        _instance = CreateInstance();
                    }
                }
                else
                {
                    _instance = CreateInstance();
                }
            }

            return _instance;
        }
    }

    private static T CreateInstance()
    {
        GameObject newObj = new GameObject();
        newObj.name = typeof(T).Name;
        newObj.SetActive(true);
        return newObj.AddComponent<T>();
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}