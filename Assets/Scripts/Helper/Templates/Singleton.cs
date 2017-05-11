﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Helper class
/// Inherit from Singleton and use the class as T
/// </summary>
/// <typeparam name="T">
/// Class name that will be the singleton.
/// T will be a MonoBehaviour.
/// </typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance as T;
        }
    }

    public bool destroyOnLoad;

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (!destroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject.GetComponent<T>());
    }
}
