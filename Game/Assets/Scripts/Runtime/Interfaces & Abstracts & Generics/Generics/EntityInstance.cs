using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInstance<T> : AEntity where T : EntityInstance<T>
{
    private static T _instance;
    public static T instance
    {

        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>(true);
            }
            return _instance;
        }
    }



    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
