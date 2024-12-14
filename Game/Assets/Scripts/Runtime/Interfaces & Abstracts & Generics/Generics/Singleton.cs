using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<T>(true);
            return _instance;
        }
    }


    protected virtual void Awake() => _instance = this as T;
}
