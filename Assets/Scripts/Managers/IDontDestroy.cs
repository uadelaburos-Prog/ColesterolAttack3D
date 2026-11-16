using System;
using UnityEngine;

public abstract class IDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Instance => instance;
    public static bool HasInstance => (T)instance != null;

    protected virtual void Awake()
    {
        if (HasInstance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this as T;
        OnSingletonAwake();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    // Usá esto en vez de Awake en las clases hijas
    protected virtual void OnSingletonAwake() { }
}

public abstract class SingletonBase<T> where T : SingletonBase<T>, new()
{
    static readonly Lazy<T> lazy = new Lazy<T>(() => new T());
    public static T Instance => lazy.Value;
}